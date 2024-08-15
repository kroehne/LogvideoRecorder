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

; Variables
Var DOTNET_INSTALLED

; Function to check if .NET 8 is installed
Function IsDotNet8Installed
    ReadRegStr $DOTNET_INSTALLED HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\{6CB9D39B-655E-4DD9-A6DE-3AEB07A1C5E0}" "DisplayName"
    StrCmp $DOTNET_INSTALLED "" 0 +2
    ReadRegStr $DOTNET_INSTALLED HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\{6CB9D39B-655E-4DD9-A6DE-3AEB07A1C5E0}" "DisplayName"
FunctionEnd

; Function to download and install .NET 8 runtime
Function InstallDotNet8
    StrCpy $1 "$TEMP\dotnet-installer.exe"
    NSISdl::download "https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-8.0.0-windows-x64-installer" $1
    ExecWait '"$1" /install /quiet /norestart'
    Delete $1
FunctionEnd

; Default section
Section ""

; Set output path to the installation directory
SetOutPath $INSTDIR

; Check if .NET 8 is installed, if not, install it
Call IsDotNet8Installed
StrCmp $DOTNET_INSTALLED "" +2
Goto DotNet8Installed

Call InstallDotNet8

DotNet8Installed:

; Copy files to the installation directory
File /r "bin\Release\net8.0-windows\*.*"

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