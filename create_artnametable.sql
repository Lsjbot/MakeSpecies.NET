USE [COL2019]
GO

/****** Object:  Table [dbo].[artnametable]    Script Date: 2020-11-03 10:35:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[artnametable](
	[id] [int] NOT NULL,
	[taxonid] [int] NULL,
	[lang] [varchar](3) NULL,
	[articlename] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[artnametable]  WITH CHECK ADD FOREIGN KEY([taxonid])
REFERENCES [dbo].[Taxon] ([taxonID])
GO


