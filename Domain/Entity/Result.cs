using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entity
{
    public class Result
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public int PlayerOneId { get; set; }
        public int PlayerTwoId { get; set; }
        
        public int WinnerId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime PlayedAt { get; set; }
    }
}
