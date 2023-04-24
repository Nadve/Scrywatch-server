CREATE PROCEDURE [dbo].[spCardFinish_Get]
	@id int
AS
BEGIN
	SELECT Value
	FROM CardFinish
	INNER JOIN Finish AS F ON F.Id = FinishId
	AND CardId = @id
END
