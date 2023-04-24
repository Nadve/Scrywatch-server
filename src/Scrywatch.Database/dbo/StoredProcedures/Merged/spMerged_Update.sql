CREATE PROCEDURE [dbo].[spMerged_Update]
	@date DATE
AS
BEGIN
	UPDATE [dbo].[Merged] SET Date = @date
END
