CREATE PROCEDURE [dbo].[spNotification_Delete]
@deleted INT OUTPUT
AS
BEGIN
DECLARE @Merge TABLE (
	[Action] VARCHAR(10),
	[Id] INT
);

WITH interestPrice AS (
	SELECT *
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
), avgPrice AS (
	SELECT CardId, FinishId, CurrencyId, AVG(Value) AS Average
	FROM [CardPrice]
	GROUP BY CardId, FinishId, CurrencyId
), trend AS (
	SELECT interestPrice.CardId,
		interestPrice.FinishId,
		interestPrice.CurrencyId,
		ROUND(
			(interestPrice.Value - avgPrice.Average)
			/ avgPrice.Average * 100, 0
		) AS Trend
	FROM interestPrice INNER JOIN avgPrice ON
		interestPrice.CardId = avgPrice.CardId AND
		interestPrice.FinishId = avgPrice.FinishId AND
		interestPrice.CurrencyId = avgPrice.CurrencyId
), cardInterests AS (
	SELECT [Interest].Id,
		   [Interest].UserId,
		   [Interest].CardId,
		   [Interest].FinishId,
		   [Interest].CurrencyId,
		   [Interest].IntentionId,
		   interestPrice.Date,
		   interestPrice.Value,
		   [Interest].Goal,
		   trend.Trend
	FROM interestPrice
	INNER JOIN [Interest] ON
		[Interest].CardId = interestPrice.CardId AND
		[Interest].FinishId = interestPrice.FinishId AND
		[Interest].CurrencyId = interestPrice.CurrencyId
	INNER JOIN trend ON
		[Interest].CardId = trend.CardId AND
		[Interest].FinishId = trend.FinishId AND
		[Interest].CurrencyId = trend.CurrencyId

), sellInterests AS (
	SELECT *
	FROM cardInterests
	WHERE IntentionId = 2
	AND Trend >= Goal
), buyInterests AS (
	SELECT *
	FROM cardInterests
	WHERE IntentionId = 1
	AND Trend <= Goal
), filteredInterests AS (
	SELECT * FROM sellInterests
	UNION
	SELECT * FROM buyInterests
)
MERGE [Notification] AS T
USING filteredInterests AS S
ON T.InterestId = S.Id
WHEN NOT MATCHED BY SOURCE THEN
	DELETE
OUTPUT $action, deleted.InterestId
INTO @Merge;

SELECT Action, COUNT(*) AS Count
FROM @Merge
GROUP BY Action

SET @deleted = (
	SELECT Count
	FROM (
		SELECT Action, COUNT(*) AS Count
		FROM @Merge
		GROUP BY Action
	) AS d
)
END
