CREATE PROCEDURE [dbo].[spName_Merge]
	@names NameType READONLY
AS
BEGIN
	DECLARE @MergeTable Table (
		[Action] VARCHAR(10),
		[Name] NVARCHAR(150)
	)
	MERGE [Name] AS T
	USING @names AS S
	ON T.Value = S.Value
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (Value) VALUES (S.Value)
	OUTPUT $action, deleted.Value
	INTO @MergeTable;

	SELECT Action, COUNT(*) AS Count
	FROM @MergeTable
	GROUP BY Action;
END
GO
