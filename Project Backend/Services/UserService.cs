using MongoDB.Bson;
using MongoDB.Driver;
using Project_Backend.Models;

namespace Project_Backend.Services
{
    public class UserService
    {
        public List<User>? Users = new List<User>();
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IConfiguration config)
        {
            // Read the settings from appsettings.json
            var connectionString = config["MongoDbSettings:ConnectionString"];
            var databaseName = config["MongoDbSettings:DatabaseName"];
            var collectionName = config["MongoDbSettings:UsersCollectionName"];

            // Connect to MongoDB
            var mongoClient = new MongoClient(connectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseName);
            _usersCollection = mongoDatabase.GetCollection<User>(collectionName);
        }

        // 1. Get ALL users for the dashboard
        public async Task<List<User>> GetAllUsersAsync()
        {
            if (Users == null || Users.Count == 0)
            {
                Users = await _usersCollection.Find(_ => true).ToListAsync();
            }

            return Users;
        }
        public async Task<List<User>> GetAllUsersForceReloadAsync()
        {
           
            Users = await _usersCollection.Find(_ => true).ToListAsync();

            return Users;
        }

        // 2. Search function (e.g., search by name)
        public async Task<List<User>> SearchUsersAsync(string searchTerm)
        {
            // This does a case-insensitive search anywhere in the user name
            var filter = Builders<User>.Filter.Regex("Name", new BsonRegularExpression(searchTerm, "i"));
            return await _usersCollection.Find(filter).ToListAsync();
        }
        public async Task CreateUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task UpdateUserAsync(string id, User updatedUser)
        {
            await _usersCollection.ReplaceOneAsync(
                u => u.Id == id,
                updatedUser);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(
                u => u.Id == id);
        }
    }
}