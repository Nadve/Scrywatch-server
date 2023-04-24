CREATE PROCEDURE [dbo].[spInterest_Insert]
	@userId NVARCHAR(450),
	@cardId INT,
	@goal INT,
	@finish NVARCHAR(10),
	@currency NCHAR(3),
	@intention NVARCHAR(4)
AS
BEGIN
	WITH F AS (
		SELECT Id
		FROM [Finish]
		WHERE Value = @finish
	), I AS (
		SELECT Id
		FROM [Intention]
		WHERE Value = @intention
	), C AS (
		SELECT Id
		FROM [Currency]
		WHERE Value = @currency
	)
	INSERT INTO [Interest]
	VALUES (
		@userId,
		@cardId,
		@goal,
		( SELECT * FROM F ),
		( SELECT * FROM C ),
		( SELECT * FROM I )
	)
END
