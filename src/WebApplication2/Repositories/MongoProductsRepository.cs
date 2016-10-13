using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class MongoProductsRepository : IProductsRepository
    {
        static MongoClient _client = new MongoClient("mongodb://localhost:27017");
        static IMongoDatabase _database = _client.GetDatabase("WebApp");
        IMongoCollection<BsonDocument> _collection = _database.GetCollection<BsonDocument>("Products");

        public async Task<Product> AddAsync(Product product)
        {
            ObjectId objectId = ObjectId.GenerateNewId();
            product.Id = objectId.ToString();
            var document = new BsonDocument
            {
                { "id", objectId },
                { "name", product.Name },
                { "price", product.Price }
            };
            await _collection.InsertOneAsync(document);

            return product;
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("id", new ObjectId(id));
            var document = await _collection.DeleteOneAsync(filter);
        }

        public async Task<ICollection<Product>> GetAllAsync()
        {
            var list = await _collection.Find(new BsonDocument()).ToListAsync();
            ICollection<Product> products = new List<Product>();
            foreach (var item in list)
            {
                Product product = new Product
                {
                    Id = item.GetValue("id").AsObjectId.ToString(),
                    Name = item.GetValue("name").AsString,
                    Price = item.GetValue("price").AsDouble
                };
                products.Add(product);
            }
            return products;
        }

        public async Task<Product> GetAsync(string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("id", new ObjectId(id));
            var document = await _collection.Find(filter).FirstAsync();

            return new Product
            {
                Id = document.GetValue("id").AsObjectId.ToString(),
                Name = document.GetValue("name").IsBsonNull ? null : document.GetValue("name").AsString,
                Price = document.GetValue("price").AsDouble
            };
        }

        public async Task PatchAsync(string id, IDictionary<string, object> patch)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("id", new ObjectId(id));
            var updateBuilder = Builders<BsonDocument>.Update;
            List<UpdateDefinition<BsonDocument>> updates = new List<UpdateDefinition<BsonDocument>>();
            foreach (var item in patch)
            {
                updates.Add(updateBuilder.Set(item.Key, item.Value));
            }
            var update = updateBuilder.Combine(updates);
            await _collection.UpdateOneAsync(filter, update);
        }

        public async Task UpdateAsync(Product product)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("id", new ObjectId(product.Id));
            var updateBuilder = Builders<BsonDocument>.Update;
            List<UpdateDefinition<BsonDocument>> updates = new List<UpdateDefinition<BsonDocument>>();
            updates.Add(updateBuilder.Set("name", product.Name));
            updates.Add(updateBuilder.Set("price", product.Price));
            var update = updateBuilder.Combine(updates);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
