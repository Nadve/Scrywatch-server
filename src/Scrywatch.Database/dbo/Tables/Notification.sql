CREATE TABLE [dbo].[Notification]
(
	[InterestId] INT NOT NULL,
	[Created] DATE NOT NULL DEFAULT GETDATE(),
	PRIMARY KEY([InterestId], [Created]),
	CONSTRAINT FK_Notification_InterestId FOREIGN KEY (InterestId) REFERENCES Interest(Id) ON DELETE CASCADE
)
