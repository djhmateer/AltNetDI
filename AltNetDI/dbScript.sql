--DB Dump done on Sunday 31st August 2014
USE [Funny]
GO
-- CREATE statements here
SET IDENTITY_INSERT [dbo].[Stories] ON 
INSERT [dbo].[Stories] ([ID], [Title], [Content], [Rating], [ImageURL], [VideoURL], [CreatedAt], [StoryType]) VALUES (17256, N'Banana', N'Q: Why did the banana go to the doctors? A: He wasn''t peeling very well', 10, NULL, NULL, CAST(0x0000A340006E9F9A AS DateTime), 0)
INSERT [dbo].[Stories] ([ID], [Title], [Content], [Rating], [ImageURL], [VideoURL], [CreatedAt], [StoryType]) VALUES (17257, N'Pizza', NULL, 2, NULL, N'//www.youtube.com/embed/y0TxfwB3BWQ?rel=0', CAST(0x0000A340006E594B AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Stories] OFF
GO