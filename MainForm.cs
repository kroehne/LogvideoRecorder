using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System; 
using System.Runtime.InteropServices; 
using System.Collections;
using System.Drawing.Imaging;
using System.Security.Cryptography; 
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using System.Globalization;
using System.IO.Compression;
using System.Data.SQLite;
using LogvideoRecorder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LogvideoRecorderWinformsAndWebview2
{
    public partial class MainForm : Form
    {


        private static int imageCounter = 0;
        private static string currentImageName = "";
        private static int interactionCounter = 0;

        private const string Application_Title = "CBA Documentation Recorder";
        private const int WebView_Border_Top = 90;
        private static int WebView_Width = 1280;
        private static int WebView_Height = 768;

        private static int form_x = 0;
        private static int form_y = 0;
        private static int form_width = 0;
        private static int form_height = 0;

        #region low-level mouse tacking

        // Delegates for hook callbacks
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static HookProc _mouseProc = MouseHookCallback;
        private static HookProc _keyboardProc = KeyboardHookCallback;
        private static IntPtr _mouseHookID = IntPtr.Zero;
        private static IntPtr _keyboardHookID = IntPtr.Zero;

        // Import necessary functions from user32.dll
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // Constants for mouse and keyboard hooks
        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_MBUTTONDOWN = 0x0207;
        private const int WM_MBUTTONUP = 0x0208;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll")]
        private static extern IntPtr GetCursor();

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        private const int IDC_ARROW = 32512;
        private const int IDC_HAND = 32649;
        private const int IDC_IBEAM = 32513;
        private const int IDC_WAIT = 32514;
        private const int IDC_CROSS = 32515;
        private const int IDC_UPARROW = 32516;
        private const int IDC_SIZE = 32640;

        private static IntPtr hArrowCursor;
        private static IntPtr hHandCursor;
        private static IntPtr hIBeamCursor;
        private static IntPtr hWaitCursor;
        private static IntPtr hCrossCursor;
        private static IntPtr hUpArrowCursor;
        private static IntPtr hSizeCursor;

        // Set the hook using the provided procedure
        private static IntPtr SetHook(HookProc proc, int idHook)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(idHook, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        // Callback for mouse events
        private static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);

                int posx = hookStruct.pt.x - form_x;
                int posy = hookStruct.pt.y - form_y;

                IntPtr currentCursor = GetCursor();
                string cursorType = "Unknown";

                // Compare current cursor handle with known cursors
                if (currentCursor == hArrowCursor)
                {
                    cursorType = "Arrow";
                }
                else if (currentCursor == hHandCursor)
                {
                    cursorType = "Hand";
                }
                else if (currentCursor == hIBeamCursor)
                {
                    cursorType = "IBeam";
                }
                else if (currentCursor == hWaitCursor)
                {
                    cursorType = "Wait";
                }
                else if (currentCursor == hCrossCursor)
                {
                    cursorType = "Cross";
                }
                else if (currentCursor == hUpArrowCursor)
                {
                    cursorType = "Up Arrow";
                }
                else if (currentCursor == hSizeCursor)
                {
                    cursorType = "Size";
                }

                if (posx >= 0 && posx <= WebView_Width && posy >= 0 && posy <= WebView_Height)
                {
                    string eventtype = "mouse_move";
                    string arguments = $"{posx}\t{posy}\t{cursorType}";
                    switch ((int)wParam)
                    {
                        case WM_LBUTTONDOWN:
                            eventtype = $"left_button_down";
                            break;
                        case WM_LBUTTONUP:
                            eventtype = $"left_button_up";
                            break;
                        case WM_RBUTTONDOWN:
                            eventtype = $"right_button_down";
                            break;
                        case WM_RBUTTONUP:
                            eventtype = $"right_button_up";
                            break;
                        case WM_MBUTTONDOWN:
                            eventtype = $"middle_button_down";
                            break;
                        case WM_MBUTTONUP:
                            eventtype = $"middle_button_up";
                            break;
                        case WM_MOUSEWHEEL:
                            // Determine scroll direction and amount
                            int delta = (short)((hookStruct.mouseData >> 16) & 0xffff);
                            eventtype = delta > 0 ? $"wheel_up" : $"wheel_down";
                            arguments = $"{posx}\t{posy}\t{cursorType}\t{delta}";
                            break;
                    }

                    LogEvent(eventtype, arguments);
                }


            }
            return CallNextHookEx(_mouseHookID, nCode, wParam, lParam);
        }

        // Callback for keyboard events
        private static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                string eventtype = "";
                string arguments = "";

                switch ((int)wParam)
                {
                    case WM_KEYDOWN:
                        eventtype = "key_down";
                        arguments = $"{((Keys)vkCode).ToString()}";
                        break;
                    case WM_KEYUP:
                        eventtype = "key_up";
                        arguments = $"{((Keys)vkCode).ToString()}";
                        break;
                }

                LogEvent(eventtype, arguments);
            }
            return CallNextHookEx(_keyboardHookID, nCode, wParam, lParam);
        }

        // Method to log events in the TextBox
        private static void LogEvent(string type, string arguments)
        {

            if (Application.OpenForms.Count > 0)
            {
                var form = Application.OpenForms[0] as MainForm;
                form?.Invoke((Action)(() =>
                {
                    form.statusLabelMousePosition.Text = "Last Interaction: " + type + "_" + arguments.Replace("\t", "_");
                    if (isRecording)
                        form.CaptureScreenshot("Interaction");
                }));
            }

            if (isRecording && currentImageName != "")
            {
                long timestamp = DateTime.Now.Ticks;
                File.AppendAllText(interactionLogCSV, $"{interactionCounter++}\t{currentImageName}\t{timestamp}\tinteraction\t{type}\t{arguments}" + Environment.NewLine);
            }

        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        #endregion

        private System.Windows.Forms.Timer screenshotTimer;
        private int ScreenshotCounter = 0;



        public MainForm()
        {
            InitializeComponent();

            screenshotTimer = new System.Windows.Forms.Timer();
            screenshotTimer.Interval = 50;
            screenshotTimer.Tick += ScreenshotTimer_Tick;

            _mouseHookID = SetHook(_mouseProc, WH_MOUSE_LL);
            _keyboardHookID = SetHook(_keyboardProc, WH_KEYBOARD_LL);

            this.FormClosing += (s, e) =>
            {
                UnhookWindowsHookEx(_mouseHookID);
                UnhookWindowsHookEx(_keyboardHookID);
            };

            statusLabelNumberImagesTaken.Text = "Images: 0";

            Guid newGuid = Guid.NewGuid();
            this.TxtProjectTitle.Text = "myproject-" + newGuid.ToString();
            this.MenuItemStopRecording.Enabled = false;
            this.MenuItemGenerateVideo.Enabled = false;

            this.Move += MainForm_Move;
            this.Resize += MainForm_Resize;

            if (!Directory.Exists(Path.Combine(Application.StartupPath, "output")))
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "output"));

            InitializeCursors();

            InitializeAsync();

        }

        private static void InitializeCursors()
        {
            // Load system cursor handles
            hArrowCursor = LoadCursor(IntPtr.Zero, IDC_ARROW);
            hHandCursor = LoadCursor(IntPtr.Zero, IDC_HAND);
            hIBeamCursor = LoadCursor(IntPtr.Zero, IDC_IBEAM);
            hWaitCursor = LoadCursor(IntPtr.Zero, IDC_WAIT);
            hCrossCursor = LoadCursor(IntPtr.Zero, IDC_CROSS);
            hUpArrowCursor = LoadCursor(IntPtr.Zero, IDC_UPARROW);
            hSizeCursor = LoadCursor(IntPtr.Zero, IDC_SIZE);
        }

        private async void InitializeAsync()
        {
            // Initialize WebView2
            await webView21.EnsureCoreWebView2Async(null);

            var html = @"
        <html>
            <head>
                <title>Start</title>
            </head>
            <body>
                  <h2>LogvideoRecorder</h2>
                  <p>This program is an Edge-based browser (i.e. Webview2, which uses Edge and is technically based on Chrome), that captures images in uncompressed size as well as basic interactions (mouse movements and keystrokes). The images are combined into videos (with and without mouse track).  </p>
                  <p>In order to use the program, the assessment must be accessible via a URL (i.e. via ""http://""), i.e. an ""assessment player"" must be executed as a local server, for example. </p>
                  <p>The recorded videos must be combined with the log events recorded by the assessment software so that they can be viewed together in the corresponding viewer. </p>
                  <hr noshade/>
                  <h3>Quick guide</h3>
                  <ul>
                    <li>1. Copy URL (incl. http://..) into the <i>Adress</i>-bar. The page should be loaded after clicking the ►-button</li>
                    <li>2. Click the <i>Hamburger</i>-icon (≡) and select the window size (e.g., 1024x768).</li>
                    <li>3. Define a <i>project name</i> in the menu (≡) by replacing the text <i>myproject-...</i>.
                    <li>4. Prepare the assessment, i.e. navigate to the last step before the <i>element</i> (i.e., item, unit, ...) of interest.</li>
                    <li>5. Start automatic screen capturing by clicking <i>Start Recording</i> in the menu (≡). Screen capturing is running, if [Recording] is shown in the status bar.</li>
                    <li>6. End the automatic screen capturing by clicking <i>Stop Recording</i>. The recorded images are in the folder that can be found using the menu entry <i>Show Output Folder (Explorer)</i>. 
                    <li>7. Compress the captured images and generate videos (with / without mouse pointer) by clicking <i>Create ZIP Archive and Export Videos</i>.                    
                  </ul>
                  <p>To reset the program and process / record another script, simply define a new <i>project name</i>. </p>
                  <hr noshade/>
                  <p>Notes:</p>
                  <ul>
                    <li>Creating the video (with mouse pointer) can take some time. </li>
                    <li>A lot of hard disk space is temporarily used to create the video that includes the mouse pointer. </li>
                    <li>The zoom setting can be changed with Ctrl/Strg + mouse wheel. It is reset to 1.0 via the ""↔ ↕""-button. </li>
                    <li>All timestamps are ""ticks"". The value of this property represents the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001 in the Gregorian calendar, which represents MinValue.</li>
                    <li>Video creation takes a particularly long time the first time the program is called up, as the necessary program component (ffmpeg) must first be downloaded once. </li>
                  </ul>
                  <hr noshade/>
                  <h3>Specific Instructions</h3>
                  <h4>TIMSS 2019</h4>
                  <p>Log event data of the TIMSS 2019 Player is saved per <i>Student ID</i> in a SQLLite database file with the following file name:</p>
                  <ul><i>eAssessmentResults_{Player-Name}_{Student ID}_{Timestamp in UNIX epoch format}_{Index}.db</i></ul>
                  <p>To create the combined documentation file, the <i>*.db</i> file must be copied into the <i>data</i> sub-directory. The documentation files are created in the sub-directory <i>output</i> via the menu item <i>Combine Event Data / TIMSS 2019<i>.</p>    
            </body>
        </html>";

            webView21.NavigateToString(html);

            // Handle the ContextMenuRequested event
            webView21.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;

            this.GotFocus += WebView21_GotFocus;
            this.LostFocus += WebView21_LostFocus;

            webView21.GotFocus += WebView21_GotFocus;
            webView21.LostFocus += WebView21_LostFocus;

            screenshotTimer.Start();

            ComboSettingScreenSizes.SelectedIndex = 3;
        }

        private void WebView21_LostFocus(object? sender, EventArgs e)
        {
            LogEvent("lost_focus", sender.ToString());
        }

        private void WebView21_GotFocus(object? sender, EventArgs e)
        {
            LogEvent("got_focus", sender.ToString());
        }

        private void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e)
        {
            // Set Handled to true to suppress the default context menu
            e.Handled = true;
        }

        private void ScreenshotTimer_Tick(object? sender, EventArgs e)
        {
            if (isRecording)
                CaptureScreenshot("Timer");
        }

        Bitmap lastImage = null;
        bool processing = false;
        private async void CaptureScreenshot(string reason)
        {
            if (!processing && webView21.CoreWebView2 != null)
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        processing = true;
                        await webView21.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, stream);
                        Bitmap newImage = new Bitmap(stream);

                        if (lastImage == null || !CompareImagesByHash(lastImage, newImage))
                        {
                            long timestamp = DateTime.Now.Ticks;
                            currentImageName = $"{imageCounter++}_{timestamp}.png";

                            var resizedImage = ResizeImage(newImage, WebView_Width, WebView_Height);
                            resizedImage.Save(Path.Combine(imageOutputFolder, currentImageName));

                            File.AppendAllText(videoTimestampCSV, $"{currentImageName}\t{timestamp}\t{reason}" + Environment.NewLine);
                            File.AppendAllText(interactionLogCSV, $"{interactionCounter++}\t{currentImageName}\t{timestamp}\tidle" + Environment.NewLine);

                            statusLabelNumberImagesTaken.Text = "Images: " + imageCounter;
                            lastImage = newImage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    long timestamp = DateTime.Now.Ticks;
                    File.AppendAllText(interactionLogCSV, $"{interactionCounter++}\t{currentImageName}\t{timestamp}\texception\t{ex.Message}" + Environment.NewLine);
                }
                processing = false;
            }
        }

        private Bitmap ResizeImage(Bitmap originalImage, int width, int height)
        {
            var resizedImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                graphics.DrawImage(originalImage, 0, 0, width, height);
            }
            return resizedImage;
        }

        private bool CompareImagesByHash(Bitmap bmp1, Bitmap bmp2)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash1 = GetImageHash(bmp1, md5);
                byte[] hash2 = GetImageHash(bmp2, md5);

                return StructuralComparisons.StructuralEqualityComparer.Equals(hash1, hash2);
            }
        }

        private byte[] GetImageHash(Bitmap bmp, HashAlgorithm hashAlgorithm)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                return hashAlgorithm.ComputeHash(stream);
            }
        }

        private void MainForm_Resize(object? sender, EventArgs e)
        {
            MainForm_UpdatePosition();
        }

        private void MainForm_Move(object? sender, EventArgs e)
        {
            MainForm_UpdatePosition();
        }

        private void MainForm_UpdatePosition()
        {
            form_x = this.Location.X + webView21.Left + (this.Width - webView21.Width) / 2;
            form_y = this.Location.Y + WebView_Border_Top;
            form_width = this.Width - webView21.Left - webView21.Right;
            form_height = this.Height - webView21.Location.Y - webView21.Bottom;
            statusLabelMain.Text = "Current Size: " + webView21.Width.ToString() + " x " + webView21.Height.ToString();
        }


        private void BtnBack_Click(object sender, EventArgs e)
        {
            webView21.GoBack();
        }

        private void BtnForward_Click(object sender, EventArgs e)
        {
            webView21.GoForward();
        }

        private void BtnReload_Click(object sender, EventArgs e)
        {
            webView21.Reload();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            webView21.Stop();
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            LoadUrl(CbxURL.Text);
        }

        private void webView21_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            addURL(webView21.Source.ToString());
        }

        private void webView21_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            PnlNavControl.Enabled = true;
            EnableNavigationButtons(false);
            webView21.CoreWebView2.DocumentTitleChanged += webView21_DocumentTitleChanged;
        }

        private void webView21_DocumentTitleChanged(object? sender, object e)
        {
            Text = Application_Title + " - " + webView21.CoreWebView2.DocumentTitle;
        }

        private void webView21_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            EnableNavigationButtons(true);
        }

        private void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            EnableNavigationButtons(false);
        }

        private void EnableNavigationButtons(bool isNavigating)
        {
            BtnBack.Enabled = webView21.CanGoBack;
            BtnForward.Enabled = webView21.CanGoForward;
            BtnStop.Enabled = isNavigating;
            BtnReload.Enabled = !isNavigating;
        }

        private void LoadUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && (url != webView21.Source.ToString()))
            {
                try
                {
                    webView21.CoreWebView2.Navigate(url);
                }
                catch (Exception ex) { }
            }
        }

        private void printToPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                webView21.CoreWebView2.PrintToPdfAsync(saveFileDialog1.FileName);
            }
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            Point point = new Point(BtnConfig.Location.X, BtnConfig.Location.Y + BtnConfig.Size.Height);
            contextMenuStrip1.Show(PnlNavButton.PointToScreen(point));

        }

        private void dEvToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView21.CoreWebView2.OpenDevToolsWindow();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addURL(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                if (CbxURL.Items.IndexOf(url) == -1)
                {
                    CbxURL.Items.Add(url);
                }
                CbxURL.Text = url;
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(openFileDialog1.FileName);
                    String file = Convert.ToBase64String(bytes);
                    String mimeType;
                    if (openFileDialog1.FilterIndex == 1)
                    {
                        mimeType = @"text/html";
                    }
                    else
                    {
                        mimeType = @"application/pdf";
                    }

                    String uri = "data:" + mimeType + ";charset=utf-8;base64," + Uri.EscapeDataString(file);
                    addURL(uri);
                    LoadUrl(uri);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("There was an error opening that file.\n" + exception.Message,
                        "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnWidth_Click(object sender, EventArgs e)
        {
            AdjustSize();
        }

        private void AdjustSize()
        {
            this.WindowState = FormWindowState.Normal;
            this.Width = WebView_Width + 22;
            this.Height = WebView_Height + 132;
            this.webView21.ZoomFactor = 1;
        }


        private async void exportVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.webView21.Visible = false;

            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);
            await CreateVideoWithoutMouse();
            await CreateVideoWithMouse();

            CreateZip();

            this.webView21.Visible = true;
        }

        private static async Task CreateVideoWithoutMouse()
        {
            List<Tuple<string, DateTime>> imagesWithTimepoints = new List<Tuple<string, DateTime>>();

            var lines = File.ReadAllLines(videoTimestampCSV);
            for (var i = 0; i < lines.Length; i += 1)
            {
                var columns = lines[i].Split("\t");
                if (File.Exists(Path.Combine(imageOutputFolder, columns[0])))
                {
                    imagesWithTimepoints.Add(new Tuple<string, DateTime>(Path.Combine(imageOutputFolder, columns[0]), new DateTime(long.Parse(columns[1]))));
                }

            }

            if (imagesWithTimepoints.Count > 0)
            {
                // Output video path

                string outputVideoPathMP4 = Path.Combine(outputFolder, "video_and_zip", projectName + "_no_mouse.mp4");
                string outputVideoPathwebM = Path.Combine(outputFolder, "video_and_zip", projectName + "_no_mouse.webm");

                if (File.Exists(outputVideoPathMP4))
                    File.Delete(outputVideoPathMP4);

                if (File.Exists(outputVideoPathwebM))
                    File.Delete(outputVideoPathwebM);

                // Calculate frame durations based on time differences
                List<Tuple<string, double>> imagesWithDurations = new List<Tuple<string, double>>();
                for (int i = 0; i < imagesWithTimepoints.Count - 1; i++)
                {
                    string imagePath = imagesWithTimepoints[i].Item1;
                    DateTime currentTime = imagesWithTimepoints[i].Item2;
                    DateTime nextTime = imagesWithTimepoints[i + 1].Item2;

                    double durationSeconds = (nextTime - currentTime).TotalSeconds;
                    imagesWithDurations.Add(new Tuple<string, double>(imagePath, durationSeconds));
                }

                // Add the last image with a default duration if needed
                imagesWithDurations.Add(new Tuple<string, double>(
                    imagesWithTimepoints.Last().Item1, 5)); // Default to 5 seconds for the last image

                // Create a video from the images
                string tempConcatFile = Path.Combine(Path.GetTempPath(), "concat.txt");
                using (StreamWriter sw = new StreamWriter(tempConcatFile))
                {
                    foreach (var item in imagesWithDurations)
                    {
                        sw.WriteLine($"file '{item.Item1}'");
                        sw.WriteLine($"duration {item.Item2.ToString(CultureInfo.InvariantCulture)}");
                    }
                }

                var conversion1 = FFmpeg.Conversions.New()
                    .AddParameter($"-f concat -safe 0 -i \"{tempConcatFile}\" -vf \"scale={WebView_Width}:{WebView_Height}\" -vsync vfr -pix_fmt yuv420p -movflags +faststart {outputVideoPathMP4}");

                await conversion1.Start();

                var conversion2 = await FFmpeg.Conversions.FromSnippet.Convert(outputVideoPathMP4, outputVideoPathwebM);
                await conversion2.Start();

            }

        }

        private static async Task CreateVideoWithMouse()
        {
            List<Tuple<string, DateTime, int, int, string, string, string>> imagesWithTimepoints = new List<Tuple<string, DateTime, int, int, string, string, string>>();


            int lastX = 0;
            int lastY = 0;
            var lines = File.ReadAllLines(interactionLogCSV);
            for (var i = 0; i < lines.Length; i += 1)
            {
                string lastPointer = "Arrow";
                string lastAction = "mouse_move";

                var columns = lines[i].Split("\t");
                if (File.Exists(Path.Combine(imageOutputFolder, columns[1])))
                {
                    if (columns.Count() > 6)
                    {
                        lastX = int.Parse(columns[5]);
                        lastY = int.Parse(columns[6]);
                        lastPointer = columns[7];
                        lastAction = columns[4];
                    }

                    imagesWithTimepoints.Add(new Tuple<string, DateTime, int, int, string, string, string>(Path.Combine(imageOutputFolder, columns[1]),
                                                                new DateTime(long.Parse(columns[2])),
                                                                lastX,
                                                                lastY,
                                                                lastPointer,
                                                                lastAction,
                                                                columns[0]));

                }

            }

            if (imagesWithTimepoints.Count > 0)
            {
                // Output video path

                string videoTempPath = Path.Combine(outputFolder, "tempfolder");

                if (!Directory.Exists(videoTempPath))
                    Directory.CreateDirectory(videoTempPath);

                string outputVideoPathMP4 = Path.Combine(outputFolder, "video_and_zip", projectName + "_mouse.mp4");
                string outputVideoPathwebM = Path.Combine(outputFolder, "video_and_zip", projectName + "_mouse.webm");

                if (File.Exists(outputVideoPathMP4))
                    File.Delete(outputVideoPathMP4);

                if (File.Exists(outputVideoPathwebM))
                    File.Delete(outputVideoPathwebM);

                // Calculate frame durations based on time differences
                List<Tuple<string, double>> imagesWithDurations = new List<Tuple<string, double>>();
                for (int i = 0; i < imagesWithTimepoints.Count - 1; i++)
                {
                    string imagePath = imagesWithTimepoints[i].Item1;
                    DateTime currentTime = imagesWithTimepoints[i].Item2;
                    DateTime nextTime = imagesWithTimepoints[i + 1].Item2;
                    int mouseX = imagesWithTimepoints[i].Item3;
                    int mouseY = imagesWithTimepoints[i].Item4;

                    double durationSeconds = (nextTime - currentTime).TotalSeconds;
                    string tempImagePath = Path.Combine(videoTempPath, imagesWithTimepoints[i].Item7 + ".png");

                    using (Bitmap screenshot = new Bitmap(imagePath))
                    {
                        string pointerType = "Arrow";
                        if (imagesWithTimepoints[i].Item5 == "Arrow" && imagesWithTimepoints[i].Item6 != "mouse_move")
                        {
                            pointerType = "Click";
                        }
                        else if (imagesWithTimepoints[i].Item5 == "Hand")
                        {
                            pointerType = "Arrow"; // TODO
                        }
                        else if (imagesWithTimepoints[i].Item5 == "IBeam")
                        {
                            pointerType = "Arrow"; // TODO
                        }
                        else if (imagesWithTimepoints[i].Item5 == "Wait")
                        {
                            pointerType = "Arrow"; // TODO
                        }
                        else if (imagesWithTimepoints[i].Item5 == "Cross")
                        {
                            pointerType = "Arrow"; // TODO
                        }
                        else if (imagesWithTimepoints[i].Item5 == "Up Arrow")
                        {
                            pointerType = "Arrow"; // TODO
                        }
                        else if (imagesWithTimepoints[i].Item5 == "Size")
                        {
                            pointerType = "Arrow"; // TODO
                        }
                        else
                        {
                            pointerType = "Arrow";
                        }

                        string pointerImagePath = Path.Combine(Application.StartupPath, "images", pointerType + ".png");

                        using (Bitmap pointerImage = new Bitmap(pointerImagePath))
                        {
                            // Create a graphics object to draw on the screenshot
                            using (Graphics graphics = Graphics.FromImage(screenshot))
                            {
                                // Draw the mouse pointer at the specified coordinates
                                graphics.DrawImage(pointerImage, mouseX, mouseY, pointerImage.Width, pointerImage.Height);
                            }

                            // Save the modified screenshot
                            screenshot.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Png);
                        }
                    }

                    imagesWithDurations.Add(new Tuple<string, double>(tempImagePath, durationSeconds));
                }

                // Add the last image with a default duration if needed
                imagesWithDurations.Add(new Tuple<string, double>(
                    imagesWithTimepoints.Last().Item1, 5)); // Default to 5 seconds for the last image

                // Create a video from the images
                string tempConcatFile = Path.Combine(Path.GetTempPath(), "concat.txt");
                using (StreamWriter sw = new StreamWriter(tempConcatFile))
                {
                    foreach (var item in imagesWithDurations)
                    {
                        sw.WriteLine($"file '{item.Item1}'");
                        sw.WriteLine($"duration {item.Item2.ToString(CultureInfo.InvariantCulture)}");
                    }
                }

                var conversion1 = FFmpeg.Conversions.New()
                    .AddParameter($"-f concat -safe 0 -i \"{tempConcatFile}\" -vf \"scale={WebView_Width}:{WebView_Height}\" -vsync vfr -pix_fmt yuv420p -movflags +faststart {outputVideoPathMP4}");

                await conversion1.Start();

                var conversion2 = await FFmpeg.Conversions.FromSnippet.Convert(outputVideoPathMP4, outputVideoPathwebM);
                await conversion2.Start();

                if (Directory.Exists(videoTempPath))
                    Directory.Delete(videoTempPath, recursive: true);
            }

        }

        static void CreateZip()
        {
            string zipFilePath = Path.Combine(outputFolder, "video_and_zip", projectName + "_images.zip");

            // Delete the zip file if it already exists
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }

            // Create a new zip archive
            using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                // Get all image files from the source folder
                string[] imageFiles = Directory.GetFiles(imageOutputFolder, "*.*", SearchOption.TopDirectoryOnly)
                                               .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                           s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                           s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                           s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                                           s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                               .ToArray();

                // Add each image file to the zip archive
                foreach (string imageFile in imageFiles)
                {
                    // Get the file name to use in the zip archive
                    string entryName = Path.GetFileName(imageFile);
                    archive.CreateEntryFromFile(imageFile, entryName);
                }

                archive.CreateEntryFromFile(videoTimestampCSV, Path.GetFileName(videoTimestampCSV));
                archive.CreateEntryFromFile(interactionLogCSV, Path.GetFileName(interactionLogCSV));
            }
        }

        private void TxtProjectTitle_TextChanged(object sender, EventArgs e)
        {
            if (isRecording)
            {
                statusLabelRecording.Text = "[Recording] " + this.TxtProjectTitle.Text;
            }
            else
            {
                statusLabelRecording.Text = "[Stopped] " + this.TxtProjectTitle.Text;
            }

            string _tmpoutputFolder = Path.Combine(Application.StartupPath, "output", this.TxtProjectTitle.Text, "images");

            if (Directory.Exists(_tmpoutputFolder))
            {
                string[] imageFiles = Directory.GetFiles(_tmpoutputFolder, "*.*", SearchOption.TopDirectoryOnly)
                                    .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                                s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                    .ToArray();

                imageCounter = imageFiles.Count();
            }
            else
            {
                imageCounter = 0;
            }
            statusLabelNumberImagesTaken.Text = "Images: " + imageCounter;
        }

        private void ComboSettingScreenSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboSettingScreenSizes.SelectedIndex != -1)
            {
                string _size = ComboSettingScreenSizes.Items[ComboSettingScreenSizes.SelectedIndex].ToString();
                string[] _dims = _size.Split('x');
                WebView_Width = int.Parse(_dims[0]);
                WebView_Height = int.Parse(_dims[1]);
                AdjustSize();
            }

            MainForm_UpdatePosition();
        }

        private static bool isRecording = false;
        private static string projectName = "";
        private static string outputFolder = "";
        private static string imageOutputFolder = "";

        private static string videoTimestampCSV = "";
        private static string interactionLogCSV = "";

        private void Start()
        {
            this.MenuItemStartRecording.Enabled = false;
            this.MenuItemStopRecording.Enabled = true;
            this.TxtProjectTitle.Enabled = false;

            if (this.TxtProjectTitle.Text.Trim() == "")
            {
                Guid newGuid = Guid.NewGuid();
                this.TxtProjectTitle.Text = "myproject-" + newGuid.ToString();
            }

            projectName = this.TxtProjectTitle.Text;

            outputFolder = Path.Combine(Application.StartupPath, "output", projectName);

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            if (!Directory.Exists(Path.Combine(outputFolder, "images")))
                Directory.CreateDirectory(Path.Combine(outputFolder, "images"));

            imageOutputFolder = Path.Combine(outputFolder, "images");

            if (!Directory.Exists(Path.Combine(outputFolder, "data")))
                Directory.CreateDirectory(Path.Combine(outputFolder, "data"));

            if (!Directory.Exists(Path.Combine(outputFolder, "output")))
                Directory.CreateDirectory(Path.Combine(outputFolder, "output"));

            if (!Directory.Exists(Path.Combine(outputFolder, "video_and_zip")))
                Directory.CreateDirectory(Path.Combine(outputFolder, "video_and_zip"));

            if (!Directory.Exists(Path.Combine(outputFolder, "csv")))
                Directory.CreateDirectory(Path.Combine(outputFolder, "csv"));

            videoTimestampCSV = Path.Combine(outputFolder, "csv", "imagelog_timestamp.csv");
            interactionLogCSV = Path.Combine(outputFolder, "csv", "interactionlog_timestamp.csv");

            statusLabelRecording.Text = "[Recording] " + this.TxtProjectTitle;
            isRecording = true;

            CaptureScreenshot("Start");
        }

        private void Stop()
        {
            this.MenuItemStartRecording.Enabled = true;
            this.MenuItemStopRecording.Enabled = false;
            this.MenuItemGenerateVideo.Enabled = true;
            this.TxtProjectTitle.Enabled = true;
            statusLabelRecording.Text = "[Stopped] " + this.TxtProjectTitle;

            CaptureScreenshot("Stop");

            isRecording = false;
            currentImageName = "";

        }

        private void MenuItemStartRecording_Click(object sender, EventArgs e)
        {
            Start();
        }



        private void MenuItemStopRecording_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void showOutputFolderExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("explorer.exe", Path.Combine(Application.StartupPath, "output"));
            }
            catch (Exception ex) { }

        }

        private void CbxURL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                LoadUrl(CbxURL.Text);
        }

        private void tIMSS2019ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Path.Combine(outputFolder, "data")))
            {
                string[] files = Directory.GetFiles(Path.Combine(outputFolder, "data"), "*.db");

                if (files.Length == 1)
                {
                    // Start and Stop Time Stampe

                    var lines = File.ReadAllLines(Path.Combine(outputFolder, "csv", "interactionlog_timestamp.csv"));
                    long time_recording_started = long.MaxValue;
                    long time_recording_ended = long.MinValue;
                    for (var i = 0; i < lines.Length; i += 1)
                    {
                        var columns = lines[i].Split("\t");
                        if (long.Parse(columns[2]) < time_recording_started)
                            time_recording_started = long.Parse(columns[2]);
                        if (long.Parse(columns[2]) > time_recording_ended)
                            time_recording_ended = long.Parse(columns[2]);
                    }

                    // Prepare Root Element 

                    Root root = new Root()
                    {
                        TsBegin = (time_recording_started - 621355968000000000) / 10000,
                        TsBeginTimeString = new DateTime(time_recording_started, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                        TsEnd = (time_recording_ended - 621355968000000000) / 10000,
                        TsEndTimeString = new DateTime(time_recording_ended, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                        TsVideoStart = (time_recording_started - 621355968000000000) / 10000,
                        TsVideoStartTimeString = new DateTime(time_recording_started, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                        Time = (int)((time_recording_ended - time_recording_started) / 10000000)
                    };

                    // TODO: Add open file dialog

                    string connectionString = "Data Source=" + Path.Combine(outputFolder, "data", files[0]) + ";Version=3;";

                    using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                    {
                        connection.Open();

                        Dictionary<int, string> _eventTypes = new Dictionary<int, string>();
                        using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM EventType", connection))
                        {
                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                    _eventTypes.Add(reader.GetInt32(0), reader.GetString(1).Replace(":", ""));
                            }
                        }
                          
                        using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Event", connection))
                        {
                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int _Id = reader.GetInt32(0);
                                    int _EventTypeId = reader.GetInt32(1);
                                    int _ieaInstrumentId = reader.GetInt32(4);
                                    long _CreatedDate = reader.GetInt32(9);
                                    int _SortOrder = reader.GetInt32(11);
                                    string _Information = reader.GetString(10);

                                    // add information

                                    JObject jsonObject = JObject.Parse(_Information);
                                    jsonObject["CreatedDate"] = _CreatedDate;
                                    jsonObject["SortOrder"] = _SortOrder;

                                    if (!reader.IsDBNull(5))
                                    {
                                        int _ieaAiuId = reader.GetInt32(5);
                                        jsonObject["ieaAiuId"] = _ieaAiuId;
                                    }

                                    if (!reader.IsDBNull(8))
                                    {
                                        int _CurrentIndex = reader.GetInt32(8);
                                        jsonObject["CurrentIndex"] = _CurrentIndex;
                                    }

                                    string _UpdatedInformation = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);


                                    long _epoch = (_CreatedDate * 1000 + _SortOrder);
                                    TimeSpan timeSpan = TimeSpan.FromMilliseconds(_epoch) + TimeSpan.FromHours(2);
                                    DateTime unixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime dateTime = unixEpochStart.Add(timeSpan);

                                    root.TraceLogs.Add(new TraceLog()
                                    {
                                        EntryId = "-1",
                                        Name = _eventTypes[_EventTypeId],
                                        Timestamp = (dateTime.Ticks - 621355968000000000) / 10000,
                                        TimestampTimeString = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                        Payload = _UpdatedInformation
                                    });
                                }
                            }
                        }

                        using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Response", connection))
                        {
                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int _Id = reader.GetInt32(0);
                                    long _CreatedDate = reader.GetInt32(5);
                                    int _SortOrder = reader.GetInt32(10);
                                    int _ieaInstrumentId = reader.GetInt32(6);
                                    int _ieaAiuId = reader.GetInt32(7);
                                    
                                    string _Information = "{}";

                                    // add information

                                    JObject jsonObject = JObject.Parse(_Information);
                                    jsonObject["CreatedDate"] = _CreatedDate;
                                    jsonObject["SortOrder"] = _SortOrder;
                                    jsonObject["ieaInstrumentId"] = _ieaInstrumentId;
                                    jsonObject["ieaAiuId"] = _ieaAiuId;

                                    if (!reader.IsDBNull(2))
                                    {
                                        int _ResponseId = reader.GetInt32(2);
                                        jsonObject["ResponseId"] = _ResponseId;
                                    }

                                    if (!reader.IsDBNull(3))
                                    {
                                        string _Answer = reader.GetString(3);
                                        jsonObject["Answer"] = _Answer;
                                    }

                                    if (!reader.IsDBNull(4))
                                    {
                                        string _LocalizedAnswer = reader.GetString(4);
                                        jsonObject["LocalizedAnswer"] = _LocalizedAnswer;
                                    }

                                    if (!reader.IsDBNull(8))
                                    {
                                        int _ieaContentItemId = reader.GetInt32(8);
                                        jsonObject["ieaContentItemId"] = _ieaContentItemId;
                                    }

                                    if (!reader.IsDBNull(9))
                                    {
                                        int _PsiVariableIdentifier = reader.GetInt32(9);
                                        jsonObject["PsiVariableIdentifier"] = _PsiVariableIdentifier;
                                    }

                                    string _UpdatedInformation = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                                     
                                    long _epoch = (_CreatedDate * 1000 + _SortOrder);
                                    TimeSpan timeSpan = TimeSpan.FromMilliseconds(_epoch) + TimeSpan.FromHours(2);
                                    DateTime unixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                                    DateTime dateTime = unixEpochStart.Add(timeSpan);

                                    root.TraceLogs.Add(new TraceLog()
                                    {
                                        EntryId = "-1",
                                        Name = "Response",
                                        Timestamp = (dateTime.Ticks - 621355968000000000) / 10000,
                                        TimestampTimeString = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                        Payload = _UpdatedInformation
                                    });
                                }
                            }
                        }

                        root.TraceLogs = root.TraceLogs.OrderBy(obj => obj.Timestamp).ToList();

                        int _EntryId = 1;
                        foreach (var l in root.TraceLogs)
                            l.EntryId = (_EntryId++).ToString();

                        connection.Close();

                       string json = JsonConvert.SerializeObject(root, Formatting.Indented);
                        File.WriteAllText(Path.Combine(outputFolder, "documentation", "meta.json"), json);

                        string _zipFileForViewer_with_mouse = Path.Combine(outputFolder, "documentation", projectName + "documentation_with_mouse.zip");
                        string _zipFileForViewer_without_mouse = Path.Combine(outputFolder, "documentation", projectName + "documentation_without_mouse.zip");

                        if (File.Exists(_zipFileForViewer_with_mouse))
                            File.Delete(_zipFileForViewer_with_mouse);

                        if (File.Exists(_zipFileForViewer_without_mouse))
                            File.Delete(_zipFileForViewer_without_mouse);

                        string _videoFileForViewer_with_mouse = Path.Combine(outputFolder, "video_and_zip", projectName + "_mouse.mp4");
                        string _videoFileForViewer_without_mouse = Path.Combine(outputFolder, "video_and_zip", projectName + "_no_mouse.mp4");

                        using (ZipArchive archive = ZipFile.Open(_zipFileForViewer_with_mouse, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(Path.Combine(outputFolder, "documentation", "meta.json"), "meta.json");
                            archive.CreateEntryFromFile(_videoFileForViewer_with_mouse, "recording.mp4");
                        }

                        using (ZipArchive archive = ZipFile.Open(_zipFileForViewer_without_mouse, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(Path.Combine(outputFolder, "documentation", "meta.json"), "meta.json");
                            archive.CreateEntryFromFile(_videoFileForViewer_without_mouse, "recording.mp4");
                        }
                    }

                }

            }
          
        }

        
    }
}
