CREATE TABLE [dbo].[CardPrice]
(
	[CardId] INT NOT NULL,
	[FinishId] INT NOT NULL,
	[CurrencyId] INT NOT NULL,
	[Date] DATE NOT NULL DEFAULT GETDATE(), 
	[Value] DECIMAL(8, 2) NOT NULL,
	PRIMARY KEY ([CardId], [FinishId], [CurrencyId], [Date]),
    CONSTRAINT FK_CardPrice_CardId FOREIGN KEY (CardId) REFERENCES Card(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CardPrice_FinishId FOREIGN KEY (FinishId) REFERENCES Finish(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CardPrice_CurrencyId FOREIGN KEY (CurrencyId) REFERENCES Currency(Id) ON DELETE CASCADE
)
