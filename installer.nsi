; Name of the installer
OutFile "LogvideoRecorder_Installer.exe"

; Name of the application
Name "LogvideoRecorder"

; Installation directory (user's AppData folder)
InstallDir "$LOCALAPPDATA\LogvideoRecorder"

; No need for admin rights
RequestExecutionLevel user

; Pages to display
Page directory
Page instfiles

; Default section
Section ""

; Set output path to the installation directory
SetOutPath $INSTDIR

; Copy files to the installation directory
File /r "bin\Release\net8.0-windows\win-x64\*.*"

; Create a shortcut on the Desktop
CreateShortCut "$DESKTOP\LogvideoRecorder.lnk" "$INSTDIR\LogvideoRecorder.exe"

; Create a shortcut in the Start Menu
CreateDirectory "$SMPROGRAMS\LogvideoRecorder"
CreateShortCut "$SMPROGRAMS\LogvideoRecorder\Uninstall.lnk" "$INSTDIR\uninstall.exe"
CreateShortCut "$SMPROGRAMS\LogvideoRecorder\LogvideoRecorder.lnk" "$INSTDIR\LogvideoRecorder.exe"

; Write uninstall information to the registry (in user's space)
WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\LogvideoRecorder" "DisplayName" "LogvideoRecorder"
WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\LogvideoRecorder" "UninstallString" "$INSTDIR\uninstall.exe"
WriteUninstaller "$INSTDIR\uninstall.exe"

SectionEnd

; Uninstaller section
Section "Uninstall"

; Remove the installed files
Delete "$INSTDIR\*.*"
RMDir "$INSTDIR"

; Remove shortcuts
Delete "$DESKTOP\LogvideoRecorder.lnk"
Delete "$SMPROGRAMS\LogvideoRecorder\Uninstall.lnk"
Delete "$SMPROGRAMS\LogvideoRecorder\LogvideoRecorder.lnk"
RMDir "$SMPROGRAMS\LogvideoRecorder"

; Remove uninstall information from the user's registry
DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\LogvideoRecorder"

SectionEnd