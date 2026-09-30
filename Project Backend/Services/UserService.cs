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
        public async Task<string> GetUserName(string id)
        {
            if (Users == null || Users.Count < 1)
            {
                await GetAllUsersAsync();
                if( Users == null || Users.Count < 1) { return ""; }
            }
            foreach (var user in Users)
            {
                if (user.Id == id) { return user.DisplayName; }
            }
            return "";
        }
        public async Task<AuthResponse> AuthenticateAsync(string email, string password)
        {
            bool isValidPassword = false;
            // 1. Fetch user by email
            var user = await _usersCollection
                .Find(u => u.Email.ToLower() == email.ToLower())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new AuthResponse { Success = false, Message = "OPERATIVE NOT FOUND IN MAINFRAME" };
            }
            if (user.Password == password)
            {
                isValidPassword = true;

                // Hash the password and upgrade MongoDB record in-place
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
                var update = Builders<User>.Update.Set(u => u.Password, hashedPassword);

                await _usersCollection.UpdateOneAsync(filter, update);
            }
            else
            {
                // 2. Standard BCrypt verification for already-upgraded hashes
                try
                {
                    isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    // Stored password wasn't a valid BCrypt hash and didn't match plain text
                    isValidPassword = false;
                }
            }

            if (!isValidPassword)
            {
                return new AuthResponse { Success = false, Message = "ACCESS DENIED: INVALID CREDENTIALS" };
            }

            return new AuthResponse
            {
                Success = true,
                Message = "AUTHENTICATION SUCCESSFUL",
                UserName = user.DisplayName,
                AvatarUrl = user.UserImage
            };
        }
    }
}