using MongoDB.Bson;
using MongoDB.Driver;
using Project_Backend.Models;

namespace Project_Backend.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderService(IConfiguration config)
        {
            var connectionString = config["MongoDbSettings:ConnectionString"];
            var databaseName = config["MongoDbSettings:DatabaseName"];
            var collectionName = config["MongoDbSettings:OrdersCollectionName"];

            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);

            _ordersCollection = mongoDatabase.GetCollection<Order>(collectionName);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _ordersCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<Order>> SearchOrdersAsync(string searchTerm)
        {
            var filter = Builders<Order>.Filter.Regex(
                "Name",
                new BsonRegularExpression(searchTerm, "i"));

            return await _ordersCollection.Find(filter).ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(string orderId, string newStatus)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId);

            var update = Builders<Order>.Update
                .Set(o => o.OrderStatus, newStatus);

            await _ordersCollection.UpdateOneAsync(filter, update);
        }
    }
}