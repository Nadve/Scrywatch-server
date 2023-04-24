CREATE PROCEDURE [dbo].[spCardPrice_GetAll]
	@id int
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
	SELECT Finish, Currency, Date, Value
	FROM CPrice
	WHERE CardId = @id
END
