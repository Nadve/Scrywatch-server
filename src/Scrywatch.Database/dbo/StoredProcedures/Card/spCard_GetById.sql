CREATE PROCEDURE [dbo].[spCard_GetById]
	@id INT
AS
BEGIN
	SELECT [Card].Id AS 'Id',
		[Name].Value AS 'Name',
		[Rarity].Value AS 'Rarity',
		[Set].Name AS 'Set',
		[Set].Code AS 'SetCode',
		[Set].Svg AS 'SetSvg',
		[Card].CollectorNumber AS 'CollectorNumber',
		CFF.Url AS 'FrontFaceUrl',
		CFB.Url AS 'BackFaceUrl'
	FROM [Card]
	LEFT JOIN [CardFace] AS CFF ON CFF.CardId = [Card].Id
		AND CFF.FaceId = 1
	LEFT JOIN [CardFace] AS CFB ON CFB.CardId = [Card].Id
		AND CFF.FaceId = 2
	INNER JOIN [Name] ON [Name].Id = [Card].NameId
	INNER JOIN [Set] ON [Set].Id = [Card].SetId
	INNER JOIN [Rarity] ON [Rarity].Id = [Card].RarityId
	WHERE [Card].Id = @id
END
