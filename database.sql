-- Creates a table named Articles with columns Id, Name, Category, and Price. Id is an auto-incrementing primary key.
CREATE TABLE [dbo].[Articles] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50)   NULL,
    [Category] NVARCHAR (50)   NULL,
    [Price]    DECIMAL (10, 2) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- Inserts a record into the AspNetUsers table, seeding it with initial data for an admin user.
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) 
VALUES ('a47e32d2-492c-45ed-bbc0-e0d13b9f8c44', 'admin@gmail.com', 0, 'AN0EMtngz91niurmJF8jMxRWsWnzGv4YbvCv4u+LbE+Wt363Ox9okESGeIzkEm+X4g==', '77ac730d-1b9d-4255-84c6-95dfc0344e84', NULL, 0, 0, NULL, 1, 0, 'admin@gmail.com');

-- Inserts a record into the AspNetRoles table, seeding it with initial data for an admin role.
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (1, 'Admin');

-- Inserts a record into the AspNetUserRoles table, assigning the admin role to the admin user.
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES ('a47e32d2-492c-45ed-bbc0-e0d13b9f8c44', 1);

-- Creates a table named UserArticleRating with columns UserId, ArticleId, and Rating, and defines foreign key constraints.
CREATE TABLE [dbo].[UserArticleRating] (
    [UserId]    NVARCHAR (128) NOT NULL,
    [ArticleId] INT            NOT NULL,
    [Rating]    INT            NOT NULL,
    CONSTRAINT [PK_UserArticleRating] PRIMARY KEY CLUSTERED ([UserId] ASC, [ArticleId] ASC),
    CONSTRAINT [FK_UserArticleRating_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserArticleRating_Articles] FOREIGN KEY ([ArticleId]) REFERENCES [dbo].[Articles] ([Id]) ON DELETE CASCADE
);