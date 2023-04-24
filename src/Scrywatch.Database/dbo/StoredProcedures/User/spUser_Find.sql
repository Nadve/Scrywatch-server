CREATE PROCEDURE [dbo].[spUser_Find]
	@userName nvarchar(256)
AS
BEGIN
	SELECT *
	FROM [dbo].[User]
	WHERE UserName = @userName
END
