CREATE TABLE [dbo].[Tournaments_types]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[buy_in] INT NOT NULL DEFAULT 0,
	[late_registration_level] INT NOT NULL DEFAULT 0,
	[starting_stack] INT NOT NULL DEFAULT 3000,
	[rebuy] BIT NOT NULL DEFAULT 0,
	[rebuy_level] TINYINT NOT NULL DEFAULT 0,
	[levels_duration] SMALLINT NOT NULL DEFAULT 240,
	[min_players] TINYINT NOT NULL DEFAULT 2,
	[max_players] SMALLINT NOT NULL DEFAULT 32,
	[players_per_table] TINYINT NOT NULL DEFAULT 9,
	[max_paid_places] SMALLINT NOT NULL DEFAULT 0,
	[gains_sharing_nr] SMALLINT NOT NULL DEFAULT 0,
	CONSTRAINT PK_tournaments_types PRIMARY KEY(id)
)
GO
ALTER TABLE [dbo].[Tournaments_types]
	ADD CONSTRAINT [CK_tournaments_types_starting_stack] CHECK(starting_stack > 0)
GO
ALTER TABLE [dbo].[Tournaments_types]
	ADD CONSTRAINT [CK_tournaments_types_levels_duration] CHECK(levels_duration >= 120)
GO
ALTER TABLE [dbo].[Tournaments_types]
	ADD CONSTRAINT [CK_tournaments_types_min_players] CHECK(min_players >= 2)
GO
ALTER TABLE [dbo].[Tournaments_types]
	ADD CONSTRAINT [CK_tournaments_types_min_max] CHECK(min_players < max_players)
GO
ALTER TABLE [dbo].[Tournaments_types]
	ADD CONSTRAINT [CK_tournaments_types_players_per_table] CHECK(players_per_table BETWEEN 2 AND 9)
GO
ALTER TABLE [dbo].[Tournaments_types]
	ADD CONSTRAINT [CK_tournaments_types_max_paid] CHECK(max_paid_places BETWEEN 1 AND max_players)
GO