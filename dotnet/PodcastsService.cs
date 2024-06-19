using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Podcasts;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Podcasts;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class PodcastsService : IPodcastsService
    {
        IDataProvider _data = null;
        ILookUpService _lookUp = null;

        public PodcastsService(IDataProvider data, ILookUpService lookUp)
        {
            _data = data;
            _lookUp = lookUp;
        }

        #region DELETE BY ID 
        public void Delete(int id)
        {
            string procName = "[dbo].[Podcasts_DeleteById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);

            }, returnParameters: null);
        }
        #endregion

        #region UPDATE
        public void Update(PodcastsUpdateRequest model, int currentId)
        {
            string procName = "[dbo].[Podcasts_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@ModifiedBy", currentId);

            }, returnParameters: null);
        }
        #endregion

        #region INSERT
        public int Add(PodcastsAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Podcasts_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@CreatedBy", userId);
                col.AddWithValue("@ModifiedBy", userId);


                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object objId = returnCol["@Id"].Value;
                int.TryParse(objId.ToString(), out id);
            });

            return id;
        }
        #endregion

        #region GET BY CREATED BY
        public Paged<Podcast> GetByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
            Paged<Podcast> pagedList = null;
            List<Podcast> podcastList = null;
            int totalCount = 0;

            string procName = "[dbo].[Podcasts_SelectByCreatedBy_Paginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@CreatedBy", createdBy);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Podcast podcast = MapSinglePodcast(reader, ref index);
                totalCount = reader.GetSafeInt32(index);

                if (podcastList == null)
                {
                    podcastList = new List<Podcast>();
                }
                podcastList.Add(podcast);
            });

            if (podcastList != null)
            {
                pagedList = new Paged<Podcast>(podcastList, pageIndex, pageSize, totalCount);
            }
            
            return pagedList;
        }
        #endregion

        #region PAGINATION
        public Paged<Podcast> GetPage(int pageIndex, int pageSize)
        {
            Paged<Podcast> pagedList = null;
            List<Podcast> podcastList = null;
            int totalCount = 0;

            string procName = "[dbo].[Podcasts_SelectAll_Paginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);

            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Podcast podcast = MapSinglePodcast(reader, ref index);
                totalCount = reader.GetSafeInt32(index);

                if (podcastList == null)
                {
                    podcastList = new List<Podcast>();
                }
                podcastList.Add(podcast);
            });

            if (podcastList != null)
            {
                pagedList = new Paged<Podcast>(podcastList, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }
        #endregion

        #region SEARCH PAGINATED
        public Paged<Podcast> SearchPaginated(int pageIndex, int pageSize, string searchTerm)
        {
            Paged<Podcast> pagedList = null;
            List<Podcast> podcastList = null;
            int totalCount = 0;

            string procName = "[dbo].[Podcasts_Search_Paginated]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@SearchTerm", searchTerm);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                Podcast podcast = MapSinglePodcast(reader, ref index);
                totalCount = reader.GetSafeInt32(index);

                if (podcastList == null)
                {
                    podcastList = new List<Podcast>();
                }
                podcastList.Add(podcast);
            });

            if (podcastList != null)
            {
                pagedList = new Paged<Podcast>(podcastList, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }
        #endregion


        #region COMMON PARAMS
        private static void AddCommonParams(PodcastsAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Url", model.Url);
            col.AddWithValue("@PodcastTypeId", model.PodcastTypeId);
            col.AddWithValue("@CoverImageUrl", model.CoverImageUrl);
      


        }
        #endregion

        #region SINGLE MAPPER
        private Podcast MapSinglePodcast(IDataReader reader, ref int index)
        {
            Podcast podcast = new Podcast();

            podcast.Id = reader.GetSafeInt32(index++);
            podcast.Title = reader.GetSafeString(index++);
            podcast.Description = reader.GetSafeString(index++);
            podcast.Url = reader.GetSafeString(index++);
            podcast.PodcastTypeId = _lookUp.MapSingleLookUp(reader, ref index);
            podcast.CoverImageUrl = reader.GetSafeString(index++);
            podcast.DateCreated = reader.GetSafeDateTime(index++);
            podcast.DateModified = reader.GetSafeDateTime(index++);
            podcast.CreatedBy = reader.DeserializeObject<BaseUser>(index++);
            podcast.ModifiedBy = reader.DeserializeObject<BaseUser>(index++);

            return podcast;
        }
        #endregion
    }
}
