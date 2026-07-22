using Microsoft.AspNetCore.Components;
using MongoDB.Bson;
using MongoDB.Driver;
using Project_Backend.Models;



namespace Project_Backend.Services
{

    public class OrderService
    {
        private readonly ProductService _productService;
        private readonly IMongoCollection<Order> _ordersCollection;
        private string log;
        public OrderService(IConfiguration config, ProductService productService)
        {
            _productService = productService;
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
        public async Task CreateOrderAsync(Order order)
        {
            await _ordersCollection.InsertOneAsync(order);
        }

        public async Task UpdateOrderAsync(string id, Order updatedOrder)
        {
            await _ordersCollection.ReplaceOneAsync(
                o => o.Id == id,
                updatedOrder);
        }

        public async Task DeleteOrderAsync(string id)
        {
            await _ordersCollection.DeleteOneAsync(
                o => o.Id == id);
        }
        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            return await _ordersCollection.Find(o => o.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _ordersCollection.Find(o => o.UserId == userId).ToListAsync();
        } 
        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _ordersCollection.Find(o => o.OrderStatus == status).ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByDateRangeAsync(string startDate, string endDate)
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Gte(o => o.OrderDate, startDate),
                Builders<Order>.Filter.Lte(o => o.OrderDate, endDate)
            );
            return await _ordersCollection.Find(filter).ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByTotalValueRangeAsync(double minValue, double maxValue)
        {
            var filter = Builders<Order>.Filter.And(
                Builders<Order>.Filter.Gte(o => o.TotalOrderValue, minValue),
                Builders<Order>.Filter.Lte(o => o.TotalOrderValue, maxValue)
            );
            return await _ordersCollection.Find(filter).ToListAsync();
        } 
        public async Task<List<Order>> GetOrdersByProductIdAsync(string productId)
        {
            var filter = Builders<Order>.Filter.ElemMatch(o => o.Items, item => item.ProductId == productId);
            return await _ordersCollection.Find(filter).ToListAsync();
        } 
        public async Task<List<Order>> GetOrdersByProductNameAsync(string productName)
        {
            var filter = Builders<Order>.Filter.ElemMatch(o => o.Items, item => item.ProductName == productName);
            return await _ordersCollection.Find(filter).ToListAsync();
        } 
        public async Task<List<Order>> GetOrdersByProductPriceRangeAsync(double minPrice, double maxPrice)
        {
            var filter = Builders<Order>.Filter.ElemMatch(o => o.Items, item => item.PriceAtPurchase >= minPrice && item.PriceAtPurchase <= maxPrice);
            return await _ordersCollection.Find(filter).ToListAsync();
        }
        public async Task<List<Order>> GetOrdersByProductDiscountRangeAsync(double minDiscount, double maxDiscount)
        {
            var filter = Builders<Order>.Filter.ElemMatch(o => o.Items, item => item.DiscountPercentage >= minDiscount && item.DiscountPercentage <= maxDiscount);
            return await _ordersCollection.Find(filter).ToListAsync();
        }
        public async Task<bool> SaveOrderAsync(SaveOrderRequest order)
        {
            log = "public async Task<string> SaveOrderAsync(SaveOrderRequest order)\n";
            bool allItemsInStock = true;
            var newOrder = new Order
            {
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                Items = order.Items,
                TotalOrderValue = order.TotalOrderValue
            };
            foreach (var item in newOrder.Items)
            {
                if (item.ProductId == null) 
                { 
                    continue; 
                }
                if (!await _productService.SubtractQuantityAsync(item.ProductId, item.Quantity))
                {
                    allItemsInStock = false;
                }
            }

            await _ordersCollection.InsertOneAsync(newOrder);
            return allItemsInStock;
        }


    }
}