#!/bin/bash
tmpsh=publish.sh.temp
SVNWCRev $cd publish.sh.in $tmpsh
$tmpsh
rm $tmpsh