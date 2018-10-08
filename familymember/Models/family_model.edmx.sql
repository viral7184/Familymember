
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/18/2017 10:57:55
-- Generated from EDMX file: C:\Users\PROCORNER EDUFLEX 29\Documents\Visual Studio 2015\Projects\familymember\familymember\Models\family_model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [member];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_tbl_address_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_address] DROP CONSTRAINT [FK_tbl_address_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_events_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_events] DROP CONSTRAINT [FK_tbl_events_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_group2_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_groupadmin] DROP CONSTRAINT [FK_tbl_group2_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_images_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_images] DROP CONSTRAINT [FK_tbl_images_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_member_role_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_member_role] DROP CONSTRAINT [FK_tbl_member_role_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_member_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_member] DROP CONSTRAINT [FK_tbl_member_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_videos_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_videos] DROP CONSTRAINT [FK_tbl_videos_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_tbl_village_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tbl_village] DROP CONSTRAINT [FK_tbl_village_ToTable];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[tbl_address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_address];
GO
IF OBJECT_ID(N'[dbo].[tbl_events]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_events];
GO
IF OBJECT_ID(N'[dbo].[tbl_groupadmin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_groupadmin];
GO
IF OBJECT_ID(N'[dbo].[tbl_groupuser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_groupuser];
GO
IF OBJECT_ID(N'[dbo].[tbl_images]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_images];
GO
IF OBJECT_ID(N'[dbo].[tbl_member]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_member];
GO
IF OBJECT_ID(N'[dbo].[tbl_member_role]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_member_role];
GO
IF OBJECT_ID(N'[dbo].[tbl_notification]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_notification];
GO
IF OBJECT_ID(N'[dbo].[tbl_relationship]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_relationship];
GO
IF OBJECT_ID(N'[dbo].[tbl_surname]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_surname];
GO
IF OBJECT_ID(N'[dbo].[tbl_videos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_videos];
GO
IF OBJECT_ID(N'[dbo].[tbl_village]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tbl_village];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'tbl_address'
CREATE TABLE [dbo].[tbl_address] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MEMBER_ID] int  NULL,
    [STREET] varchar(50)  NULL,
    [CITY] varchar(50)  NULL,
    [STATE] varchar(50)  NULL,
    [PINCODE] varchar(8)  NULL,
    [IS_ACTIVE] bit  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_events'
CREATE TABLE [dbo].[tbl_events] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MEMBER_ID] int  NULL,
    [EVENTNAME] varchar(100)  NULL,
    [EVENTDATE] datetime  NULL,
    [CREATED_BY] varchar(20)  NULL,
    [IS_ACTIVE] bit  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_groupadmin'
CREATE TABLE [dbo].[tbl_groupadmin] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MEMBER_ID] int  NULL,
    [GROUP_NAME] nvarchar(50)  NULL,
    [IS_ACTIVE] bit  NULL,
    [CREATED_DATE] datetime  NULL,
    [IS_DELETE] bit  NULL
);
GO

-- Creating table 'tbl_groupuser'
CREATE TABLE [dbo].[tbl_groupuser] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GROUP_NAME] nvarchar(50)  NULL,
    [MEMBER_NAME] nvarchar(50)  NULL,
    [ISADMIN] bit  NULL,
    [USER_ADDED_DATE] datetime  NULL,
    [ISNOTIFY] bit  NULL,
    [ISACTIVE] bit  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_images'
CREATE TABLE [dbo].[tbl_images] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MEMBER_ID] int  NULL,
    [IMAGE] varchar(max)  NULL,
    [IS_ACTIVE] bit  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_member'
CREATE TABLE [dbo].[tbl_member] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FIRSTNAME] nvarchar(max)  NOT NULL,
    [SURNAME] varchar(50)  NULL,
    [EMAIL] varchar(50)  NULL,
    [PASSWORD] varchar(12)  NULL,
    [MOBILE1] varchar(12)  NULL,
    [MOBILE2] varchar(12)  NULL,
    [DOB] datetime  NULL,
    [IMAGE] varchar(max)  NULL,
    [BLOODGROUP] varchar(10)  NULL,
    [EDUCATION] varchar(50)  NULL,
    [RELATIONSHIP] int  NULL,
    [CREATED_DATE] datetime  NULL,
    [MODIFIED_DATE] datetime  NULL,
    [IS_ACTIVE] bit  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_member_role'
CREATE TABLE [dbo].[tbl_member_role] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MEMBER_ID] int  NULL,
    [MEMBER_ROLE] varchar(20)  NULL,
    [CREATED_DATE] datetime  NULL,
    [MODIFIED_DATE] datetime  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_notification'
CREATE TABLE [dbo].[tbl_notification] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GROUP_NAME] nvarchar(50)  NULL,
    [MEMBER_NAME] nvarchar(50)  NULL,
    [MESSAGE] nvarchar(150)  NULL,
    [ADMIN] nvarchar(50)  NULL,
    [IS_NOTIFY] bit  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_relationship'
CREATE TABLE [dbo].[tbl_relationship] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RELATIONSHIP] varchar(50)  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_surname'
CREATE TABLE [dbo].[tbl_surname] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SURNAME] nvarchar(50)  NULL,
    [IS_ACTIVE] bit  NULL,
    [IS_DELETED] nchar(10)  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_videos'
CREATE TABLE [dbo].[tbl_videos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MEMBER_ID] int  NULL,
    [VIDEO] varchar(max)  NULL,
    [IS_ACTIVE] bit  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- Creating table 'tbl_village'
CREATE TABLE [dbo].[tbl_village] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MEMBER_ID] int  NULL,
    [VILLAGE] varchar(50)  NULL,
    [SUBDISTRICT] varchar(50)  NULL,
    [DISTRICT] varchar(50)  NULL,
    [IS_DELETED] bit  NULL,
    [LOGIN_BY] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'tbl_address'
ALTER TABLE [dbo].[tbl_address]
ADD CONSTRAINT [PK_tbl_address]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_events'
ALTER TABLE [dbo].[tbl_events]
ADD CONSTRAINT [PK_tbl_events]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_groupadmin'
ALTER TABLE [dbo].[tbl_groupadmin]
ADD CONSTRAINT [PK_tbl_groupadmin]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_groupuser'
ALTER TABLE [dbo].[tbl_groupuser]
ADD CONSTRAINT [PK_tbl_groupuser]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_images'
ALTER TABLE [dbo].[tbl_images]
ADD CONSTRAINT [PK_tbl_images]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_member'
ALTER TABLE [dbo].[tbl_member]
ADD CONSTRAINT [PK_tbl_member]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_member_role'
ALTER TABLE [dbo].[tbl_member_role]
ADD CONSTRAINT [PK_tbl_member_role]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_notification'
ALTER TABLE [dbo].[tbl_notification]
ADD CONSTRAINT [PK_tbl_notification]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_relationship'
ALTER TABLE [dbo].[tbl_relationship]
ADD CONSTRAINT [PK_tbl_relationship]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_surname'
ALTER TABLE [dbo].[tbl_surname]
ADD CONSTRAINT [PK_tbl_surname]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_videos'
ALTER TABLE [dbo].[tbl_videos]
ADD CONSTRAINT [PK_tbl_videos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tbl_village'
ALTER TABLE [dbo].[tbl_village]
ADD CONSTRAINT [PK_tbl_village]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MEMBER_ID] in table 'tbl_address'
ALTER TABLE [dbo].[tbl_address]
ADD CONSTRAINT [FK_tbl_address_ToTable]
    FOREIGN KEY ([MEMBER_ID])
    REFERENCES [dbo].[tbl_member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_address_ToTable'
CREATE INDEX [IX_FK_tbl_address_ToTable]
ON [dbo].[tbl_address]
    ([MEMBER_ID]);
GO

-- Creating foreign key on [MEMBER_ID] in table 'tbl_events'
ALTER TABLE [dbo].[tbl_events]
ADD CONSTRAINT [FK_tbl_events_ToTable]
    FOREIGN KEY ([MEMBER_ID])
    REFERENCES [dbo].[tbl_member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_events_ToTable'
CREATE INDEX [IX_FK_tbl_events_ToTable]
ON [dbo].[tbl_events]
    ([MEMBER_ID]);
GO

-- Creating foreign key on [MEMBER_ID] in table 'tbl_groupadmin'
ALTER TABLE [dbo].[tbl_groupadmin]
ADD CONSTRAINT [FK_tbl_group2_ToTable]
    FOREIGN KEY ([MEMBER_ID])
    REFERENCES [dbo].[tbl_member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_group2_ToTable'
CREATE INDEX [IX_FK_tbl_group2_ToTable]
ON [dbo].[tbl_groupadmin]
    ([MEMBER_ID]);
GO

-- Creating foreign key on [MEMBER_ID] in table 'tbl_images'
ALTER TABLE [dbo].[tbl_images]
ADD CONSTRAINT [FK_tbl_images_ToTable]
    FOREIGN KEY ([MEMBER_ID])
    REFERENCES [dbo].[tbl_member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_images_ToTable'
CREATE INDEX [IX_FK_tbl_images_ToTable]
ON [dbo].[tbl_images]
    ([MEMBER_ID]);
GO

-- Creating foreign key on [MEMBER_ID] in table 'tbl_member_role'
ALTER TABLE [dbo].[tbl_member_role]
ADD CONSTRAINT [FK_tbl_member_role_ToTable]
    FOREIGN KEY ([MEMBER_ID])
    REFERENCES [dbo].[tbl_member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_member_role_ToTable'
CREATE INDEX [IX_FK_tbl_member_role_ToTable]
ON [dbo].[tbl_member_role]
    ([MEMBER_ID]);
GO

-- Creating foreign key on [RELATIONSHIP] in table 'tbl_member'
ALTER TABLE [dbo].[tbl_member]
ADD CONSTRAINT [FK_tbl_member_ToTable]
    FOREIGN KEY ([RELATIONSHIP])
    REFERENCES [dbo].[tbl_relationship]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_member_ToTable'
CREATE INDEX [IX_FK_tbl_member_ToTable]
ON [dbo].[tbl_member]
    ([RELATIONSHIP]);
GO

-- Creating foreign key on [MEMBER_ID] in table 'tbl_videos'
ALTER TABLE [dbo].[tbl_videos]
ADD CONSTRAINT [FK_tbl_videos_ToTable]
    FOREIGN KEY ([MEMBER_ID])
    REFERENCES [dbo].[tbl_member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_videos_ToTable'
CREATE INDEX [IX_FK_tbl_videos_ToTable]
ON [dbo].[tbl_videos]
    ([MEMBER_ID]);
GO

-- Creating foreign key on [MEMBER_ID] in table 'tbl_village'
ALTER TABLE [dbo].[tbl_village]
ADD CONSTRAINT [FK_tbl_village_ToTable]
    FOREIGN KEY ([MEMBER_ID])
    REFERENCES [dbo].[tbl_member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tbl_village_ToTable'
CREATE INDEX [IX_FK_tbl_village_ToTable]
ON [dbo].[tbl_village]
    ([MEMBER_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------