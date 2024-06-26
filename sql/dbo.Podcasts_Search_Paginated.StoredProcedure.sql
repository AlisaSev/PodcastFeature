USE [Tabi]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_Search_Paginated]    Script Date: 6/18/2024 7:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================================================
-- Author: Alisa Sevastianova
-- Create Date: 05/31/2024
-- Description: A stored procedure to search Podcasts in a paginated way.
--              Additionally joins the PodcastType table and Users table.
-- Code Reviewer: Hannah Silvers
-- =========================================================================================

CREATE PROC [dbo].[Podcasts_Search_Paginated]
@PageIndex int,
@PageSize int,
@SearchTerm nvarchar(100)

AS

/*
Execute dbo.Podcasts_Search_Paginated @PageIndex = 0, @PageSize = 10, @SearchTerm = "Test"

*/

BEGIN
    SET NOCOUNT ON;

    DECLARE @offset int = @PageIndex * @PageSize;

    SELECT 
        p.[Id],
        p.[Title],
        p.[Description],
        p.[Url],
        pt.[Name] AS PodcastTypeName,
        p.[CoverImageUrl],
        p.[DateCreated],
        p.[DateModified]
        ,dbo.fn_GetUserJSON(p.CreatedBy) as CreatedBy
        ,dbo.fn_GetUserJSON(p.ModifiedBy) as ModifiedBy
        ,TotalCount = COUNT(1) OVER()
    FROM 
        [dbo].[Podcasts] p
    INNER JOIN 
        [dbo].[PodcastType] pt ON pt.Id = p.PodcastTypeId
    WHERE 
        p.[Title] LIKE '%' + @SearchTerm + '%'
        OR p.[Description] LIKE '%' + @SearchTerm + '%'
    ORDER BY 
        p.Id
    OFFSET 
        @offset ROWS
    FETCH NEXT 
        @PageSize ROWS ONLY;
END
GO
