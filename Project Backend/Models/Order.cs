using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project_Backend.Models
{
    [BsonIgnoreExtraElements]
    public class Order
    {
        private OrderItem[] items = new OrderItem[0];

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Put the EXACT spelling/capitalization from MongoDB inside the quotes here
        [BsonRepresentation(BsonType.ObjectId)] // <--- mongodb name
        public string? UserId { get; set; } // <--- C# class member variable tied to the mongodb name

        [BsonElement("orderDate")] // <--- mongodb name
        public string OrderDate { get; set; } = string.Empty;

        [BsonElement("status")] // <--- mongodb name
        public string OrderStatus { get; set; } = string.Empty;

        [BsonElement("items")] // <--- mongodb name
        public OrderItem[] Items { get => items; set => items = value; }
        [BsonElement("totalOrderValue")] // <--- mongodb name
        public double TotalOrderValue { get; set; } = 0.0;

    }
    public class OrderItem
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ProductId { get; set; }
        [BsonElement("quantity")]
        public double Quantity { get; set; } = 0;
        [BsonElement("priceAtPurchase")]
        public double PriceAtPurchase { get; set; } = 0;
        [BsonElement("discountPercentage")]
        public double DiscountPercentage { get; set; } = 0;
    }
}