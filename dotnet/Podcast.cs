using System;
using Sabio.Models.Domain.Users;

namespace Sabio.Models.Domain.Podcasts
{
    public class Podcast
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public LookUp PodcastTypeId { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public BaseUser CreatedBy { get; set; }
        public BaseUser ModifiedBy { get; set;}

    }
}
