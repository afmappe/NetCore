﻿CREATE TABLE [dbo].[CORE_USER]
(
	[id] INT IDENTITY NOT NULL,
	[creation_date] DATETIME NOT NULL,
	[full_name] NVARCHAR(200) NOT NULL,
	[user_name] NVARCHAR(200) NOT NULL,

	CONSTRAINT [PK_CORE_USER] PRIMARY KEY ([id]),
)
