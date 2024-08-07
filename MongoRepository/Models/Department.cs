using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoRepository.Models
{
    public class Department
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? DepartmentName { get; set; }
        public int DepartmentId { get; set; }
    }
}
