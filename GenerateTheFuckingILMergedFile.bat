@echo off
del Launcher.exe /f /s /q
@echo merging...
ilmerge /t:winexe /ndebug /out:Launcher.exe /v4 %~dp0WebLogger.EVE.Ver1\bin\Release\EVELoggerV2.exe Microsoft.VisualBasic.PowerPacks.Vs.dll Microsoft.VisualBasic.PowerPacks.Vs.resources.dll
@echo merge complete.
pause