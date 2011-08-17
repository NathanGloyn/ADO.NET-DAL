USE [tempdb]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

GRANT EXECUTE On [dbo].[AddToTestTable] TO [MinimumPermission]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'ResetTestTable')
	DROP PROCEDURE [dbo].[ResetTestTable]
GO

CREATE PROCEDURE [dbo].[ResetTestTable]
AS
BEGIN
	SET NOCOUNT ON;

    DELETE FROM [dbo].[TestTable]
END
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

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectOneFromTestTable')
	DROP PROCEDURE [dbo].[SelectOneFromTestTable]
GO

CREATE PROCEDURE [dbo].[SelectOneFromTestTable]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TestValue FROM [dbo].[TestTable] WHERE TestKey = 'key1'
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectAllFromTestTable')
	DROP PROCEDURE [dbo].[SelectAllFromTestTable]
GO

CREATE PROCEDURE [dbo].[SelectAllFromTestTable]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TestKey, TestValue FROM [dbo].[TestTable] ORDER BY TestKey
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectAllFromTestTable')
	DROP PROCEDURE [dbo].[Sproc with spaces in name]
GO

CREATE PROCEDURE [dbo].[Sproc with spaces in name]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TestKey, TestValue FROM [dbo].[TestTable] ORDER BY TestKey
END
GO

GRANT EXECUTE On [dbo].[Sproc with spaces in name] TO [MinimumPermission]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectAllButFromTestTable')
	DROP PROCEDURE [dbo].[SelectAllButFromTestTable]
GO

CREATE PROCEDURE [dbo].[SelectAllButFromTestTable]
	@ExcludeKey varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TestKey, TestValue FROM [dbo].[TestTable] WHERE TestKey != @ExcludeKey ORDER BY TestKey
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'DeleteFromTestTable')
	DROP PROCEDURE [dbo].[DeleteFromTestTable]
GO

CREATE PROCEDURE [dbo].[DeleteFromTestTable]
	@TestKey varchar(100)
AS
BEGIN

    DELETE FROM [dbo].[TestTable] WHERE TestKey = @TestKey
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'DeleteOneFromTestTable')
	DROP PROCEDURE [dbo].[DeleteOneFromTestTable]
GO

CREATE PROCEDURE [dbo].[DeleteOneFromTestTable]
AS
BEGIN

    DELETE FROM [dbo].[TestTable] WHERE TestKey = 'key1'
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectAllFromTestTableAsXml')
	DROP PROCEDURE [dbo].[SelectAllFromTestTableAsXml]
GO

CREATE PROCEDURE [dbo].[SelectAllFromTestTableAsXml]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	TestKey		'Key', 
			TestValue	'Value' 
	FROM	[dbo].[TestTable] ORDER BY TestKey
	FOR XML PATH ('Entry'), ROOT ('Entries')
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectAllButFromTestTableAsXml')
	DROP PROCEDURE [dbo].[SelectAllButFromTestTableAsXml]
GO

CREATE PROCEDURE [dbo].[SelectAllButFromTestTableAsXml]
	@ExcludeKey varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	TestKey		'Key', 
			TestValue	'Value' 
	FROM	[dbo].[TestTable]
	WHERE TestKey != @ExcludeKey 
	ORDER BY TestKey
	FOR XML PATH ('Entry'), ROOT ('Entries') 
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectAllFromTestTableAsXmlUsingAttributes')
	DROP PROCEDURE [dbo].[SelectAllFromTestTableAsXmlUsingAttributes]
GO

CREATE PROCEDURE [dbo].[SelectAllFromTestTableAsXmlUsingAttributes]
AS
BEGIN
	SET NOCOUNT ON;

    SELECT	TestKey		'@Key', 
			TestValue	'@Value' 
	FROM	[dbo].[TestTable]
	ORDER BY TestKey
	FOR XML PATH ('Entry'), ROOT ('Entries') 
END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'P' AND name = 'SelectTestTableAsXmlWithAttributesUsingNamespaces')
	DROP PROCEDURE [dbo].[SelectTestTableAsXmlWithAttributesUsingNamespaces]
GO

CREATE PROCEDURE [dbo].[SelectTestTableAsXmlWithAttributesUsingNamespaces]
AS
BEGIN
	SET NOCOUNT ON;

	WITH XMLNAMESPACES ('uri' as ns1)
    SELECT	TestKey		'@ns1:Key', 
			TestValue	'@ns1:Value' 
	FROM	[dbo].[TestTable]
	ORDER BY TestKey
	FOR XML PATH ('ns1:Entry'), ROOT ('ns1:Entries') 
END
GO