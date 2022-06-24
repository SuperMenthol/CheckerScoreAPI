using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entity
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public int PlayerId { get; set; }

        [BsonElement("Name")]
        public string Login { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
