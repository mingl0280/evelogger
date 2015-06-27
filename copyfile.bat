@echo off
@echo copying files...
if %1 equ "" (
del /f /s /q %~dp0ReleaseFiles\*.*
copy %~dp0WebLogger.EVE.Ver1\bin\Release\EVELoggerV2.exe %~dp0ReleaseFiles\Launcher.exe
copy %~dp0WebLogger.EVE.Ver1\bin\Release\Microsoft.WindowsAPICodePack.DirectX.dll %~dp0ReleaseFiles\Microsoft.WindowsAPICodePack.DirectX.dll
) else (
del /f /s /q %~dp0ReleaseFiles\*.*
copy %1WebLogger.EVE.Ver1\bin\Release\EVELoggerV2.exe %1ReleaseFiles\Launcher.exe
copy %1WebLogger.EVE.Ver1\bin\Release\Microsoft.WindowsAPICodePack.DirectX.dll %1ReleaseFiles\Microsoft.WindowsAPICodePack.DirectX.dll
)
@echo copy OK