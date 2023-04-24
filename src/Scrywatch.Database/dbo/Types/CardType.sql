CREATE TYPE [dbo].[CardType] AS TABLE
(
    [Guid] NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [SetCode] NVARCHAR(10) NOT NULL, 
    [Rarity] NVARCHAR(20) NOT NULL,
    [CollectorNumber] NVARCHAR(10) NOT NULL
)
