@ECHO OFF
ECHO ����������������������������������������������
ECHO MsBuild�����s���܂��B
ECHO ����������������������������������������������
@ECHO ON

MSBuild src/PicSum.sln /p:OutputPath="%CD%/bin" /t:Rebuild /p:Configuration=Release

pause