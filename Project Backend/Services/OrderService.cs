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
            // Read the settings from appsettings.json
            var connectionString = config["MongoDbSettings:ConnectionString"];
            var databaseName = config["MongoDbSettings:DatabaseName"];
            var collectionName = config["MongoDbSettings:OrdersCollectionName"];

            // Connect to MongoDB
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            _ordersCollection = mongoDatabase.GetCollection<Order>(collectionName);
        }

        // 1. Get ALL products for the dashboard
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _ordersCollection.Find(_ => true).ToListAsync();
        }

        // 2. Search function (e.g., search by name)
        public async Task<List<Order>> SearchOrdersAsync(string searchTerm)
        {
            // This does a case-insensitive search anywhere in the product name
            var filter = Builders<Order>.Filter.Regex("Name", new BsonRegularExpression(searchTerm, "i"));
            return await _ordersCollection.Find(filter).ToListAsync();
        }
    }
}