using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoRepository.Models
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? TeamName { get; set; }
        public string? TeamId { get; set; }
        public string? DepartmentId { get; set; }

    }
}
