echo OFF

for /R %%f in (protos/*.proto) do (
    protoc-3.11.2.exe --descriptor_set_out lua/%%~nf.pb protos/%%~nxf
    echo protos/%%f Finished
)

pause
    rem echo %%f        E:\DotGameProject\DotGameTools\PB\protos\SearchResult.proto
    rem echo %%~dpf     E:\DotGameProject\DotGameTools\PB\protos\
    rem echo %%~nf      SearchResult
    rem echo %%~xf      .proto
    rem echo %%~nxf     SearchResult.proto
    rem echo %%~dpnf    E:\DotGameProject\DotGameTools\PB\protos\SearchResult