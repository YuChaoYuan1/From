
#define  appVer "1.0.0.2"
[Setup]
AppName=LDJM
AppVersion={#appVer}
VersionInfoVersion={#appVer}
WizardStyle=modern
;��װ·��
DefaultDirName=D:\LD
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\LYForms.exe
;ѹ�����
Compression=lzma2
SolidCompression=yes
;����ļ�·��
OutputDir=installation 
; �����װ������
OutputBaseFilename=LDJM{#appVer}

[Languages]
Name: zh; MessagesFile: "compiler:Languages\ChineseSimplified.isl"

[Files]
Source: "Resource\LYForms.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "Resource\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "Resource\*.config"; DestDir: "{app}\Skin"
Source: "Resource\*.xml"; DestDir: "{app}\Xml"; Flags: uninsneveruninstall 

[Icons]
; ��ʼ�˵���ݷ�ʽ
Name: "{autoprograms}\LDJM"; Filename: "{app}\LYForms.exe"
; �����ݷ�ʽ
Name: "{autodesktop}\LDJM"; Filename: "{app}\LYForms.exe"

[Run]
Filename: "{app}\README.TXT"; Description: "����Readme.txt"; Flags: postinstall shellexec skipifsilent unchecked
;Filename: "{app}\initialize.EXE"; Description: "ִ�г�ʼ��"; Flags: postinstall nowait skipifsilent 