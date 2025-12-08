SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [PaymentMethodType], [telephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (1, N'1', N'CreditCard', NULL, N'1234567890123456', N'2030-12-31 00:00:00', NULL)
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [PaymentMethodType], [telephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (2, N'1', N'Paypal', NULL, NULL, NULL, N'samuel@uclm.es')
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [PaymentMethodType], [telephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (3, N'1', N'Bizum', N'636187115', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF

