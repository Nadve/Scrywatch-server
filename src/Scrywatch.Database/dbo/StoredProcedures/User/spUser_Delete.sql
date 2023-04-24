CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id nvarchar(450)
AS
BEGIN
	DELETE FROM [dbo].[User]
	WHERE Id = @Id
END
