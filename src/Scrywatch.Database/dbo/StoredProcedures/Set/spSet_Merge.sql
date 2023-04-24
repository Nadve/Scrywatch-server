CREATE PROCEDURE [dbo].[spSet_Merge]
	@sets SetType READONLY
AS
BEGIN
	DECLARE @MergeTable Table (
		[Action] VARCHAR(10),
		[Code] NVARCHAR(10)
	)
	MERGE [Set] AS T
	USING @sets AS S
	ON T.Code = S.Code
	WHEN MATCHED AND (
		T.Name <> S.Name OR
		T.Svg <> S.Svg
	) THEN UPDATE SET
		T.Name = S.Name,
		T.Svg = S.Svg
	WHEN NOT MATCHED BY TARGET THEN
		INSERT (Name, Code, Svg)
		VALUES (S.Name, S.Code, S.Svg)
	OUTPUT $action, deleted.Code
	INTO @MergeTable;

	SELECT Action, COUNT(*) AS Count
	FROM @MergeTable
	GROUP BY Action;
END
GO
