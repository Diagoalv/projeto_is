CREATE TABLE [dbo].[Apps] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50)   NULL,
    [Creation_DataTime] Datetime    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
CREATE TABLE [dbo].[Cont] (
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50)   NULL,
    [Creation_DataTime] DATETIME    NULL,
    [Parent]    INT     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
    );
CREATE TABLE [dbo].[Notif](
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50)   NULL,
    [Creation_DataTime] DATETIME    NULL,
    [Parent]    INT      NULL,
    [Event]     INT        NULL,
    [Endpoint]  VARCHAR(250)    NOT NULL,
    [Enabled]   BOOL         NOT NULL
    PRIMARY KEY CLUSTERED ([Id] ASC)

);
CREATE TABLE [dbo].[Record](
    [Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50)   NULL,
    [Content]   VARCHAR(50)     NULL,
    [Creation_DataTime] Datetime    NULL,
    [Parent]        VARCHAR(50)     NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);