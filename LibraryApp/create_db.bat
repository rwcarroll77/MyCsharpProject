echo off

rem batch file to fun a script to create a db
rem 8/28/18

sqlcmd -S localhost -E -i LibraryDB.sql
rem sqlcmd -S localhost\sqlexpress -E -i boatDB.sqlcmd

ECHO .
ECHO if no error messsages appear DB was created 
PAUSE