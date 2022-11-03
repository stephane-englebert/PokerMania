CREATE TABLE [dbo].[Registrations]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[tournament_id] INT NOT NULL,
	[player_id] INT NOT NULL,
	[table_nr] TINYINT NOT NULL DEFAULT 0,
	[stack] INT NOT NULL DEFAULT 0,
	[bonus_time] SMALLINT NOT NULL DEFAULT 0,
	[eliminated_at] DATETIME2(7),
	CONSTRAINT PK_registrations PRIMARY KEY(id),
	CONSTRAINT FK_registrations_members FOREIGN KEY(player_id) REFERENCES Members(id),
	CONSTRAINT FK_registrations_tournaments FOREIGN KEY(tournament_id) REFERENCES Tournaments(id)
)
GO
ALTER TABLE [dbo].[Registrations]
	ADD CONSTRAINT [CK_registrations_table_nr] CHECK(table_nr >= 0)
GO
ALTER TABLE [dbo].[Registrations]
	ADD CONSTRAINT [CK_registrations_stack] CHECK(stack >= 0)
GO
ALTER TABLE [dbo].[Registrations]
	ADD CONSTRAINT [CK_registrations_bonus_time] CHECK(bonus_time >= 0)
GO