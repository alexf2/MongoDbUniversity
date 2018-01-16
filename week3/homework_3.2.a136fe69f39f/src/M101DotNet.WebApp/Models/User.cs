using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace M101DotNet.WebApp.Models
{
    public class User
    {
        [BsonId]
        public ObjectId UserId { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
    }
}