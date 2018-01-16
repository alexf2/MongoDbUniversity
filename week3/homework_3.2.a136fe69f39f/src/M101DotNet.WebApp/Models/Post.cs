using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class Post
    {
        public Post()
        {
            Tags = new List<string>();
            Comments = new List<Comment>();
        }

        [BsonId]
        public ObjectId PostId { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public IList<string> Tags { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public IList<Comment> Comments { get; set; }
    }
}