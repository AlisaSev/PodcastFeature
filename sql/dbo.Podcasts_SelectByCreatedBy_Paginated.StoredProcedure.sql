USE [Tabi]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_SelectByCreatedBy_Paginated]    Script Date: 6/18/2024 7:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================================================
-- Author: Alisa Sevastianova
-- Create Date: 05/31/2024
-- Description: A select procedure to select Podcasts by Created By in a paginated way.
--				Additionally implements inner joins of EventTypes table, Venues Table, 
--				Files Table, and Event Status Table.
-- Code Reviewer:
-- =========================================================================================

CREATE PROC [dbo].[Podcasts_SelectByCreatedBy_Paginated]

@PageIndex int,
@PageSize int,
@CreatedBy int

AS

/*

Execute dbo.Podcasts_SelectByCreatedBy_Paginated @PageIndex=0, @PageSize= 10, @CreatedBy = 1

*/

BEGIN

Declare @offset int = @PageIndex * @PageSize

SELECT p.[Id]
      ,p.[Title]
      ,p.[Description]
      ,p.[Url]
      ,p.[PodcastTypeId]
      ,pt.[Name] as PodcastTypeName
      ,p.[CoverImageUrl]
      ,p.[DateCreated]
      ,p.[DateModified]
      ,dbo.fn_GetUserJSON(p.CreatedBy) as CreatedBy
      ,dbo.fn_GetUserJSON(p.ModifiedBy) as ModifiedBy
      , TotalCount = COUNT(1) OVER()

  FROM [dbo].[Podcasts] p
  join dbo.PodcastType pt ON pt.Id = p.PodcastTypeId
 
  WHERE p.CreatedBy = @CreatedBy
  ORDER BY p.Id
  OFFSET @offset ROWS
 FETCH NEXT @PageSize Rows ONLY

END

GO
