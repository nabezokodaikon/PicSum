@ECHO OFF
ECHO ━━━━━━━━━━━━━━━━━━━━━━━
ECHO MsBuildを実行します。
ECHO ━━━━━━━━━━━━━━━━━━━━━━━
@ECHO ON

@Set Path=C:\WINDOWS\Microsoft.NET\Framework64\v4.0.30319;%PATH%
MSBuild src/PicSum.sln /p:OutputPath="%CD%/bin" /t:Rebuild /p:Configuration=Release

pause