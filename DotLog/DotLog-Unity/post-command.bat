ECHO OFF

SET TargetExt=.dll
SET CopyTargetDir=..\..\DotLibs\

SET ConfigurationName=%1%
SET TargetName=%2%
SET TargetDir=%3%

SET TargetPath=%TargetDir%%TargetName%%TargetExt%

if "%ConfigurationName%"=="WinEditor" (
    SET CopyTargetName=%TargetName:~0,6%_unity_editor
)else if "%ConfigurationName%"=="Release" (
    SET CopyTargetName=%TargetName:~0,6%_unity
)

SET CopyTargetPath=%~dp0%CopyTargetDir%%CopyTargetName%%TargetExt%

ECHO %CopyTargetPath%

COPY %TargetPath% %CopyTargetPath% /y