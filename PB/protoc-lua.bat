echo off

set LUA_PROTO_DIR=.\lua-proto
set LUA_PROTO_EXT=.pb
set PROTO_DIR=.\protos
set PROTO_EXT=.proto

if exist %LUA_PROTO_DIR% (
    rd /s /q %LUA_PROTO_DIR%
)

mkdir %LUA_PROTO_DIR%

for /R %%f in (%PROTO_DIR%\*%PROTO_EXT%) do (
    protoc-3.11.2.exe --descriptor_set_out %LUA_PROTO_DIR%\%%~nf%LUA_PROTO_EXT% %PROTO_DIR%\%%~nxf

    if exist %LUA_PROTO_DIR%\%%~nf%LUA_PROTO_EXT% (
        echo %%~nxf Success
    ) else (
        echo %%~nxf Failed
    )
)

pause
    rem echo %%f        E:\DotGameProject\DotGameTools\PB\protos\SearchResult.proto
    rem echo %%~dpf     E:\DotGameProject\DotGameTools\PB\protos\
    rem echo %%~nf      SearchResult
    rem echo %%~xf      .proto
    rem echo %%~nxf     SearchResult.proto
    rem echo %%~dpnf    E:\DotGameProject\DotGameTools\PB\protos\SearchResult