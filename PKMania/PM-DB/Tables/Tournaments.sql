CREATE TABLE [dbo].[Tournaments]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[status] NVARCHAR(20) NOT NULL DEFAULT 'created',
	[started_on] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
	[finished_on] DATETIME2(7),
	[name] NVARCHAR(150) NOT NULL DEFAULT 'New tournament',
	[tournament_type_id] INT NOT NULL,
	CONSTRAINT PK_tournaments PRIMARY KEY(id),
	CONSTRAINT FK_tournaments_tournament_types FOREIGN KEY(tournament_type_id) REFERENCES Tournaments_types(id)
)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_status] CHECK(status IN ('created','waitingForPlayers','canceled','ongoing','paused','finished'))
GO