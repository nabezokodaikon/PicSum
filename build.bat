@ECHO OFF
ECHO ━━━━━━━━━━━━━━━━━━━━━━━
ECHO MsBuildを実行します。
ECHO ━━━━━━━━━━━━━━━━━━━━━━━
@ECHO ON

MSBuild src/PicSum.sln /p:OutputPath="%CD%/bin" /t:Rebuild /p:Configuration=Release

pause