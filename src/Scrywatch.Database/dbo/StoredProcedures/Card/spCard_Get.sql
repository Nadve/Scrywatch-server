CREATE PROCEDURE [dbo].[spCard_Get]
	@name NVARCHAR(150)
AS
BEGIN
	WITH C AS (
		SELECT *
		FROM [Card]
		WHERE NameId = (
			SELECT Id
			FROM [Name]
			WHERE Value = @name
		)
	)
	SELECT C.Id AS 'Id',
		[Name].Value AS 'Name',
		[Rarity].Value AS 'Rarity',
		[Set].Name AS 'Set',
		[Set].Code AS 'SetCode',
		[Set].Svg AS 'SetSvg',
		C.CollectorNumber AS 'CollectorNumber',
		CFF.Url AS 'FrontFaceUrl',
		CFB.Url AS 'BackFaceUrl'
	FROM C
	LEFT JOIN [CardFace] AS CFF ON CFF.CardId = C.Id
		AND CFF.FaceId = 1
	LEFT JOIN [CardFace] AS CFB ON CFB.CardId = C.Id
		AND CFF.FaceId = 2
	INNER JOIN [Name] ON [Name].Id = C.NameId
	INNER JOIN [Set] ON [Set].Id = C.SetId
	INNER JOIN [Rarity] ON [Rarity].Id = C.RarityId
END
