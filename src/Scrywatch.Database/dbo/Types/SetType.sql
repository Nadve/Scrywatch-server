﻿CREATE TYPE [dbo].[SetType] AS TABLE
(
	[Name] NVARCHAR(100) NOT NULL,
	[Code] NVARCHAR(10) NOT NULL,
	[Svg] NVARCHAR(450) NOT NULL
)
