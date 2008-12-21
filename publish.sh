#!/bin/bash
# Copyright (C) 2007-2008 Alexander M. Batishchev aka Godfather (abatishchev at gmail.com)

files='CHLOE.Compiler.csproj CHLOE.Compiler.sln OnTheFlyCompiler.cs OnTheFlyCompilerException.cs Properties/AssemblyInfo.cs'
data='Data/*.xml Data/*.cs Data/*.vb'

release='bin/Release'
lib='CHLOE.Compiler.dll'

batch='batch'
rm $batch
echo 'cd uploads' >> $batch

name='fly-chloe-1.0-source.tar.bz'
tar cfjv $name $files
echo 'put '$name >> $batch

name='fly-chloe-1.0-source+data.tar.bz'
tar cfjv -r $name $files $data
echo 'put '$name >> $batch

name='fly-chloe-1.0-binary.tar.bz'
tar cfjv $name -C $release $lib
echo 'put '$name >> $batch

name='fly-chloe-1.0-binary+data.tar.bz'
tar cfjv $name $data -C $release $lib
echo 'put '$name >> $batch

sftp -b $batch abatishchev@frs.sourceforge.net

rm $batch

dist='../../release/chloe-1.0'

mkdir $dist
mv -f fly-chloe-1.0-source.tar.bz $dist
mv -f fly-chloe-1.0-source+data.tar.bz $dist
mv -f fly-chloe-1.0-binary.tar.bz $dist
mv -f fly-chloe-1.0-binary+data.tar.bz $dist
