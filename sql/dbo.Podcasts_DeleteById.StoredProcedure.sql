USE [Tabi]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_DeleteById]    Script Date: 6/18/2024 7:28:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================================
-- Author: Alisa Sevastianova
-- Create Date: 05/31/2024
-- Description: A delete procedure to delete a Podcast by Id.
-- Code Reviewer: Hannah Silvers
-- =========================================================================================

CREATE proc [dbo].[Podcasts_DeleteById]

@Id int 

/*

Declare @Id int = 1
Execute [dbo].[Podcasts_DeleteById] @Id

*/
as

begin

DELETE FROM [dbo].[Podcasts]
      WHERE [Id] = @Id
end
GO
