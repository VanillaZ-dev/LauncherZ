!include "MUI2.nsh"
!include "FileFunc.nsh"

Name "LauncherZ"
OutFile "LauncherZ_Setup.exe"
InstallDir "$PROGRAMFILES64\LauncherZ"
RequestExecutionLevel admin
Unicode True

VIProductVersion "1.0.0.0"
VIAddVersionKey "ProductName" "LauncherZ"
VIAddVersionKey "ProductVersion" "1.0.0"
VIAddVersionKey "FileDescription" "LauncherZ - DayZ Launcher"

!define MUI_ABORTWARNING
!define MUI_WELCOMEPAGE_TITLE "Welcome to LauncherZ"
!define MUI_WELCOMEPAGE_TEXT "LauncherZ is a DayZ server browser and launcher.$\n$\nClick Next to continue."
!define MUI_FINISHPAGE_RUN "$INSTDIR\LauncherZ.exe"
!define MUI_FINISHPAGE_RUN_TEXT "Launch LauncherZ now"

!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"

Section "LauncherZ" SecMain
    SectionIn RO
    SetOutPath "$INSTDIR"
    File "LauncherZ.exe"

    WriteRegStr HKLM "Software\LauncherZ" "InstallPath" "$INSTDIR"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ" "DisplayName" "LauncherZ"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ" "UninstallString" '"$INSTDIR\Uninstall.exe"'
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ" "DisplayIcon" "$INSTDIR\LauncherZ.exe"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ" "Publisher" "LauncherZ"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ" "DisplayVersion" "1.0.0"
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ" "NoModify" 1
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ" "NoRepair" 1

    WriteUninstaller "$INSTDIR\Uninstall.exe"
    CreateDirectory "$SMPROGRAMS\LauncherZ"
    CreateShortcut "$SMPROGRAMS\LauncherZ\LauncherZ.lnk" "$INSTDIR\LauncherZ.exe"
    CreateShortcut "$DESKTOP\LauncherZ.lnk" "$INSTDIR\LauncherZ.exe"
SectionEnd

Section "Uninstall"
    Delete "$INSTDIR\LauncherZ.exe"
    Delete "$INSTDIR\Uninstall.exe"
    RMDir "$INSTDIR"
    Delete "$SMPROGRAMS\LauncherZ\LauncherZ.lnk"
    RMDir "$SMPROGRAMS\LauncherZ"
    Delete "$DESKTOP\LauncherZ.lnk"
    DeleteRegKey HKLM "Software\LauncherZ"
    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\LauncherZ"
SectionEnd
