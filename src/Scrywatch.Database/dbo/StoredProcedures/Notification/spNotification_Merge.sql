CREATE PROCEDURE [dbo].[spNotification_Merge]
AS
BEGIN
DECLARE @Merge TABLE (
	[Action] VARCHAR(10),
	[Count] INT
);

DECLARE @deleted INT
DECLARE @inserted INT
EXEC spNotification_Delete @deleted OUTPUT
EXEC spNotification_Insert @inserted OUTPUT

INSERT INTO @Merge VALUES ('DELETE', @deleted)
INSERT INTO @Merge VALUES ('INSERT', @inserted)

SELECT * FROM @Merge
END
