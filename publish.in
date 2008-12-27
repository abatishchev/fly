rem Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

set PATH=%PATH%;%PROGRAMFILES%\WinRar;%PROGRAMFILES%\PuTTY

set CMD=A -afzip -o+ -s -ibck -t

set FILES=Compiler.cs Errors.cs Events.cs Output.cs Settings.cs Properties\AssemblyInfo.cs
set FILES-LIB=libfly.csproj
set FILES-BINARY=fly.csproj ConsoleStub.cs Core.cs Examples\*.cs Settings\*.xml

set LIB=bin\Release\libfly.dll
set BINARY=bin\Release\fly.exe

set BATCH=batch.txt
del %BATCH%
echo cd uploads >> %BATCH%

set NAME=libfly-2.0.1-source.zip
call winrar.exe %CMD% %NAME% %FILES% %FILES-LIB%
echo put %NAME% >> %BATCH%

set NAME=libfly-2.0.1-binary.zip
call winrar.exe -ep %CMD% %NAME% %LIB%
echo put %NAME% >> %BATCH%

set NAME=fly-2.0.1-source.zip
call winrar.exe %CMD% %NAME% %FILES% %FILES-BINARY%
echo put %NAME% >> %BATCH%

set NAME=fly-2.0.1-binary.zip
call winrar.exe -ep %CMD% %NAME% %BINARY%
echo put %NAME% >> %BATCH%

call psftp.exe -b %BATCH% abatishchev@web.sourceforge.net

del %BATCH%

set DIST=..\..\release

mkdir %DIST%
move /y libfly-2.0.1-source.zip %DIST%
move /y libfly-2.0.1-binary.zip %DIST%
move /y fly-2.0.1-source.zip %DIST%
move /y fly-2.0.1-binary.zip %DIST%
