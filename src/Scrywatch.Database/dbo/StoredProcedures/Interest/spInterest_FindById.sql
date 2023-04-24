CREATE PROCEDURE [dbo].[spInterest_FindById]
	@id INT
AS
BEGIN
	SELECT [Interest].UserId,
		   [Interest].CardId,
		   [Interest].Goal,
		   [Finish].Value AS Finish,
		   [Currency].Value As Currency,
		   [Intention].Value As Intention
	FROM [Interest]
	INNER JOIN [Finish] ON [Finish].Id = [Interest].FinishId
	INNER JOIN [Currency] ON [Currency].Id = [Interest].CurrencyId
	INNER JOIN [Intention] ON [Intention].Id = [Interest].IntentionId
	WHERE [Interest].Id = @id
END
