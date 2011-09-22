USE [tempdb]
GO
EXEC [dbo].[ResetTestTable]
GO
EXEC [dbo].[AddToTestTable] 'key1', 'value1'
GO
EXEC [dbo].[AddToTestTable] 'key2', 'value2'
GO
EXEC [dbo].[AddToTestTable] 'key3', 'value3'
GO
EXEC [dbo].[AddToTestTable] 'key4', 'value4'
GO
EXEC [dbo].[AddToTestTable] 'key5', 'value5'
GO
EXEC [TestSchema].[AddToTestSchemaTable] 'value1'
GO