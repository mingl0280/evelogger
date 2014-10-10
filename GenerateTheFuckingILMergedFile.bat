@echo off
@echo merging...
if %1 equ "" ( 
del Launcher.exe /f /s /q
ilmerge /t:winexe /ndebug /out:Launcher.exe /v4 %~dp0WebLogger.EVE.Ver1\bin\Release\EVELoggerV2.exe Microsoft.VisualBasic.PowerPacks.Vs.dll Microsoft.VisualBasic.PowerPacks.Vs.resources.dll
@echo merge complete.
pause
 ) else (
 del %1Launcher.exe /f /s /q
 %1ilmerge /t:winexe /ndebug /out:%1Launcher.exe /v4 %1WebLogger.EVE.Ver1\bin\Release\EVELoggerV2.exe %1Microsoft.VisualBasic.PowerPacks.Vs.dll %1Microsoft.VisualBasic.PowerPacks.Vs.resources.dll
@echo merge complete.
)
