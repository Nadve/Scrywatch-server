CREATE PROCEDURE [dbo].[spUser_FindByEmail]
	@normalizedEmail nvarchar(256)
AS
BEGIN
	SELECT *
	FROM [dbo].[User]
	WHERE NormalizedEmail = @normalizedEmail
END
