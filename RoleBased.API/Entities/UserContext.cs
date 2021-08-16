using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RoleBased.API.Models;

namespace RoleBased.API.Entities
{
    public class UserContext
    {
        private readonly IMongoDatabase _database = null;

        public UserContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return _database.GetCollection<User>("User");
            }
        }

        public IMongoCollection<Request> Requests
        {
            get
            {
                return _database.GetCollection<Request>("Request");
            }
        }
    }
}
