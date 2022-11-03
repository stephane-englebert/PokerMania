CREATE TABLE [dbo].[Hands]
(
	[id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	[tournament_id] INT NOT NULL,
	[table_nr] TINYINT NOT NULL,
	[started_on] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
	[finished_on] DATETIME2(7),
	[hand_history] NVARCHAR(4000),
	CONSTRAINT PK_hands PRIMARY KEY(id),
	CONSTRAINT FK_hands_tournaments FOREIGN KEY(tournament_id) REFERENCES Tournaments(id)
)
GO
ALTER TABLE [dbo].[Hands]
	ADD CONSTRAINT [CK_hands_table_nr] CHECK(table_nr > 0)
GO
