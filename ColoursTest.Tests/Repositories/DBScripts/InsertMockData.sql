USE [@DBName]

INSERT INTO [dbo].[Colours] ([Name],[IsEnabled]) VALUES ('Red',1)
INSERT INTO [dbo].[Colours] ([Name],[IsEnabled]) VALUES ('Green',1)
INSERT INTO [dbo].[Colours] ([Name],[IsEnabled]) VALUES ('Blue',1)

INSERT INTO [dbo].[People] ([FirstName],[LastName],[IsAuthorised],[IsValid],[IsEnabled]) VALUES ('Test','1',1,1,1)
INSERT INTO [dbo].[People] ([FirstName],[LastName],[IsAuthorised],[IsValid],[IsEnabled]) VALUES ('Test','2',1,1,1)
INSERT INTO [dbo].[People] ([FirstName],[LastName],[IsAuthorised],[IsValid],[IsEnabled]) VALUES ('Test','3',1,1,1)
INSERT INTO [dbo].[People] ([FirstName],[LastName],[IsAuthorised],[IsValid],[IsEnabled]) VALUES ('Test','4',1,1,1)
INSERT INTO [dbo].[People] ([FirstName],[LastName],[IsAuthorised],[IsValid],[IsEnabled]) VALUES ('Test','5',1,1,1)

INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (1,1)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (1,2)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (2,1)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (2,3)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (3,1)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (3,2)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (3,3)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (4,2)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (4,3)
INSERT INTO [dbo].[FavouriteColours] ([PersonId],[ColourId]) VALUES (5,3)