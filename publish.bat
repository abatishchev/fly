SET TMPBAT=publish.temp.bat
SubWCRev "%CD%" publish.bat.in %TMPBAT%
call %TMPBAT%
del %TMPBAT%