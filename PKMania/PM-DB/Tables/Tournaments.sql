CREATE TABLE [dbo].[Tournaments]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[status] NVARCHAR(20) NOT NULL DEFAULT 'created',
	[started_on] DATETIME2(7) NOT NULL DEFAULT GETDATE(),
	[finished_on] DATETIME2(7),
	[name] NVARCHAR(150) NOT NULL DEFAULT 'New tournament',
	[tournament_type_id] INT NOT NULL,
	[players_nb] INT NOT NULL DEFAULT 0,
	[prize_pool] INT NOT NULL DEFAULT 0,
	[gains_sharing_nr] SMALLINT NOT NULL DEFAULT 0,
	CONSTRAINT PK_tournaments PRIMARY KEY(id),
	CONSTRAINT FK_tournaments_tournament_types FOREIGN KEY(tournament_type_id) REFERENCES Tournaments_types(id)
)
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_status] CHECK(status IN ('created','waitingForPlayers','canceled','ongoing','paused','finished'))
GO
ALTER TABLE [dbo].[Tournaments]
	ADD CONSTRAINT [CK_tournaments_prize] CHECK(prize_pool >=0)
GO