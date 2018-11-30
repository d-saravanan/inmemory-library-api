using AutoFixture;
using LibMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibMS.Services
{
    public class MediaServices
    {
        private static Fixture _fixture = new Fixture();

        private static List<MediaType> mediaTypes = new List<MediaType>
        {
            new MediaType { Id = Guid.NewGuid(), Name = "Book" },
            new MediaType { Id = Guid.NewGuid(), Name = "Audio CD" },
            new MediaType { Id = Guid.NewGuid(), Name = "Video CD" },
            new MediaType { Id = Guid.NewGuid(), Name = "DVD" }
        };

        static string[] _videoClips = new string[]
        {
            "https://www.youtube.com/watch?v=CAnFIZothRg",
            "https://www.youtube.com/watch?v=9GAWCIT0rjc",
            "https://www.youtube.com/watch?v=6nq-xsOtFFQ",
            "https://www.youtube.com/watch?v=gw1L_UZRfAA",
            "https://www.youtube.com/watch?v=IsEm7T3qR78",
            "https://www.youtube.com/watch?v=lxszogJfpAY"
        };

        static string[] audioClips = new[]
        {
            "https://www.youtube.com/watch?v=eaDMezjH3jI",
            "https://www.youtube.com/watch?v=s8z2gqA_7mU",
            "https://www.youtube.com/watch?v=AyEiL2U_TKk",
            "https://www.youtube.com/watch?v=1AYBTHYz18I"
        };

        static Dictionary<Guid, Media> _ = new Dictionary<Guid, Media>();
        public IEnumerable<Media> GetAllMedia()
        {
            if (_.Count < 1)
            {
                var mediadata = Media.CreateManyMedia(25);

                for (int i = 0; i < mediadata.Count(); i++)
                {
                    var mediaType = mediaTypes[i % mediaTypes.Count];
                    mediadata.ElementAt(i).Category = mediaType;

                    if (mediaType.Name.Contains("Video") || mediaType.Name.Contains("DVD"))
                        mediadata.ElementAt(i).Link = _videoClips[i % _videoClips.Length];
                    else if (mediaType.Name.Contains("audio"))
                        mediadata.ElementAt(i).Link = audioClips[i % audioClips.Length];
                }

                _ = mediadata.ToDictionary(t => t.Id, t => t);
            }
            return _.Values;
        }

        public IEnumerable<Media> GetMediaByCategoryName(string catName)
                => _.Values.Where(m => m.Category.Name.Equals(catName, StringComparison.OrdinalIgnoreCase));

        public Media GetById(Guid id) => _.ContainsKey(id) ? _[id] : null;
    }


}