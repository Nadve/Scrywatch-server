CREATE TABLE [dbo].[CardFinish]
(
    [CardId] INT NOT NULL,
	[FinishId] INT NOT NULL,
	PRIMARY KEY ([CardId], [FinishId]),
	CONSTRAINT FK_CardFinish_CardId FOREIGN KEY (CardId) REFERENCES Card(Id) ON DELETE CASCADE,
	CONSTRAINT FK_CardFinish_FinishId FOREIGN KEY (FinishId) REFERENCES Finish(Id)
)
