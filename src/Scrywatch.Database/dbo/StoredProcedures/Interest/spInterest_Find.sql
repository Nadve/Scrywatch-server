CREATE PROCEDURE [dbo].[spInterest_Find]
	@userId NVARCHAR(256),
	@cardId INT,
	@goal INT,
	@finish NVARCHAR(10),
	@currency NCHAR(3),
	@intention NVARCHAR(4)
AS
BEGIN
	WITH I AS (
		SELECT [Interest].Id,
			   [Interest].CardId,
			   [Interest].Goal,
			   [Finish].Value AS Finish,
			   [Currency].Value AS Currency,
			   [Intention].Value As Intention
		FROM [Interest]
		INNER JOIN [Finish] ON [Finish].Id = [Interest].FinishId
		INNER JOIN [Currency] ON [Currency].Id = [Interest].CurrencyId
		INNER JOIN [Intention] ON [Intention].Id = [Interest].IntentionId
		WHERE [Interest].UserId = @userId
	)
	SELECT *
	FROM I
	WHERE CardId = @cardId
	AND Finish = @finish
	AND Currency = @currency
	AND Intention = @intention
END
