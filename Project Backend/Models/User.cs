using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project_Backend.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("displayName")]
        public string DisplayName { get; set; } = string.Empty;

        [BsonElement("userImage")]
        public string? UserImage { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        // List of Order document ids
        [BsonElement("orders")]
        public List<string> Orders { get; set; } = new();

        // List of Product document ids
        [BsonElement("favoriteProductIds")]
        public List<string> FavoriteProductIds { get; set; } = new();

        [BsonElement("lastLogin")]
        public DateTime LastLogin { get; set; }

        [BsonElement("priviledge")]
        public int Priviledge { get; set; } = 3;
    }
}