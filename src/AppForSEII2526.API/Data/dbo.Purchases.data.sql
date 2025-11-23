SET IDENTITY_INSERT [dbo].[Purchases] ON
INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Street], [TotalPrice], [Description], [PaymentMethodId]) VALUES (1, N'Madrid', N'Spain', N'2025-10-15 14:30:00', N'Calle Gran Via 45', CAST(89.97 AS Decimal(10, 2)), N'Initial equipment purchase for gym', 3)
INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Street], [TotalPrice], [Description], [PaymentMethodId]) VALUES (2, N'Barcelona', N'Spain', N'2025-10-18 10:15:00', N'Avinguda Diagonal 200', CAST(124.97 AS Decimal(10, 2)), N'Restock of popular items', 3)
INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Street], [TotalPrice], [Description], [PaymentMethodId]) VALUES (3, N'Valencia', N'Spain', N'2025-10-20 16:45:00', N'Calle Colon 30', CAST(67.98 AS Decimal(10, 2)), NULL, 3)
INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Street], [TotalPrice], [Description], [PaymentMethodId]) VALUES (4, N'Seville', N'Spain', N'2025-10-22 11:20:00', N'Avenida de la Constitución 5', CAST(159.96 AS Decimal(10, 2)), N'Bulk purchase for new class offerings', 3)
SET IDENTITY_INSERT [dbo].[Purchases] OFF

