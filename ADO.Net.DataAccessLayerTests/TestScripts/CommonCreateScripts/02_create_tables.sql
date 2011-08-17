USE [tempdb]
GO


IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'U' AND name = 'TestTable')
	DROP TABLE [dbo].[TestTable]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TestTable](
	[TestKey] [varchar](100) NULL,
	[TestValue] [varchar](100) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

