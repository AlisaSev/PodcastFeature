USE [Tabi]
GO
/****** Object:  StoredProcedure [dbo].[PodcastType_SelectAll]    Script Date: 6/18/2024 7:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Alisa Sevastianova
-- Create date: 06/01/2024
-- Description:	A select procedure to retrieve all Podcast Types.
-- Code Reviewer: Hannah Silvers
-- =============================================


CREATE proc [dbo].[PodcastType_SelectAll]
AS

/*
Execute dbo.PodcastType_SelectAll
*/

BEGIN

SELECT 
    [Id],
    [Name]
FROM 
    [dbo].[PodcastType]


END

GO
