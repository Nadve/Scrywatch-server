CREATE PROCEDURE [dbo].[spCardPrice_Insert]
	@prices CardPriceType READONLY
AS
DECLARE @Output TABLE (CardId INT)

/* Input */
DECLARE @PriceTable AS TABLE
(
	[CardId] INT NOT NULL,
	[FinishId] INT NOT NULL,
	[CurrencyId] INT NOT NULL,
	[Date] DATE NOT NULL DEFAULT GETDATE(), 
	[Value] DECIMAL(8, 2) NOT NULL
)

SET NOCOUNT ON;
INSERT INTO @PriceTable
SELECT [Card].Id,
	[Finish].Id AS FinishId,
	[Currency].Id AS CurrencyId,
	P.Date,
	P.Value
FROM @prices AS P
INNER JOIN [Card] ON [Card].Guid = P.CardGuid
INNER JOIN [Finish] ON [Finish].Value = Finish
INNER JOIN [Currency] ON [Currency].Value = Currency
SET NOCOUNT OFF

IF (NOT EXISTS( SELECT 1 FROM [CardPrice] ))
	BEGIN
		INSERT INTO [CardPrice]
		OUTPUT inserted.CardId
		INTO @Output
		SELECT * FROM @PriceTable
	END
ELSE
	BEGIN
		SET NOCOUNT ON;
		/* Only latest card prices */
		DECLARE @PriceTable1 AS TABLE
		(
			[CardId] INT NOT NULL,
			[FinishId] INT NOT NULL,
			[CurrencyId] INT NOT NULL,
			[Date] DATE NOT NULL DEFAULT GETDATE(), 
			[Value] DECIMAL(8, 2) NOT NULL
		)
		INSERT INTO @PriceTable1
		SELECT CardId,
			FinishId,
			CurrencyId,
			Date,
			Value
		FROM (
			SELECT CardId,
				FinishId,
				CurrencyId,
				Date,
				Value,
				ROW_NUMBER() OVER (
					PARTITION BY CardId, FinishId, CurrencyId
					ORDER BY Date DESC
				) AS RN
			FROM [CardPrice]
		) T
		WHERE T.RN = 1

		/* Contains two latest card prices ranked*/
		DECLARE @PriceTable2 AS TABLE
		(
			[CardId] INT NOT NULL,
			[FinishId] INT NOT NULL,
			[CurrencyId] INT NOT NULL,
			[Date] DATE NOT NULL DEFAULT GETDATE(), 
			[Value] DECIMAL(8, 2) NOT NULL,
			[Rank] INT NOT NULL
		)
		INSERT INTO @PriceTable2
		SELECT CardId, FinishId, CurrencyId, Date, Value, Rank() OVER(
			PARTITION BY CardId, FinishId, CurrencyId
			ORDER BY Value
		) AS Rank
		FROM (
			SELECT *
			FROM @PriceTable1
			UNION ALL
			SELECT *
			FROM @PriceTable
		) AS Un

		/* Contains Ids of new card prices */
		DECLARE @PriceTable3 AS TABLE
		(
			[CardId] INT NOT NULL,
			[FinishId] INT NOT NULL,
			[CurrencyId] INT NOT NULL
		)
		INSERT INTO @PriceTable3
		SELECT CardId, FinishId, CurrencyId
		FROM @PriceTable2
		WHERE Rank = 2
		SET NOCOUNT OFF;

		INSERT INTO [CardPrice]
		OUTPUT inserted.CardId
		INTO @Output
		SELECT P.CardId, P.FinishId, P.CurrencyId, P.Date, P.Value
		FROM @PriceTable AS P
		INNER JOIN @PriceTable3 AS P3
		ON P.CardId = P3.CardId
		AND P.FinishId = P3.FinishId
		AND P.CurrencyId = P3.CurrencyId
	END
RETURN SELECT count(CardId) FROM @Output
GO
