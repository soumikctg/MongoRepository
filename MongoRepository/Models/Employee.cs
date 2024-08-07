using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoRepository.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("Name")]
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public int TeamId { get; set; }

    }
}
