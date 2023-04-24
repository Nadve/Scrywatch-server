CREATE PROCEDURE [dbo].[spCardFinish_Merge]
	@finishes CardFinishType READONLY
AS
BEGIN
	DECLARE @FinishTable TABLE (
		[CardId] INT,
		[FinishId] INT
	);
	WITH Finishes AS ( SELECT * FROM @finishes )
	INSERT INTO @FinishTable
	SELECT C.Id AS CardId, F.Id AS FinishId
	FROM Finishes
	INNER JOIN [Card] AS C ON C.Guid = Finishes.CardGuid
	INNER JOIN [Finish] AS F ON F.Value = Finishes.Finish
	DECLARE @MergeTable Table (
		[Action] VARCHAR(10),
		[CardId] INT
	)
	MERGE [CardFinish] AS T
	USING @FinishTable AS S
	ON T.CardId = S.CardId AND T.FinishId = S.FinishId
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (CardId, FinishId)
		VALUES (S.CardId, S.FinishId)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
	OUTPUT $action, deleted.CardId
	INTO @MergeTable;

	SELECT Action, COUNT(*) AS Count
	FROM @MergeTable
	GROUP BY Action;
END
GO
