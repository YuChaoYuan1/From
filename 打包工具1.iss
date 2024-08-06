
#define  appVer "1.0.0.2"
[Setup]
AppName=LDJM
AppVersion={#appVer}
VersionInfoVersion={#appVer}
WizardStyle=modern
;安装路径
DefaultDirName=D:\LD
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\LYForms.exe
;压缩相关
Compression=lzma2
SolidCompression=yes
;输出文件路径
OutputDir=installation 
; 输出安装包名称
OutputBaseFilename=LDJM{#appVer}

[Languages]
Name: zh; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Files]
Source: "Resource\LYForms.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "Resource\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Resource\*.config"; DestDir: "{app}\Skin"
Source: "Resource\*.xml"; DestDir: "{app}\Xml"; Flags: uninsneveruninstall 

[Icons]
; 开始菜单快捷方式
Name: "{autoprograms}\LDJM"; Filename: "{app}\LYForms.exe"
; 桌面快捷方式
Name: "{autodesktop}\LDJM"; Filename: "{app}\LYForms.exe"

[Run]
Filename: "{app}\README.TXT"; Description: "查阅Readme.txt"; Flags: postinstall shellexec skipifsilent unchecked
;Filename: "{app}\initialize.EXE"; Description: "执行初始化"; Flags: postinstall nowait skipifsilent 