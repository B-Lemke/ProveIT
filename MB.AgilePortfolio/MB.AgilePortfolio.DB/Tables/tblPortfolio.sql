﻿CREATE TABLE [dbo].[tblPortfolio]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(255) NOT NULL, 
    [Description] VARCHAR(255) NULL, 
    [PortfolioImage] VARCHAR(255) NULL, 
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    [PrivacyId] UNIQUEIDENTIFIER NOT NULL
)
