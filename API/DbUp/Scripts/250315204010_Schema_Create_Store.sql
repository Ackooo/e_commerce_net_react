CREATE SCHEMA [Store]
GO

/*
	[Product]
*/

CREATE TABLE [Store].[Product](
	[Id] [uniqueidentifier] NOT NULL,
	[CIId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Price] [bigint] NOT NULL,	
	[PictureUrl] [nvarchar](256) NULL,
	[Brand] [nvarchar](32) NOT NULL,
	[Type] [nvarchar](32) NOT NULL,
	[QuantityInStock] [int] NOT NULL,
	[PublicId] [nvarchar](256) NULL,

	PRIMARY KEY NONCLUSTERED ([Id] ASC) ON [PRIMARY],
    CONSTRAINT UQ_Product UNIQUE CLUSTERED ([CIId])	
) ON [PRIMARY]
GO

/*
	[Basket]
*/

CREATE TABLE [Store].[Basket](
	[Id] [uniqueidentifier] NOT NULL,
	[CIId] [bigint] IDENTITY(1,1) NOT NULL,
	[BuyerId] [nvarchar](256) NOT NULL,
	[PaymentIntentId] [nvarchar](256) NULL,
	[ClientSecret] [nvarchar](256) NULL	

	PRIMARY KEY NONCLUSTERED ([Id] ASC) ON [PRIMARY],
    CONSTRAINT UQ_Basket UNIQUE CLUSTERED ([CIId])
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
	[Id] [uniqueidentifier] NOT NULL,
	[CIId] [bigint] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[BasketId] [bigint] NOT NULL	

	PRIMARY KEY NONCLUSTERED ([Id] ASC) ON [PRIMARY],
    CONSTRAINT UQ_BasketItem UNIQUE CLUSTERED ([CIId])
) ON [PRIMARY]
GO

ALTER TABLE [Store].[BasketItem] WITH CHECK ADD CONSTRAINT FK_BasketItem_Product_Id 
FOREIGN KEY (ProductId)
REFERENCES [Store].[Product] (CIId)
ON DELETE CASCADE
GO

ALTER TABLE [Store].[BasketItem] WITH CHECK ADD CONSTRAINT FK_BasketItem_Basket_Id 
FOREIGN KEY (BasketId)
REFERENCES [Store].[Basket] (CIId)
ON DELETE CASCADE
GO

/*
	[Order]
*/

CREATE TABLE [Store].[Order](
	[Id] [uniqueidentifier] NOT NULL,
	[CIId] [bigint] IDENTITY(1,1) NOT NULL,
	[BuyerId] [nvarchar](256) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[Subtotal] [bigint] NOT NULL,
	[DeliveryFee] [bigint] NOT NULL,	
	[OrderStatus] [tinyint] NOT NULL,
	[PaymentIntentId] [nvarchar](256) NULL,	

	[ShippingAddress_FullName] [nvarchar](265) NOT NULL,
    [ShippingAddress_Address1] [nvarchar](64) NOT NULL,
    [ShippingAddress_Address2] [nvarchar](64) NOT NULL,
    [ShippingAddress_Zip] [nvarchar](32) NOT NULL,
    [ShippingAddress_City] [nvarchar](32) NOT NULL,
    [ShippingAddress_State] [nvarchar](32) NOT NULL,
    [ShippingAddress_Country] [nvarchar](64) NOT NULL,

	PRIMARY KEY NONCLUSTERED ([Id] ASC) ON [PRIMARY],
    CONSTRAINT UQ_Order UNIQUE CLUSTERED ([CIId])	
) ON [PRIMARY]
GO

--ALTER TABLE [Store].[Order] WITH CHECK ADD CONSTRAINT FK_Order_User_Id 
--FOREIGN KEY (UserId)
--REFERENCES [User].[AspNetUsers] (Id)
--ON DELETE CASCADE
--GO

/*
	[OrderItem]
*/

CREATE TABLE [Store].[OrderItem](
	[Id] [uniqueidentifier] NOT NULL,
	[CIId] [bigint] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [bigint] NOT NULL,
	[ProductId] [bigint] NOT NULL	

	PRIMARY KEY NONCLUSTERED ([Id] ASC) ON [PRIMARY],
    CONSTRAINT UQ_OrderItem UNIQUE CLUSTERED ([CIId])
) ON [PRIMARY]
GO

ALTER TABLE [Store].[OrderItem] WITH CHECK ADD CONSTRAINT FK_OrderItem_Product_Id 
FOREIGN KEY (ProductId)
REFERENCES [Store].[Product] (CIId)
ON DELETE CASCADE
GO