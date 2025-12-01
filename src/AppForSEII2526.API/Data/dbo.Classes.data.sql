SET IDENTITY_INSERT [dbo].[Classes] ON
INSERT INTO [dbo].[Classes] ([Id], [Name], [Price], [Capacity], [Date]) VALUES (6, N'Yoga', CAST(11.00 AS Decimal(5, 2)), 20, N'2026-01-10 18:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Name], [Price], [Capacity], [Date]) VALUES (10, N'Spinning', CAST(11.00 AS Decimal(5, 2)), 20, N'2026-01-11 17:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Name], [Price], [Capacity], [Date]) VALUES (12, N'CrossFit', CAST(10.00 AS Decimal(5, 2)), 20, N'2026-01-12 16:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Name], [Price], [Capacity], [Date]) VALUES (13, N'Strech & Relax', CAST(10.00 AS Decimal(5, 2)), 25, N'2026-01-13 20:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Name], [Price], [Capacity], [Date]) VALUES (14, N'Zumba', CAST(8.00 AS Decimal(5, 2)), 20, N'2026-01-14 17:30:00')
INSERT INTO [dbo].[Classes] ([Id], [Name], [Price], [Capacity], [Date]) VALUES (15, N'Pilates', CAST(10.00 AS Decimal(5, 2)), 25, N'2026-01-15 15:30:00')
SET IDENTITY_INSERT [dbo].[Classes] OFF
