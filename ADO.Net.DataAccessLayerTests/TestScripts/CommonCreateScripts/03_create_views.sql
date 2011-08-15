USE [tempdb]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[TestView]'))
DROP VIEW [dbo].[TestView]
GO

CREATE VIEW [dbo].[TestView]
AS
SELECT     TOP (100) PERCENT TestKey, TestValue
FROM         dbo.TestTable
ORDER BY TestKey

GO

