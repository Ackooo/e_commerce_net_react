CREATE SCHEMA [User]
GO

/*
	[AspNetRoles]
*/
CREATE TABLE [User].[AspNetRoles](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,

	CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] 
	ON [User].[AspNetRoles]
	(
		[NormalizedName] ASC
	)
	WHERE ([NormalizedName] IS NOT NULL)
	ON [PRIMARY]
GO

/*
	[AspNetUsers]
*/

CREATE TABLE [User].[AspNetUsers](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,

	CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [EmailIndex] 
	ON [User].[AspNetUsers]
	(
		[NormalizedEmail] ASC
	)
	ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] 
	ON [User].[AspNetUsers]
	(
		[NormalizedUserName] ASC
	)
	WHERE ([NormalizedUserName] IS NOT NULL)
	ON [PRIMARY]
GO

/*
	[AspNetRoleClaims]
*/

CREATE TABLE [User].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,

	CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] 
	ON [User].[AspNetRoleClaims]
	(
		[RoleId] ASC
	) 
	ON [PRIMARY]
GO

ALTER TABLE [User].[AspNetRoleClaims]  
	WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
	FOREIGN KEY([RoleId])
	REFERENCES [User].[AspNetRoles] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [User].[AspNetRoleClaims] 
	CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO

/*
	[AspNetUserClaims]
*/

CREATE TABLE [User].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,

	CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] 
	ON [User].[AspNetUserClaims]
	(
		[UserId] ASC
	) 
	ON [PRIMARY]
GO

ALTER TABLE [User].[AspNetUserClaims]  
	WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
	FOREIGN KEY([UserId])
	REFERENCES [User].[AspNetUsers] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [User].[AspNetUserClaims] 
	CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO

/*
	[AspNetUserLogins]
*/

CREATE TABLE [User].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [uniqueidentifier] NOT NULL,

	CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
	(
		[LoginProvider] ASC,
		[ProviderKey] ASC
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId]
	ON [User].[AspNetUserLogins]
	(
		[UserId] ASC
	) 
	ON [PRIMARY]
GO

ALTER TABLE [User].[AspNetUserLogins] 
	WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] 
	FOREIGN KEY([UserId])
	REFERENCES [User].[AspNetUsers] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [User].[AspNetUserLogins] 
	CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO

/*
	[AspNetUserRoles]
*/

CREATE TABLE [User].[AspNetUserRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,

	CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[RoleId] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] 
	ON [User].[AspNetUserRoles]
	(
		[RoleId] ASC
	) 
	ON [PRIMARY]
GO

ALTER TABLE [User].[AspNetUserRoles] 
	WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] 
	FOREIGN KEY([RoleId])
	REFERENCES [User].[AspNetRoles] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [User].[AspNetUserRoles]
	CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO

ALTER TABLE [User].[AspNetUserRoles]  
	WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
	FOREIGN KEY([UserId])
	REFERENCES [User].[AspNetUsers] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [User].[AspNetUserRoles] 
	CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO

/*
	[AspNetUserTokens]
*/

CREATE TABLE [User].[AspNetUserTokens](
	[UserId] [uniqueidentifier] NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,

	CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
	(
		[UserId] ASC,
		[LoginProvider] ASC,
		[Name] ASC
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [User].[AspNetUserTokens] 
	WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
	FOREIGN KEY([UserId])
	REFERENCES [User].[AspNetUsers] ([Id])
	ON DELETE CASCADE
GO

ALTER TABLE [User].[AspNetUserTokens] 
	CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO

/*
	[Address]
*/

CREATE TABLE [User].[Address](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](256) NOT NULL,
	[Address1] [nvarchar](256) NOT NULL,
	[Address2] [nvarchar](256) NOT NULL,
	[Zip] [nvarchar](256) NOT NULL,
	[City] [nvarchar](256) NOT NULL,
	[State] [nvarchar](256) NULL,
	[Country] [nvarchar](256) NOT NULL,
	[UserId] [uniqueidentifier] NULL,

	CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Address_UserId] 
	ON [User].[Address]
	(
		[UserId] ASC
	)
	WHERE ([UserId] IS NOT NULL)
	ON [PRIMARY]
GO

ALTER TABLE [User].[Address] 
	WITH CHECK ADD  CONSTRAINT [FK_Address_AspNetUsers_UserId] 
	FOREIGN KEY([UserId])
	REFERENCES [User].[AspNetUsers] ([Id])
GO

ALTER TABLE [User].[Address]
	CHECK CONSTRAINT [FK_Address_AspNetUsers_UserId]
GO