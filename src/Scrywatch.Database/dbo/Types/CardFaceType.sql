﻿CREATE TYPE [dbo].[CardFaceType] AS TABLE
(
	[CardGuid] NVARCHAR(450) NOT NULL,
	[Face] NVARCHAR(5) NOT NULL,
	[Url] NVARCHAR(450) NOT NULL
)