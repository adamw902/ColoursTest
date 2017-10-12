USE [master]

DECLARE @DBName VARCHAR(50) = @DatabaseName

IF EXISTS(SELECT name FROM dbo.sysdatabases WHERE name = @DBName)
BEGIN
	EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = @DBName
	ALTER DATABASE [@DBName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE [@DBName]
END


IF NOT EXISTS(SELECT name FROM dbo.sysdatabases WHERE name = @DBName)
BEGIN
	CREATE DATABASE [@DBName]
	ALTER DATABASE [@DBName] SET MULTI_USER
END