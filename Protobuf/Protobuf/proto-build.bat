
rem ProtoConfigGenerator.exe所在的位置
SET GeneratorPath=Generator\ProtoConfigGenerator.exe
rem protoc.exe所在的位置
SET ProtocPath=protoc-3.11.4-win64\bin\protoc.exe

rem proto文件的后缀
SET ProtoFileExt=.proto

rem 获取输入的配置文件proto-config.xml
SET ProtoConfigPath=%1

rem 获取输入的脚本输出目录
SET ScriptOutputRootDir=%2

rem 获取导出的语言类型(CSharp/Lua/CPlusPlus)
SET ScriptWriterType=%3
if "%ScriptWriterType%"=="CSharp" (
    SET ScriptOutputFolderName=csharp
)

rem 获取导出脚本使用目标，是服务器还客户端(Client/Server)
SET ScriptTargetType=%4
IF /I "%ScriptTargetType%"=="Client" (
    SET ScriptOutputTargetFolderName=client
) else (
    SET ScriptOutputTargetFolderName=server
)

rem 最终脚本的输出目录
SET ScriptOutputDir=%ScriptOutputRootDir%\%ScriptOutputTargetFolderName%\%ScriptOutputFolderName%
IF NOT EXIST %ScriptOutputDir% (
    MD %ScriptOutputDir%
)

rem 获取proto文件所在的目录
SET ProtoInputFileDir=%5

IF /I "%ScriptTargetType%"=="Client" (
    ECHO ----开始生成 -客户端- 网络消息的描述脚本文件----
) else (
    ECHO ----开始生成 -服务器- 网络消息的描述脚本文件----
)

rem 调用ProtoConfigGenerator生成消息的描述脚本
SET ExportType=Recognizer
%GeneratorPath% -i %ProtoConfigPath% -o %ScriptOutputDir% -w %ScriptWriterType% -e %ExportType%

if /I "%ScriptTargetType%"=="Client" (
    ECHO ----生成  -客户端-  的消息解析脚本----
) else (
    ECHO ----生成  -服务器-  的消息解析脚本----
)

rem ProtoConfigGenerator生成客户端或者服务器的消息解析脚本
SET ExportType=Parser
%GeneratorPath% -i %ProtoConfigPath% -o %ScriptOutputDir% -w %ScriptWriterType% -e %ExportType% -p %ScriptTargetType%

if /I "%ScriptTargetType%"=="Client" (
    ECHO ----转换Proto文件为  -客户端-  脚本----
) else (
    ECHO ----转换Proto文件为  -服务器-  脚本-----
)

FOR /R %%f IN (%ProtoInputFileDir%\*%ProtoFileExt%) do (
    IF /I "%ScriptWriterType%"=="CSharp" (
        %ProtocPath% --csharp_out=%ScriptOutputDir% --proto_path=%ProtoInputFileDir% %%~nxf
    )
)

ECHO ----处理完成----