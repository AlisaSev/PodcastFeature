USE [Tabi]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_Update]    Script Date: 6/18/2024 7:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================
-- Author: Alisa Sevastianova
-- Create Date: 06/01/2024
-- Description: A update procedure to update Podcasts details.
-- 
-- =========================================================================================

CREATE proc [dbo].[Podcasts_Update]

@Id int 
,@Title nvarchar(50)
,@Description nvarchar(200)
,@Url nvarchar(200)
,@PodcastTypeId int
,@CoverImageUrl nvarchar(200)
,@ModifiedBy int

AS
/*
Example usage:
Declare @Id int = 3
      ,@Title nvarchar(50) = 'New Podcast Title'
      ,@Description nvarchar(200) = 'Updated podcast description.'
      ,@Url nvarchar(200) = 'https://example.com/new-podcast-url'
      ,@PodcastTypeId int = 1
      ,@CoverImageUrl nvarchar(200) = 'https://example.com/new-cover-image-url'
      ,@DateModified datetime2(7) = '2024-06-01'
      ,@ModifiedBy int = 1

Execute dbo.Podcasts_Update @Id 
                            ,@Title 
                            ,@Description 
                            ,@Url 
                            ,@PodcastTypeId 
                            ,@CoverImageUrl 
                            ,@DateModified
                            ,@ModifiedBy
*/

BEGIN
DECLARE @DateModified datetime2 =  GETUTCDATE()
UPDATE [dbo].[Podcasts]
   SET [Title] = @Title
      ,[Description] = @Description
      ,[Url] = @Url
      ,[PodcastTypeId] = @PodcastTypeId
      ,[CoverImageUrl] = @CoverImageUrl
      ,[DateModified] = @DateModified
      ,[ModifiedBy] = @ModifiedBy
 WHERE [Id] = @Id

END
GO
