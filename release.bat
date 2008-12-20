set PATH=%PATH%;d:\Program Files\WinRar;d:\Program Files\PuTTy

set CMD=A -afzip -r0 -o+ -s -ibck -t

set FILES=CHLOE.Compiler.csproj  CHLOE.Compiler.sln OnTheFlyCompiler.cs OnTheFlyCompilerException.cs Properties\AssemblyInfo.cs
set DATA=%FILES% Data\*.xml Data\*.cs Data\*.vb
set DLL=bin\Release\CHLOE.Compiler.dll

set NAME=fly-chloe-1.0-source.zip
call winrar.exe %CMD% %NAME% %FILES%

set NAME=fly-chloe-1.0-source+data.zip
call winrar.exe %CMD% %NAME% %FILES% %DATA%

set NAME=fly-chloe-1.0-lib.zip
call winrar.exe %CMD% %NAME% %DLL%

set NAME=fly-chloe-1.0-lib+data.zip
call winrar.exe %CMD% %NAME% %DLL% %DATA%

call psftp.exe -b psftp.bat abatishchev@web.sourceforge.net

set DIST=..\..\release\chloe-1.0

move /y fly-chloe-1.0-lib.zip %DIST%
move /y  fly-chloe-1.0-lib+data.zip %DIST%
move /y  fly-chloe-1.0-source.zip %DIST%
move /y  fly-chloe-1.0-source+data.zip %DIST%
