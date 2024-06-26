USE [Tabi]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_SelectAll_Paginated]    Script Date: 6/18/2024 7:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================
-- Author: Alisa Sevastianova
-- Create Date: 05/31/2024
-- Description: A select procedure to select all Podcasts in a paginated way.
--              Additionally implements inner joins of PodcastType table.
-- Code Reviewer: Hannah Silvers
-- =========================================================================================

CREATE proc [dbo].[Podcasts_SelectAll_Paginated]

@PageIndex int,
@PageSize int

AS

/*

Execute dbo.Podcasts_SelectAll_Paginated @PageIndex=0, @PageSize=8

*/

BEGIN

DECLARE @offset int = @PageIndex * @PageSize

SELECT p.[Id]
      ,p.[Title]
      ,p.[Description]
      ,p.[Url]
      ,pt.Id AS PodcastTypeId
	  ,pt.Name AS PodcastName
      ,p.[CoverImageUrl]
      ,p.[DateCreated]
      ,p.[DateModified]
      ,dbo.fn_GetUserJSON(p.CreatedBy) as CreatedBy
      ,dbo.fn_GetUserJSON(p.ModifiedBy) as ModifiedBy

      ,TotalCount = COUNT(1) OVER()

FROM [dbo].[Podcasts] p
JOIN [dbo].[PodcastType] pt ON pt.Id = p.PodcastTypeId
    JOIN 
        [dbo].[Users] uc ON uc.Id = p.CreatedBy
    JOIN 
        [dbo].[Users] um ON um.Id = p.ModifiedBy
ORDER BY p.Id
OFFSET @offset ROWS
FETCH NEXT @PageSize ROWS ONLY




END
GO
