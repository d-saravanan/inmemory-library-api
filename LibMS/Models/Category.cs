using AutoFixture;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LibMS.Models
{
    public class MediaType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class Media
    {
        public Guid Id { get; set; }
        public MediaType Category { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string UniqueId { get; set; }
        public string Classification { get; set; }
        public DateTime CreatedDate { get; set; }
        public float Rating { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }

        public static IEnumerable<Media> CreateManyMedia(int count)
        {
            Fixture f = new Fixture();
            List<Media> _ = new List<Media>();
            for (int i = 0; i < count; i++)
            {
                _.Add(
                new Media
                {
                    Id = Guid.NewGuid(),
                    Author = Path.GetRandomFileName() + i,
                    Category = new MediaType(),
                    Classification = Path.GetRandomFileName() + i,
                    CreatedDate = f.Create<DateTime>(),
                    Description = Path.GetRandomFileName() + i,
                    Link = $"https://{Path.GetRandomFileName()}.com",
                    Price = f.Create<decimal>(),
                    Rating = f.Create<float>(),
                    Title = Path.GetRandomFileName() + i,
                    UniqueId = Guid.NewGuid().ToString()
                });
            }
            return _;
        }

        public static Media CreateMedia()
        {
            Fixture f = new Fixture();
            return new Media
            {
                Id = Guid.NewGuid(),
                Author = Path.GetRandomFileName(),
                Category = new MediaType(),
                Classification = Path.GetRandomFileName(),
                CreatedDate = f.Create<DateTime>(),
                Description = Path.GetRandomFileName(),
                Link = $"https://{Path.GetRandomFileName()}.com",
                Price = f.Create<decimal>(),
                Rating = f.Create<float>(),
                Title = Path.GetRandomFileName(),
                UniqueId = Guid.NewGuid().ToString()
            };
        }
    }
}