CREATE PROCEDURE [dbo].[spUser_Get]
	@id NVARCHAR(450)
AS
BEGIN
	SELECT *
	FROM [dbo].[User]
	WHERE Id = @id
END
