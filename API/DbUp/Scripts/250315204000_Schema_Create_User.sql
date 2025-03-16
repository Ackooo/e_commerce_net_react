CREATE SCHEMA [User]
GO

/*
	[AspNetRoles]
*/
CREATE TABLE [User].[AspNetRoles](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](64) NOT NULL

	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY] 
GO

/*
	[AspNetUsers]
*/
CREATE TABLE [User].[AspNetUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](64) NOT NULL,
	[NormalizedUserName] [nvarchar](64) UNIQUE,
	[Email] [nvarchar](256) NOT NULL,
    [NormalizedEmail] [nvarchar](256) UNIQUE,
	[EmailConfirmed] [int] NOT NULL,
	[PasswordHash] [nvarchar](512) NOT NULL,
	[SecurityStamp] [nvarchar](512) NOT NULL,

	[ConcurrencyStamp] [nvarchar](128) NOT NULL,
    [PhoneNumber] [nvarchar](128) NULL,
    [PhoneNumberConfirmed] [int] NULL,
    [TwoFactorEnabled] [int] NOT NULL,
    [LockoutEnd] [nvarchar](128) NULL,
    [LockoutEnabled] [int] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,

	PRIMARY KEY CLUSTERED ([Id] ASC) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_NormalizedUserName] ON [User].[AspNetUsers]
(
	[NormalizedUserName] ASC
) ON [PRIMARY]
GO

/*
	[AspNetUserClaims]
*/

CREATE TABLE [User].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](256) NULL,
	[ClaimValue] [nvarchar](256) NULL,
	[User_Id] [uniqueidentifier] NOT NULL

	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

/*
	[AspNetUserLogins]
*/

CREATE TABLE [User].[AspNetUserLogins](
	[UserId] [uniqueidentifier] NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](128) NULL

	PRIMARY KEY CLUSTERED 
	(
		[LoginProvider] ASC,
		[ProviderKey] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

/*
	[AspNetUserRoles]
*/

CREATE TABLE [User].[AspNetUserRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL

	PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[RoleId] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
FOREIGN KEY([Role_Id])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

/*
	[AspNetUserTokens]
*/

CREATE TABLE [User].[AspNetUserTokens] (
    [UserId] [uniqueidentifier] NOT NULL,
    [LoginProvider] [nvarchar](256) NOT NULL,
    [Name] [nvarchar](256) NOT NULL,
    [Value] [nvarchar](256) NULL

	PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[LoginProvider] ASC,
		[Name] ASC
	) ON [PRIMARY]    
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

/*
	[Address]
*/

CREATE TABLE [User].[Address](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](265) NOT NULL,
	[Address1] [nvarchar](64) NOT NULL,
	[Address2] [nvarchar](64) NOT NULL,
	[Zip] [nvarchar](32) NOT NULL,
	[City] [nvarchar](32) NOT NULL,
	[State] [nvarchar](32) NULL,
	[Country] [nvarchar](64) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL

	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Address]  WITH CHECK ADD CONSTRAINT [FK_Address_AspNetUsers_UserId]
FOREIGN KEY([User_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO