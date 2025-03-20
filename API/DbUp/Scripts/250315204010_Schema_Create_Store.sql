CREATE SCHEMA [Store]
GO

/*
	[Product]
*/

CREATE TABLE [Store].[Product](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Price] [bigint] NOT NULL,
	[PictureUrl] [nvarchar](512) NULL,
	[Brand] [nvarchar](256) NOT NULL,
	[Type] [nvarchar](256) NOT NULL,
	[QuantityInStock] [int] NOT NULL,
	[PublicId] [nvarchar](512) NULL,

	CONSTRAINT [PK_Product] 
	PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

/*
	[Basket]
*/

CREATE TABLE [Store].[Basket](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWSEQUENTIALID(),	
	[BuyerId] [nvarchar](256) NOT NULL,
	[PaymentIntentId] [nvarchar](256) NULL,
	[ClientSecret] [nvarchar](256) NULL,

	CONSTRAINT [PK_Basket] 
	PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

--ALTER TABLE [Store].[Basket] WITH CHECK ADD CONSTRAINT FK_Basket_User_Id 
--FOREIGN KEY (UserId)
--REFERENCES [User].[AspNetUsers] (Id)
--ON DELETE CASCADE
--GO

/*
	[BasketItem]
*/

CREATE TABLE [Store].[BasketItem](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWSEQUENTIALID(),
	[Quantity] [int] NOT NULL,
	[BasketId] [uniqueidentifier] NOT NULL,
	[ProductId] [bigint] NOT NULL,

	CONSTRAINT [PK_BasketItem]
	PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

--CREATE NONCLUSTERED INDEX [IX_BasketItem_BasketId] ON [Store].[BasketItem]
--(
--	[BasketId] ASC
--) ON [PRIMARY]
--GO

ALTER TABLE [Store].[BasketItem]  
	WITH CHECK ADD  CONSTRAINT [FK_BasketItem_Product_ProductId] 
	FOREIGN KEY([ProductId])
	REFERENCES [Store].[Product] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [Store].[BasketItem] 
	CHECK CONSTRAINT [FK_BasketItem_Product_ProductId]
GO

ALTER TABLE [Store].[BasketItem]  
	WITH CHECK ADD  CONSTRAINT [FK_BasketItem_Basket_BasketId] 
	FOREIGN KEY([BasketId])
	REFERENCES [Store].[Basket] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [Store].[BasketItem] 
	CHECK CONSTRAINT [FK_BasketItem_Basket_BasketId]
GO

/*
	[Order]
*/

CREATE TABLE [Store].[Order](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWSEQUENTIALID(),
	[OrderDate] [datetime2](7) NOT NULL,
	[Subtotal] [bigint] NOT NULL,
	[DeliveryFee] [bigint] NOT NULL,
	[OrderStatus] [int] NOT NULL,
	[ShippingAddressId] [bigint] NOT NULL,
	[BuyerId] [nvarchar](256) NOT NULL,
	[PaymentIntentId] [nvarchar](256) NULL,

	CONSTRAINT [PK_Order] 
	PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Store].[Order] 
	WITH CHECK ADD  CONSTRAINT [FK_Order_Address_ShippingAddressId] 
	FOREIGN KEY([ShippingAddressId])
	REFERENCES [User].[Address] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [Store].[Order] CHECK CONSTRAINT [FK_Order_Address_ShippingAddressId]
GO

--CREATE NONCLUSTERED INDEX [IX_Order_ShippingAddressId] ON [Store].[Order]
--(
--	[ShippingAddressId] ASC
--)

--ALTER TABLE [Store].[Order] WITH CHECK ADD CONSTRAINT FK_Order_User_Id 
--FOREIGN KEY (UserId)
--REFERENCES [User].[AspNetUsers] (Id)
--ON DELETE CASCADE
--GO

/*
	[OrderItem]
*/

CREATE TABLE [Store].[OrderItem](
	[Id] [uniqueidentifier] NOT NULL DEFAULT NEWSEQUENTIALID(),
	[Price] [bigint] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[PictureUrl] [nvarchar](512) NULL,
	[OrderId] [uniqueidentifier] NULL,

	CONSTRAINT [PK_OrderItem] 
	PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Store].[OrderItem]
	WITH CHECK ADD CONSTRAINT [FK_OrderItem_Product_ProductId] 
	FOREIGN KEY ([ProductId])
	REFERENCES [Store].[Product] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [Store].[OrderItem] 
	CHECK CONSTRAINT [FK_OrderItem_Product_ProductId]
GO


ALTER TABLE [Store].[OrderItem]  
	WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order_OrderId] 
	FOREIGN KEY([OrderId])
	REFERENCES [Store].[Order] ([Id])
GO

ALTER TABLE [Store].[OrderItem] 
	CHECK CONSTRAINT [FK_OrderItem_Order_OrderId]
GO

--CREATE NONCLUSTERED INDEX [IX_OrderItem_OrderId] ON [Store].[OrderItem]
--(
--	[OrderId] ASC
--)