; Inno Setup script for EEPROM Programmer

#define GetMajorMinorRevisionString(str FileName) \
  Local[0] = GetVersionNumbersString(FileName), \
  Local[1] = DeleteToFirstPeriod(Local[0]), \
  Local[2] = DeleteToFirstPeriod(Local[0]), \
  Local[3] = DeleteToFirstPeriod(Local[0]), \
  Local[1] + "." + Local[2] + "." + Local[3]

#define MyAppVersion GetMajorMinorRevisionString('.\bin\Release\EEPROMProgrammer.exe')

[Setup]
AppId={{F8D1E91D-770D-447D-9FCA-0760E1D157FB}
AppName=EEPROM Programmer
AppVersion={#MyAppVersion}
AppPublisher=Dana Peters
DefaultDirName={autopf}\EEPROM Programmer
DisableDirPage=yes
DefaultGroupName=.
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=commandline
OutputBaseFilename=EEPROM_Programmer_{#MyAppVersion}_Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: ".\bin\Release\*"; DestDir: "{app}"; Flags: ignoreversion; Excludes: "*.pdb"

[Icons]
Name: "{group}\EEPROM Programmer"; Filename: "{app}\EEPROMProgrammer.exe"

[Run]
Filename: "{app}\EEPROMProgrammer.exe"; Description: "{cm:LaunchProgram,EEPROM Programmer}"; Flags: nowait postinstall skipifsilent
