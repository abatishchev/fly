rem Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

set PATH=%PATH%;d:\Program Files\WinRar;d:\Program Files\PuTTy

set CMD=A -afzip -o+ -s -ibck -t

set FILES=Compiler.cs Errors.cs Events.cs Output.cs Settings.cs Properties\AssemblyInfo.cs
set FILES-LIB=OnTheFlyCompiler.csproj
set FILES-TOOL= OnTheFlyCompiler-Tool.csproj ConsoleStub.cs Core.cs Examples\*.cs Settings\*.xml

set LIB=bin\Release\fly.dll
set TOOL=bin\Release\fly.exe

set NAME=fly-2.0.1-source.zip
call winrar.exe %CMD% %NAME% %FILES% %FILES-LIB%

set NAME=fly-tool-2.0.1-source.zip
call winrar.exe %CMD% %NAME% %FILES% %FILES-TOOL%

set NAME=fly-2.0.1-binary.zip
call winrar.exe -ep %CMD% %NAME% %LIB%

set NAME=fly-tool-2.0.1-binary.zip
call winrar.exe -ep %CMD% %NAME% %TOOL%

rem call psftp.exe -b pub-batch.bat abatishchev@web.sourceforge.net

set DIST=..\..\release\2.0.1

mkdir %DIST%
move /y fly-2.0.1-source.zip %DIST%
move /y fly-2.0.1-binary.zip %DIST%
move /y fly-tool-2.0.1-source.zip %DIST%
move /y fly-tool-2.0.1-binary.zip %DIST%
