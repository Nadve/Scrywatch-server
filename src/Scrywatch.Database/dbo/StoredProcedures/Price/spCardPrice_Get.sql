CREATE PROCEDURE [dbo].[spCardPrice_Get]
	@id int,
	@finish NVARCHAR(10),
	@currency NCHAR(3)
AS
BEGIN
	WITH CPrice AS (
		SELECT P.CardId,
			F.Value AS Finish,
			C.Value AS Currency,
			P.Date,
			P.Value
		FROM [CardPrice] AS P
		INNER JOIN [Finish] AS F ON FinishId = F.Id
		INNER JOIN [Currency] AS C ON CurrencyId = C.Id
	)
	SELECT Date, Value
	FROM CPrice
	WHERE CardId = @id
	AND Finish = @finish
	AND Currency = @currency
END
