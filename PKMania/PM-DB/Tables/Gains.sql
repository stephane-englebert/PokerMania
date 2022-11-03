CREATE TABLE [dbo].[Gains]
(
	[id] INT NOT NULL IDENTITY(1,1),
	[gains_sharing_nr] SMALLINT NOT NULL,
	[start_place] SMALLINT NOT NULL DEFAULT 0,
	[end_place] SMALLINT NOT NULL DEFAULT 1,
	[percentage] DECIMAL(10,7) NOT NULL DEFAULT 1,
	CONSTRAINT PK_gains PRIMARY KEY(id)
)
GO
ALTER TABLE [dbo].[Gains]
	ADD CONSTRAINT [CK_gains_sharing_nr] CHECK(gains_sharing_nr > 0)
GO
ALTER TABLE [dbo].[Gains]
	ADD CONSTRAINT [CK_gains_start_end] CHECK(start_place <= end_place)
GO
ALTER TABLE [dbo].[Gains]
	ADD CONSTRAINT [CK_gains_percentage] CHECK(percentage > 0)
GO