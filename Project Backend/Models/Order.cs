using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project_Backend.Models
{
    [BsonIgnoreExtraElements]
    public class Order
    {
        /// <summary>
        /// This is the main class for orders, it contains the user id of the user who made the order, the date of the order, the status of the order, an array of OrderItems and the total value of the order
        /// </summary>
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
    /// <summary>
    /// There is one OrderItem per unique product in each order of products, and it contains the product id, quantity, price at purchase and discount percentage at purchase
    /// </summary>
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