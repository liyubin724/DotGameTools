
rem ProtoConfigGenerator.exe���ڵ�λ��
SET GeneratorPath=Generator\ProtoConfigGenerator.exe
rem protoc.exe���ڵ�λ��
SET ProtocPath=protoc-3.11.4-win64\bin\protoc.exe

rem proto�ļ��ĺ�׺
SET ProtoFileExt=.proto

rem ��ȡ����������ļ�proto-config.xml
SET ProtoConfigPath=%1

rem ��ȡ����Ľű����Ŀ¼
SET ScriptOutputRootDir=%2

rem ��ȡ��������������(CSharp/Lua/CPlusPlus)
SET ScriptWriterType=%3
if "%ScriptWriterType%"=="CSharp" (
    SET ScriptOutputFolderName=csharp
)

rem ��ȡ�����ű�ʹ��Ŀ�꣬�Ƿ��������ͻ���(Client/Server)
SET ScriptTargetType=%4
IF /I "%ScriptTargetType%"=="Client" (
    SET ScriptOutputTargetFolderName=client
) else (
    SET ScriptOutputTargetFolderName=server
)

rem ���սű������Ŀ¼
SET ScriptOutputDir=%ScriptOutputRootDir%\%ScriptOutputTargetFolderName%\%ScriptOutputFolderName%
IF NOT EXIST %ScriptOutputDir% (
    MD %ScriptOutputDir%
)

rem ��ȡproto�ļ����ڵ�Ŀ¼
SET ProtoInputFileDir=%5

IF /I "%ScriptTargetType%"=="Client" (
    ECHO ----��ʼ���� -�ͻ���- ������Ϣ�������ű��ļ�----
) else (
    ECHO ----��ʼ���� -������- ������Ϣ�������ű��ļ�----
)

rem ����ProtoConfigGenerator������Ϣ�������ű�
SET ExportType=Recognizer
%GeneratorPath% -i %ProtoConfigPath% -o %ScriptOutputDir% -w %ScriptWriterType% -e %ExportType%

if /I "%ScriptTargetType%"=="Client" (
    ECHO ----����  -�ͻ���-  ����Ϣ�����ű�----
) else (
    ECHO ----����  -������-  ����Ϣ�����ű�----
)

rem ProtoConfigGenerator���ɿͻ��˻��߷���������Ϣ�����ű�
SET ExportType=Parser
%GeneratorPath% -i %ProtoConfigPath% -o %ScriptOutputDir% -w %ScriptWriterType% -e %ExportType% -p %ScriptTargetType%

if /I "%ScriptTargetType%"=="Client" (
    ECHO ----ת��Proto�ļ�Ϊ  -�ͻ���-  �ű�----
) else (
    ECHO ----ת��Proto�ļ�Ϊ  -������-  �ű�-----
)

FOR /R %%f IN (%ProtoInputFileDir%\*%ProtoFileExt%) do (
    IF /I "%ScriptWriterType%"=="CSharp" (
        %ProtocPath% --csharp_out=%ScriptOutputDir% --proto_path=%ProtoInputFileDir% %%~nxf
    )
)

ECHO ----�������----