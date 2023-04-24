CREATE PROCEDURE [dbo].[spCard_Merge]
	@cards CardType READONLY
AS
BEGIN
	DECLARE @CardTable TABLE
	(
		[Guid] NVARCHAR(450) NOT NULL PRIMARY KEY,
		[NameId] INT NOT NULL, 
		[SetId] INT NOT NULL, 
		[RarityId] INT NOT NULL,
		[CollectorNumber] NVARCHAR(10) NOT NULL
	);
	INSERT INTO @CardTable
	SELECT C.Guid,
		[Name].Id AS NameId,
		[Set].Id AS SetId,
		[Rarity].Id AS RarityId,
		C.CollectorNumber
	FROM @cards AS C
	INNER JOIN [Name] ON [Name].Value = C.Name
	INNER JOIN [Set] ON [Set].Code = C.SetCode
	INNER JOIN [Rarity] ON [Rarity].Value = C.Rarity

	DECLARE @MergeTable Table (
		[Action] VARCHAR(10),
		[Guid] NVARCHAR(450)
	)
	MERGE [Card] AS T
	USING @CardTable AS S
	ON T.Guid = S.Guid
	WHEN MATCHED AND (
		T.NameId <> S.NameId OR
		T.SetId <> S.SetId OR
		T.RarityId <> S.RarityId OR
		T.CollectorNumber <> S.CollectorNumber
	) THEN UPDATE SET
		T.NameId = S.NameId,
		T.SetId = S.SetId,
		T.RarityId = S.RarityId,
		T.CollectorNumber = S.CollectorNumber
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (Guid, NameId, SetId, RarityId, CollectorNumber)
		VALUES (S.Guid, S.NameId, S.SetId, S.RarityId, S.CollectorNumber)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
	OUTPUT $action, deleted.Guid
	INTO @MergeTable;
		
	SELECT Action, COUNT(*) AS Count
	FROM @MergeTable
	GROUP BY Action;
END
GO
