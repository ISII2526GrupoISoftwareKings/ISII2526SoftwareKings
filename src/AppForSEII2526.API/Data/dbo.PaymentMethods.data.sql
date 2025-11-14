SET IDENTITY_INSERT [dbo].[PaymentMethods] ON
INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [PaymentMethodType], [telephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (3, N'1', N'CreditCard', N'636187115', N'1234567890123456', N'2050-11-11 00:00:00', N'samuel@uclm.es')
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF
