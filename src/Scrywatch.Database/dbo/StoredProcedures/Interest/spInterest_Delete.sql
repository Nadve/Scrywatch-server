CREATE PROCEDURE [dbo].[spInterest_Delete]
	@id INT
AS
BEGIN
	DELETE FROM [Interest]
	WHERE Id = @id
END
