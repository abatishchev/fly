#!/bin/bash
# Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

source='CHANGELOG.txt Compiler.cs Errors.cs Events.cs LICENSE.txt Output.cs Settings.cs Properties/AssemblyInfo.cs'
source_lib='OnTheFlyCompiler.csproj'
source_binary='OnTheFlyCompiler-Tool.csproj ConsoleStub.cs Core.cs Examples/*.cs Settings/*.xml'

release='bin/Release'
lib='fly.dll'
binary='fly.exe'

batch='batch'
rm $batch
echo 'cd uploads' >> $batch

name='fly-2.0.1-source.tar.bz'
tar cfjv $name $source $source_lib
echo 'put '$name >> $batch

name='fly-tool-2.0.1-source.tar.bz'
tar cfjv $name $source $source_binary
echo 'put '$name >> $batch

name='fly-2.0.1-binary.tar.bz'
tar cfjv $name -C $release $lib
echo 'put '$name >> $batch

name='fly-tool-2.0.1-binary.tar.bz'
tar cfjv $name -C $release $binary
echo 'put '$name >> $batch

sftp -b $batch abatishchev@frs.sourceforge.net

rm $batch

dist='..\..\release\2.0.1'

mkdir -p $dist
mv -f fly-2.0.1-source.tar $dist
mv -f fly-2.0.1-binary.tar $dist
mv -f fly-tool-2.0.1-source.tar $dist
mv -f fly-tool-2.0.1-binary.tar $dist
