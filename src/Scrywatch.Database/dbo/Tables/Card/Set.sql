﻿CREATE TABLE [dbo].[Set]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Code] NVARCHAR(10) NOT NULL, 
    [Svg] NVARCHAR(450) NOT NULL
)
