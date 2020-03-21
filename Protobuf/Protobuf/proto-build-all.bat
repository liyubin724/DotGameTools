ECHO OFF

call .\proto-build.bat proto-config.xml output CSharp Client protos
call .\proto-build.bat proto-config.xml output CSharp Server protos

PAUSE