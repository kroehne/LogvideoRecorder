﻿namespace LogvideoRecorderWinformsAndWebview2
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            PnlNavControl = new Panel();
            CbxURL = new ComboBox();
            PnlConfig = new Panel();
            BtnWidth = new Button();
            BtnGo = new Button();
            PnlNavButton = new Panel();
            BtnStop = new Button();
            BtnConfig = new Button();
            BtnReload = new Button();
            BtnForward = new Button();
            BtnBack = new Button();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            statusStrip1 = new StatusStrip();
            statusLabelRecording = new ToolStripStatusLabel();
            statusLabelMain = new ToolStripStatusLabel();
            statusLabelMousePosition = new ToolStripStatusLabel();
            statusLabelNumberImagesTaken = new ToolStripStatusLabel();
            contextMenuStrip1 = new ContextMenuStrip(components);
            TxtProjectTitle = new ToolStripTextBox();
            MenuItemStartRecording = new ToolStripMenuItem();
            MenuItemStopRecording = new ToolStripMenuItem();
            MenuItemGenerateVideo = new ToolStripMenuItem();
            showOutputFolderExplorerToolStripMenuItem = new ToolStripMenuItem();
            combineWithEventDataToolStripMenuItem = new ToolStripMenuItem();
            tIMSS2019ToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            ComboSettingScreenSizes = new ToolStripComboBox();
            toolStripMenuItem2 = new ToolStripSeparator();
            dEvToolsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            openFileToolStripMenuItem = new ToolStripMenuItem();
            printToPDFToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
            PnlNavControl.SuspendLayout();
            PnlConfig.SuspendLayout();
            PnlNavButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            statusStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // PnlNavControl
            // 
            PnlNavControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PnlNavControl.Controls.Add(CbxURL);
            PnlNavControl.Controls.Add(PnlConfig);
            PnlNavControl.Controls.Add(PnlNavButton);
            PnlNavControl.Location = new Point(0, 0);
            PnlNavControl.Name = "PnlNavControl";
            PnlNavControl.Size = new Size(1008, 25);
            PnlNavControl.TabIndex = 0;
            // 
            // CbxURL
            // 
            CbxURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CbxURL.FormattingEnabled = true;
            CbxURL.Items.AddRange(new object[] { "edge://flags/", "edge://gpu/", "edge://about/" });
            CbxURL.Location = new Point(152, 3);
            CbxURL.Name = "CbxURL";
            CbxURL.Size = new Size(770, 23);
            CbxURL.TabIndex = 2;
            CbxURL.Text = "http:// ... (copy URL here)";
            CbxURL.KeyPress += CbxURL_KeyPress;
            // 
            // PnlConfig
            // 
            PnlConfig.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PnlConfig.Controls.Add(BtnWidth);
            PnlConfig.Controls.Add(BtnGo);
            PnlConfig.Location = new Point(922, 0);
            PnlConfig.Name = "PnlConfig";
            PnlConfig.Size = new Size(86, 25);
            PnlConfig.TabIndex = 1;
            // 
            // BtnWidth
            // 
            BtnWidth.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnWidth.Location = new Point(36, 0);
            BtnWidth.Name = "BtnWidth";
            BtnWidth.Size = new Size(49, 25);
            BtnWidth.TabIndex = 6;
            BtnWidth.Text = "↔ ↕";
            BtnWidth.UseVisualStyleBackColor = true;
            BtnWidth.Click += BtnWidth_Click;
            // 
            // BtnGo
            // 
            BtnGo.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnGo.Location = new Point(0, 0);
            BtnGo.Name = "BtnGo";
            BtnGo.Size = new Size(25, 25);
            BtnGo.TabIndex = 4;
            BtnGo.Text = "►";
            BtnGo.UseVisualStyleBackColor = true;
            BtnGo.Click += BtnGo_Click;
            // 
            // PnlNavButton
            // 
            PnlNavButton.Controls.Add(BtnStop);
            PnlNavButton.Controls.Add(BtnConfig);
            PnlNavButton.Controls.Add(BtnReload);
            PnlNavButton.Controls.Add(BtnForward);
            PnlNavButton.Controls.Add(BtnBack);
            PnlNavButton.Location = new Point(0, 0);
            PnlNavButton.Name = "PnlNavButton";
            PnlNavButton.Size = new Size(150, 25);
            PnlNavButton.TabIndex = 0;
            // 
            // BtnStop
            // 
            BtnStop.Font = new Font("Webdings", 12F);
            BtnStop.Location = new Point(121, 1);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new Size(25, 25);
            BtnStop.TabIndex = 3;
            BtnStop.Text = "=";
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += BtnStop_Click;
            // 
            // BtnConfig
            // 
            BtnConfig.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BtnConfig.Location = new Point(4, 1);
            BtnConfig.Name = "BtnConfig";
            BtnConfig.Size = new Size(25, 25);
            BtnConfig.TabIndex = 5;
            BtnConfig.Text = "≡";
            BtnConfig.UseVisualStyleBackColor = true;
            BtnConfig.Click += BtnConfig_Click;
            // 
            // BtnReload
            // 
            BtnReload.Font = new Font("Webdings", 12F);
            BtnReload.Location = new Point(91, 1);
            BtnReload.Name = "BtnReload";
            BtnReload.Size = new Size(25, 25);
            BtnReload.TabIndex = 2;
            BtnReload.Text = "q";
            BtnReload.UseVisualStyleBackColor = true;
            BtnReload.Click += BtnReload_Click;
            // 
            // BtnForward
            // 
            BtnForward.Font = new Font("Webdings", 12F);
            BtnForward.Location = new Point(61, 1);
            BtnForward.Name = "BtnForward";
            BtnForward.Size = new Size(25, 25);
            BtnForward.TabIndex = 1;
            BtnForward.Text = "4";
            BtnForward.UseVisualStyleBackColor = true;
            BtnForward.Click += BtnForward_Click;
            // 
            // BtnBack
            // 
            BtnBack.Font = new Font("Webdings", 12F);
            BtnBack.Location = new Point(34, 1);
            BtnBack.Name = "BtnBack";
            BtnBack.Size = new Size(25, 25);
            BtnBack.TabIndex = 0;
            BtnBack.Text = "3";
            BtnBack.UseVisualStyleBackColor = true;
            BtnBack.Click += BtnBack_Click;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(0, 26);
            webView21.Margin = new Padding(4, 3, 4, 3);
            webView21.Name = "webView21";
            webView21.Size = new Size(1008, 584);
            webView21.TabIndex = 1;
            webView21.ZoomFactor = 1D;
            webView21.CoreWebView2InitializationCompleted += webView21_CoreWebView2InitializationCompleted;
            webView21.NavigationStarting += webView21_NavigationStarting;
            webView21.NavigationCompleted += webView21_NavigationCompleted;
            webView21.SourceChanged += webView21_SourceChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabelRecording, statusLabelMain, statusLabelMousePosition, statusLabelNumberImagesTaken });
            statusStrip1.Location = new Point(0, 608);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1008, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusLabelRecording
            // 
            statusLabelRecording.Name = "statusLabelRecording";
            statusLabelRecording.Size = new Size(12, 17);
            statusLabelRecording.Text = "-";
            // 
            // statusLabelMain
            // 
            statusLabelMain.Name = "statusLabelMain";
            statusLabelMain.Size = new Size(12, 17);
            statusLabelMain.Text = "-";
            // 
            // statusLabelMousePosition
            // 
            statusLabelMousePosition.Name = "statusLabelMousePosition";
            statusLabelMousePosition.Size = new Size(12, 17);
            statusLabelMousePosition.Text = "-";
            // 
            // statusLabelNumberImagesTaken
            // 
            statusLabelNumberImagesTaken.Name = "statusLabelNumberImagesTaken";
            statusLabelNumberImagesTaken.Size = new Size(12, 17);
            statusLabelNumberImagesTaken.Text = "-";
            statusLabelNumberImagesTaken.TextAlign = ContentAlignment.MiddleRight;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { TxtProjectTitle, MenuItemStartRecording, MenuItemStopRecording, MenuItemGenerateVideo, showOutputFolderExplorerToolStripMenuItem, combineWithEventDataToolStripMenuItem, toolStripMenuItem1, ComboSettingScreenSizes, toolStripMenuItem2, dEvToolsToolStripMenuItem, toolStripMenuItem3, openFileToolStripMenuItem, printToPDFToolStripMenuItem, toolStripMenuItem4, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(461, 278);
            // 
            // TxtProjectTitle
            // 
            TxtProjectTitle.BorderStyle = BorderStyle.FixedSingle;
            TxtProjectTitle.Name = "TxtProjectTitle";
            TxtProjectTitle.Size = new Size(400, 23);
            TxtProjectTitle.ToolTipText = "Enter Project Title";
            TxtProjectTitle.TextChanged += TxtProjectTitle_TextChanged;
            // 
            // MenuItemStartRecording
            // 
            MenuItemStartRecording.Name = "MenuItemStartRecording";
            MenuItemStartRecording.Size = new Size(460, 22);
            MenuItemStartRecording.Text = "Start Recording";
            MenuItemStartRecording.Click += MenuItemStartRecording_Click;
            // 
            // MenuItemStopRecording
            // 
            MenuItemStopRecording.Name = "MenuItemStopRecording";
            MenuItemStopRecording.Size = new Size(460, 22);
            MenuItemStopRecording.Text = "Stop Recording";
            MenuItemStopRecording.Click += MenuItemStopRecording_Click;
            // 
            // MenuItemGenerateVideo
            // 
            MenuItemGenerateVideo.Name = "MenuItemGenerateVideo";
            MenuItemGenerateVideo.Size = new Size(460, 22);
            MenuItemGenerateVideo.Text = "Create ZIP Archive and Export Videos";
            MenuItemGenerateVideo.Click += exportVideoToolStripMenuItem_Click;
            // 
            // showOutputFolderExplorerToolStripMenuItem
            // 
            showOutputFolderExplorerToolStripMenuItem.Name = "showOutputFolderExplorerToolStripMenuItem";
            showOutputFolderExplorerToolStripMenuItem.Size = new Size(460, 22);
            showOutputFolderExplorerToolStripMenuItem.Text = "Show Output Folder (Explorer)";
            showOutputFolderExplorerToolStripMenuItem.Click += showOutputFolderExplorerToolStripMenuItem_Click;
            // 
            // combineWithEventDataToolStripMenuItem
            // 
            combineWithEventDataToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tIMSS2019ToolStripMenuItem });
            combineWithEventDataToolStripMenuItem.Name = "combineWithEventDataToolStripMenuItem";
            combineWithEventDataToolStripMenuItem.Size = new Size(460, 22);
            combineWithEventDataToolStripMenuItem.Text = "Combine With Event Data";
            // 
            // tIMSS2019ToolStripMenuItem
            // 
            tIMSS2019ToolStripMenuItem.Name = "tIMSS2019ToolStripMenuItem";
            tIMSS2019ToolStripMenuItem.Size = new Size(180, 22);
            tIMSS2019ToolStripMenuItem.Text = "TIMSS 2019";
            tIMSS2019ToolStripMenuItem.Click += tIMSS2019ToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(457, 6);
            // 
            // ComboSettingScreenSizes
            // 
            ComboSettingScreenSizes.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboSettingScreenSizes.Items.AddRange(new object[] { "1024 x 600", "1024 x 768 ", "1280 x 720 ", "1280 x 768 ", "1280 x 800", "1366 x 768", "1920 x 1080" });
            ComboSettingScreenSizes.Name = "ComboSettingScreenSizes";
            ComboSettingScreenSizes.Size = new Size(400, 23);
            ComboSettingScreenSizes.SelectedIndexChanged += ComboSettingScreenSizes_SelectedIndexChanged;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(457, 6);
            // 
            // dEvToolsToolStripMenuItem
            // 
            dEvToolsToolStripMenuItem.Name = "dEvToolsToolStripMenuItem";
            dEvToolsToolStripMenuItem.Size = new Size(460, 22);
            dEvToolsToolStripMenuItem.Text = "Show DevTools";
            dEvToolsToolStripMenuItem.Click += dEvToolsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(457, 6);
            // 
            // openFileToolStripMenuItem
            // 
            openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            openFileToolStripMenuItem.Size = new Size(460, 22);
            openFileToolStripMenuItem.Text = "Open Local File in Browser";
            openFileToolStripMenuItem.Click += openFileToolStripMenuItem_Click;
            // 
            // printToPDFToolStripMenuItem
            // 
            printToPDFToolStripMenuItem.Name = "printToPDFToolStripMenuItem";
            printToPDFToolStripMenuItem.Size = new Size(460, 22);
            printToPDFToolStripMenuItem.Text = "Print Current Page to PDF (Experimental)";
            printToPDFToolStripMenuItem.Click += printToPDFToolStripMenuItem_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(457, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(460, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "PDF files|*pdf";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "HTML files|*.htm;*.html|PDF files|*.pdf";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 630);
            Controls.Add(statusStrip1);
            Controls.Add(webView21);
            Controls.Add(PnlNavControl);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            PnlNavControl.ResumeLayout(false);
            PnlConfig.ResumeLayout(false);
            PnlNavButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            contextMenuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel PnlNavControl;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private StatusStrip statusStrip1;
        private Panel PnlNavButton;
        private Panel PnlConfig;
        private ComboBox CbxURL;
        private Button BtnBack;
        private Button BtnStop;
        private Button BtnReload;
        private Button BtnForward;
        private Button BtnGo;
        private Button BtnConfig;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem printToPDFToolStripMenuItem;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private ToolStripMenuItem dEvToolsToolStripMenuItem;
        private ToolStripMenuItem openFileToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Button BtnWidth;
        private ToolStripStatusLabel statusLabelMain;
        private ToolStripStatusLabel statusLabelMousePosition;
        private ToolStripMenuItem MenuItemGenerateVideo;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem MenuItemStartRecording;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem MenuItemStopRecording;
        private ToolStripComboBox ComboSettingScreenSizes;
        private ToolStripStatusLabel statusLabelNumberImagesTaken;
        private ToolStripTextBox TxtProjectTitle;
        private ToolStripStatusLabel statusLabelRecording;
        private ToolStripMenuItem showOutputFolderExplorerToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem combineWithEventDataToolStripMenuItem;
        private ToolStripMenuItem tIMSS2019ToolStripMenuItem;
    }
}
