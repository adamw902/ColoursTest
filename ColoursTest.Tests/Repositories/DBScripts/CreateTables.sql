USE [@DBName]

/* DROP EXISTING TABLES */
IF EXISTS(SELECT name FROM sys.tables WHERE name = N'FavouriteColours')
BEGIN
	DROP TABLE [dbo].[FavouriteColours]
END

IF EXISTS(SELECT name FROM sys.tables WHERE name = N'Colours')
BEGIN
	DROP TABLE [dbo].[Colours]
END

IF EXISTS(SELECT name FROM sys.tables WHERE name = N'People')
BEGIN
	DROP TABLE [dbo].[People]
END

/* CREATE TABLES */
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

/* CREATE COLOURS TABLE */
CREATE TABLE [dbo].[Colours](
	[ColourId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_Colours] PRIMARY KEY CLUSTERED 
(
	[ColourId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

/* CREATE PEOPLE TABLE */
CREATE TABLE [dbo].[People](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[IsAuthorised] [bit] NOT NULL,
	[IsValid] [bit] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

/* CREATE FAVOURITECOLOURS TABLE */
CREATE TABLE [dbo].[FavouriteColours](
	[PersonId] [int] NOT NULL,
	[ColourId] [int] NOT NULL,
 CONSTRAINT [PK_FavouriteColours] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC,
	[ColourId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[FavouriteColours]  WITH CHECK ADD  CONSTRAINT [FK_FavouriteColours_Colours] FOREIGN KEY([ColourId])
REFERENCES [dbo].[Colours] ([ColourId])

ALTER TABLE [dbo].[FavouriteColours] CHECK CONSTRAINT [FK_FavouriteColours_Colours]

ALTER TABLE [dbo].[FavouriteColours]  WITH CHECK ADD  CONSTRAINT [FK_FavouriteColours_People] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])

ALTER TABLE [dbo].[FavouriteColours] CHECK CONSTRAINT [FK_FavouriteColours_People]