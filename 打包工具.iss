; -- Example2.iss --
; Same as Example1.iss, but creates its icon in the Programs folder of the
; Start Menu instead of in a subfolder, and also creates a desktop icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=解密软件
AppVersion=1.1.0.1
VersionInfoVersion=1.1.0.1
WizardStyle=modern
DefaultDirName=解密软件
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\LYForms.exe
Compression=lzma2
SolidCompression=yes
OutputDir=installation 
OutputBaseFilename=解密软件-V1.1.0.1
ChangesAssociations=yes

[Languages]
Name: zh; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Files]
Source: "Resource\LYForms.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "Resource\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Resource\*.config"; DestDir: "{app}\Skin"
Source: "Resource\*.xml"; DestDir: "{app}\Xml"; Flags: uninsneveruninstall 

[Icons]
Name: "{autoprograms}\解密软件"; Filename: "{app}\LYForms.exe"
Name: "{autodesktop}\解密软件"; Filename: "{app}\LYForms.exe"

[Run]
