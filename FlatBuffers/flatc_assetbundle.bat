echo off & color 0A
set DIR=%cd%\assetbundle

for /R %DIR% %%f in (*.fbs) do (
  echo %%f
  flatc.exe -n --gen-onefile  -o %DIR% %%f
)

xcopy %DIR%\*.cs ..\..\DotGameClient\Assets\Scripts\Dot\Core\Asset\Bundle /y

pause