USE [master]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'AdditionalDBUser')
DROP LOGIN [AdditionalDBUser]
GO

CREATE LOGIN [AdditionalDBUser] WITH PASSWORD=N'password', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [AdditionalDB]
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'AdditionalDBUser')
DROP USER [AdditionalDBUser] 
GO

CREATE USER [AdditionalDBUser] FOR LOGIN [AdditionalDBUser] WITH DEFAULT_SCHEMA=[dbo]
GO


IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'U' AND name = 'TestTable')
	DROP TABLE [dbo].[TestTable]
GO


CREATE TABLE [dbo].[TestTable](
	[TestKey] [varchar](100) NULL,
	[TestValue] [varchar](100) NULL
) ON [PRIMARY]

GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'AddToTestTable')
	DROP PROCEDURE [dbo].[AddToTestTable]
GO

CREATE PROCEDURE [dbo].[AddToTestTable]
	@TestKey varchar(100),
	@TestValue varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[TestTable]
	  (TestKey, TestValue)
	  VALUES
	  (@TestKey, @TestValue)
END
GO

GRANT EXECUTE On [dbo].[AddToTestTable] TO [AdditionalDBUser] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectFromTestTable')
	DROP PROCEDURE [dbo].[SelectFromTestTable]
GO

CREATE PROCEDURE [dbo].[SelectFromTestTable]
	@TestKey varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TestValue FROM [dbo].[TestTable] WHERE TestKey = @TestKey
END
GO

GRANT EXECUTE On [dbo].[SelectFromTestTable] TO [AdditionalDBUser] 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectAllFromTestTableAdditional')
	DROP PROCEDURE [dbo].[SelectAllFromTestTableAdditional]
GO

CREATE PROCEDURE [dbo].[SelectAllFromTestTableAdditional]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TestValue FROM [dbo].[TestTable] 
END
GO

GRANT EXECUTE On [dbo].[SelectAllFromTestTableAdditional] TO [AdditionalDBUser] 
GO