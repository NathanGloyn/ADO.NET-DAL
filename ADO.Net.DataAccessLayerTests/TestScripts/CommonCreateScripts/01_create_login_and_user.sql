USE [master]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'MinimumPermission')
DROP LOGIN [MinimumPermission]
GO

CREATE LOGIN [MinimumPermission] WITH PASSWORD=N'password', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [tempdb]
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'MinimumPermission')
DROP USER [MinimumPermission]
GO

CREATE USER [MinimumPermission] FOR LOGIN [MinimumPermission] WITH DEFAULT_SCHEMA=[dbo]
GO