CREATE PROCEDURE [dbo].[spInterest_Get]
	@userId NVARCHAR(450)
AS
BEGIN
	SELECT [Interest].Id,
		   [Interest].CardId,
		   [Interest].Goal,
		   [Finish].Value AS Finish,
		   [Currency].Value AS Currency,
		   [Intention].Value AS Intention
	FROM [Interest]
	INNER JOIN [Finish] ON [Finish].Id = [Interest].FinishId
	INNER JOIN [Currency] ON [Currency].Id = [Interest].CurrencyId
	INNER JOIN [Intention] ON [Intention].Id = [Interest].IntentionId
	WHERE [Interest].UserId = @userId
END
