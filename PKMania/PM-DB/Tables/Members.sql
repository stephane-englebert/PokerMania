CREATE TABLE [dbo].[Members]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[role] NVARCHAR(10) NOT NULL DEFAULT 'player',
	[pseudo] NVARCHAR(30) NOT NULL UNIQUE,
	[email] NVARCHAR(150) NOT NULL UNIQUE,
	[password] VARBINARY(255) NOT NULL,
	[salt] VARBINARY(64) NOT NULL,
	[bankroll] INT NOT NULL DEFAULT 0,
	[playing] BIT NOT NULL DEFAULT 0,
	[current_tournament_id] INT DEFAULT 0,
	[disconnected] BIT NOT NULL DEFAULT 1,
	CONSTRAINT PK_members PRIMARY KEY(id)
)
GO
ALTER TABLE [dbo].[Members]
	ADD CONSTRAINT [CK_members_role] CHECK(role IN ('admin','player'))
GO
ALTER TABLE [dbo].[Members]
	ADD CONSTRAINT [CK_members_bankroll] CHECK(bankroll >=0)
GO
