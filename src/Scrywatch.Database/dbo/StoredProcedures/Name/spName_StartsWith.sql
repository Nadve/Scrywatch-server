CREATE PROCEDURE [dbo].[spName_StartsWith]
	@name NVARCHAR(150)
AS
BEGIN
	SELECT TOP 10 *
	FROM [Name]
	WHERE Value LIKE @name + '%'
END
