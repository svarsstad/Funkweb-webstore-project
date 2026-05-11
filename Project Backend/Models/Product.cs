using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project_Backend.Models
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Put the EXACT spelling/capitalization from MongoDB inside the quotes here
        [BsonElement("productName")] // <--- mongodb name
        public string Name { get; set; } = string.Empty; // <--- C# class member variable tied to the mongodb name

        [BsonElement("productStyle")] // <--- mongodb name
        public string ProductStyle { get; set; } = string.Empty;

        [BsonElement("productCategory")] // <--- mongodb name
        public string ProductCategory { get; set; } = string.Empty;

        [BsonElement("productSlogan")] // <--- mongodb name
        public string ProductSlogan { get; set; } = string.Empty;

        [BsonElement("description")] // <--- mongodb name
        public string description { get; set; } = string.Empty;


        [BsonElement("specs")]
        public Dictionary<string, Dictionary<string, string>> Specs { get; set; } = new();
        // specs contain a dictionary of 2: Product Specifications and Technical Data Table
        // which each contain dictionary-lists of characteristics and their descriptions
        // I thought I could later use this to make search tags or propose similar products based on shared characteristics

        [BsonElement("Images")]
        public Dictionary<int, string> Images { get; set; } = new();
        //image paths, the key is just an index for the image, the value is the path to the image in the file system, relative to /wwwroot/

        [BsonElement("Warrenty disclaimber")] // <--- mongodb name
        public string Disclaimber { get; set; } = string.Empty;


        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("currency")]
        public string Curruency { get; set; } = "CZK-IID";

        [BsonElement("stock")]
        public decimal Stock { get; set; }


       /* [BsonExtraElements]
        public BsonDocument? ExtraData { get; set; }
       */
    }
}