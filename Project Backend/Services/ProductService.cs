using MongoDB.Bson;
using MongoDB.Driver;
using Project_Backend.Models;

namespace Project_Backend.Services
{
    public class ProductService
    {
        public List<Product>? Products = new List<Product>();
        private readonly IMongoCollection<Product> _productsCollection;

        public ProductService(IConfiguration config)
        {
            // Read the settings from appsettings.json
            var connectionString = config["MongoDbSettings:ConnectionString"];
            var databaseName = config["MongoDbSettings:DatabaseName"];
            var collectionName = config["MongoDbSettings:ProductsCollectionName"];

            // Connect to MongoDB
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            _productsCollection = mongoDatabase.GetCollection<Product>(collectionName);
        }

        // 1. Get ALL products for the dashboard
        public async Task<List<Product>> GetAllProductsAsync()
        {
            if (Products == null || Products.Count == 0)
            {
                Products = await _productsCollection.Find(_ => true).ToListAsync();
            }

            return Products;
        }

        // 2. Search function (e.g., search by name)
        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            // This does a case-insensitive search anywhere in the product name
            var filter = Builders<Product>.Filter.Regex("Name", new BsonRegularExpression(searchTerm, "i"));
            return await _productsCollection.Find(filter).ToListAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            await _productsCollection.InsertOneAsync(product);
        }

        public async Task UpdateProductAsync(string id, Product updatedProduct)
        {
            await _productsCollection.ReplaceOneAsync(
                p => p.Id == id,
                updatedProduct);
        }

        public async Task DeleteProductAsync(string id)
        {
            await _productsCollection.DeleteOneAsync(
                p => p.Id == id);
        }
    }
}