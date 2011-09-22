USE [tempdb]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TestSchema].[ResetTestSchemaTable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [TestSchema].[ResetTestSchemaTable]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TestSchema].[AddToTestSchemaTable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [TestSchema].[AddToTestSchemaTable]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TestSchema].[SelectAllFromTestTable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [TestSchema].[SelectAllFromTestTable]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TestSchema].[SelectAllFromTestSchemaTable]') AND type in (N'P', N'PC'))
DROP PROCEDURE [TestSchema].[SelectAllFromTestSchemaTable]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[TestSchema].[SchemaTesting]') AND type in (N'U'))
DROP TABLE [TestSchema].[SchemaTesting]
GO

IF  EXISTS (SELECT * FROM sys.schemas WHERE name = N'TestSchema')
DROP SCHEMA [TestSchema]
GO

IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'TestSchemaOwner')
DROP USER [TestSchemaOwner]
GO


USE [master]
GO

IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'TestSchemaOwner')
DROP LOGIN [TestSchemaOwner]
GO

CREATE LOGIN [TestSchemaOwner] WITH PASSWORD=N'password', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [tempdb]
GO



CREATE USER [TestSchemaOwner] FOR LOGIN [TestSchemaOwner] 
GO

CREATE SCHEMA [TestSchema] AUTHORIZATION [TestSchemaOwner]
GO

CREATE TABLE [TestSchema].[SchemaTesting]
(
	[test] [nchar](10) NULL

) ON [PRIMARY]

GO


CREATE PROCEDURE [TestSchema].[SelectAllFromTestTable]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Test 
	FROM [TestSchema].[TestTable] 
END
GO

CREATE PROCEDURE [TestSchema].[SelectAllFromTestSchemaTable]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Test 
	FROM [TestSchema].[TestTable] 
END
GO

CREATE PROCEDURE [TestSchema].[AddToTestSchemaTable]
	@TestValue varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [TestSchema].[SchemaTesting]
	  (test)
	  VALUES
	  (@TestValue)
END
GO

CREATE PROCEDURE [TestSchema].[ResetTestSchemaTable]
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM [TestSchema].[SchemaTesting]
END
GO