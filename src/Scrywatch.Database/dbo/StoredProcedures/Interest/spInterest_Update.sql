CREATE PROCEDURE [dbo].[spInterest_Update]
	@id INT,
	@goal INT
AS
BEGIN
	UPDATE [Interest]
	SET Goal=@goal
	WHERE Id=@id
END
