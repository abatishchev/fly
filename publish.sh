#!/bin/bash
# Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

source='CHANGELOG.txt Compiler.cs Errors.cs Events.cs LICENSE.txt Output.cs Settings.cs Properties/AssemblyInfo.cs'
source_lib='libfly.csproj'
source_binary='fly.csproj ConsoleStub.cs Core.cs Examples/*.cs Settings/*.xml'

release='bin/Release'
lib='libfly.dll'
binary='fly.exe'

batch='batch'
rm -f $batch
echo 'cd uploads' >> $batch

name='fly-2.0.1-source.tar.bz2'
tar cfjv $name $source $source_lib
echo 'put '$name >> $batch

name='fly-tool-2.0.1-source.tar.bz2'
tar cfjv $name $source $source_binary
echo 'put '$name >> $batch

name='fly-2.0.1-binary.tar.bz2'
tar cfjv $name -C $release $lib
echo 'put '$name >> $batch

name='fly-tool-2.0.1-binary.tar.bz2'
tar cfjv $name -C $release $binary
echo 'put '$name >> $batch

sftp -b $batch abatishchev@frs.sourceforge.net

rm $batch

dist='..\..\release\'

mkdir -p $dist
mv -f fly-2.0.1-source.tar.bz2 $dist
mv -f fly-2.0.1-binary.tar.bz2 $dist
mv -f fly-tool-2.0.1-source.tar.bz2 $dist
mv -f fly-tool-2.0.1-binary.tar.bz2 $dist
