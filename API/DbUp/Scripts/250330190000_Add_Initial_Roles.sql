INSERT INTO [User].[Role] ([Id], [Name], [NormalizedName])
VALUES 
	(NEWID(), 'SuperAdmin', 'SUPERADMIN'),
	(NEWID(), 'Admin', 'ADMIN'),
	(NEWID(), 'Vendor', 'VENDOR'),
	(NEWID(), 'Member', 'MEMBER')