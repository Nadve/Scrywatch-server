CREATE PROCEDURE [dbo].[spCardFace_Merge]
	@faces CardFaceType READONLY
AS
BEGIN
	DECLARE @FaceTable TABLE (
		[CardId] INT NOT NULL,
		[FaceId] INT NOT NULL,
		[Url] NVARCHAR(450) NOT NULL
	);
	WITH Faces AS (
		SELECT CardGuid, F.Id AS FaceId, Url
		FROM [Face] AS F
		INNER JOIN @faces ON Value = Face
	)
	INSERT INTO @FaceTable
	SELECT C.Id AS CardId, Faces.FaceId, Faces.Url
	FROM [Card] AS C
	INNER JOIN Faces ON Faces.CardGuid = C.Guid

	DECLARE @MergeTable Table (
		[Action] VARCHAR(10),
		[CardId] INT
	)
	MERGE [CardFace] AS T
	USING @FaceTable AS S
	ON T.CardId = S.CardId AND T.FaceId = S.FaceId
	WHEN MATCHED AND T.Url <> S.Url THEN
		UPDATE SET
		T.Url = S.Url
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (CardId, FaceId, Url)
		VALUES (S.CardId, S.FaceId, S.Url)
	WHEN NOT MATCHED BY SOURCE THEN
		DELETE
	OUTPUT $action, deleted.CardId
	INTO @MergeTable;

	SELECT Action, COUNT(*) AS Count
	FROM @MergeTable
	GROUP BY Action;
END
GO
