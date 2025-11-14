SET IDENTITY_INSERT [dbo].[Plans] ON
INSERT INTO [dbo].[Plans] ([Id], [Name], [Description], [Weeks], [CreatedDate], [TotalPrice], [HealthIssues], [PaymentMethodId]) VALUES (7, N'PLAN777', NULL, 6, N'2025-10-10 00:00:00', CAST(50.00 AS Decimal(5, 2)), NULL, 3)
SET IDENTITY_INSERT [dbo].[Plans] OFF
