USE [Tabi]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_Insert]    Script Date: 6/18/2024 7:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================
-- Author: Alisa Sevastianova
-- Create Date: 05/31/2024
-- Description: An insert procedure to create a Podcast.
-- 
-- =========================================================================================

CREATE proc [dbo].[Podcasts_Insert]

@Id int OUTPUT
,@Title nvarchar(50)
,@Description nvarchar(200)
,@Url nvarchar(200)
,@PodcastTypeId int
,@CoverImageUrl nvarchar(200)
,@CreatedBy int
,@ModifiedBy int

AS
BEGIN

    /*
    DECLARE @Id int
           ,@Title nvarchar(50) = 'Tabi Podcast'
           ,@Description nvarchar(200) = 'A podcast about evnts service.'
           ,@Url nvarchar(200) = 'http://example.com/tabipodcast'
           ,@PodcastTypeId int = 1
           ,@CoverImageUrl nvarchar(200) = 'http://example.com/images/tabipodcast.jpg'
           ,@DateCreated datetime2(7) = GETDATE()
           ,@DateModified datetime2(7) = GETDATE()
           ,@CreatedBy int = 1
           ,@ModifiedBy int = 1


    EXEC dbo.Podcasts_Insert 
         @Id OUTPUT
        ,@Title
        ,@Description
        ,@Url
        ,@PodcastTypeId
        ,@CoverImageUrl
        ,@DateCreated
        ,@DateModified
        ,@CreatedBy
        ,@ModifiedBy
    */


    INSERT INTO [dbo].[Podcasts]
           ([Title]
           ,[Description]
           ,[Url]
           ,[PodcastTypeId]
           ,[CoverImageUrl]
           ,[CreatedBy]
           ,[ModifiedBy])
    VALUES
           (@Title
           ,@Description
           ,@Url
           ,@PodcastTypeId
           ,@CoverImageUrl
           ,@CreatedBy
           ,@ModifiedBy)

    SET @Id = SCOPE_IDENTITY()

END
GO
