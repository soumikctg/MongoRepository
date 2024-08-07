using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MongoRepository.Repository
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly IMongoClient Client;
        public DatabaseContext(IMongoClient client)
        {
            Client = client;
        }

        private IMongoDatabase GetDatabase(string databaseName)
        {
            return Client.GetDatabase(databaseName);
        }

        private IMongoCollection<T> GetCollection<T>(string databaseName) where T : class
        {
            var collection = GetDatabase(databaseName).GetCollection<T>(typeof(T).Name.ToLower());
            return collection;
        }


        public async Task DeleteItemByIdAsync<T>(string databaseName, string id) where T : class
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            var collection = GetCollection<T>(databaseName);
            await collection.DeleteOneAsync(filter);
        }

        public async Task DeleteItemsAsync<T>(string databaseName) where T : class
        {
            var collection = GetCollection<T>(databaseName);
            await collection.DeleteManyAsync(new BsonDocument());
        }

        public async Task<T> GetItemByIdAsync<T>(string databaseName, string id) where T : class
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId());
            var collection = GetCollection<T>(databaseName);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetItemsAsync<T>(string databaseName) where T : class
        {
            var collection = GetCollection<T>(databaseName);
            return await collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<List<T>> GetItemsByConditionAsync<T>(string databaseName, Expression<Func<T, bool>> predicate) where T : class
        {
            var collection = GetCollection<T>(databaseName);
            return await collection.Find(predicate).ToListAsync();
        }

        public async Task SaveAsync<T>(string databaseName, T item) where T : class
        {
            var collection = GetCollection<T>(databaseName);
            await collection.InsertOneAsync(item);
        }

        public async Task SaveManyAsync<T>(string databaseName, List<T> items) where T : class
        {
            var collection = GetCollection<T>(databaseName);
            await collection.InsertManyAsync(items);
        }

        public async Task UpdateItemByIdAsync<T>(string databaseName, string id, T item) where T : class
        {
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            var collection = GetCollection<T>(databaseName);
            await collection.ReplaceOneAsync(filter, item);
        }

        public async Task<List<Dictionary<string, object>>> GetFieldsOfItemsByAsync<T>(string databaseName, List<string> fields) where T : class
        {
            var projection = Builders<T>.Projection.Include(fields.FirstOrDefault()).Exclude("Id");
            foreach (var field in fields.Skip(1))
            {
                projection = projection.Include(field);
            }

            var collection = GetCollection<T>(databaseName);
            var items = await collection.Find(new BsonDocument()).Project<BsonDocument>(projection).ToListAsync();

            var result = items.Select(i => i.ToDictionary()).ToList();
            return result;
        }

        public async Task<List<TDest>> GetProjectedItemsAsync<TSource, TDest>(string databaseName, ProjectionDefinition<TSource, TDest> projectionDef)
            where TSource : class
            where TDest : class
        {
            var collection = GetCollection<TSource>(databaseName);
            var result = await collection.Find(new BsonDocument()).Project(projectionDef).ToListAsync();
            return result;
        }

        // public async Task DeleteItemByIdAsync(string id)
        // {     
        //     var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        //     await mongoCollection.DeleteOneAsync(filter);
        // }
        //
        // public async Task DeleteItemsAsync()
        // {
        //     await mongoCollection.DeleteManyAsync(new BsonDocument());
        // }
        //
        // public async Task<T> GetItemByIdAsync(string id)
        // {
        //     var filter = Builders<T>.Filter.Eq("_id", new ObjectId());
        //     return await mongoCollection.Find(filter).FirstOrDefaultAsync();
        // }
        //
        // public async Task<List<T>> GetItemsAsync()
        // {
        //     return await mongoCollection.Find(new BsonDocument()).ToListAsync();
        // }
        //
        // public async Task<List<T>> GetItemsByConditionAsync(Expression<Func<T, bool>> predicate)
        // {
        //     return await mongoCollection.Find(predicate).ToListAsync();
        //
        // }
        //
        // public async Task SaveManyAsync(List<T> items)
        // {
        //     await mongoCollection.InsertManyAsync(items);
        // }
        //
        // public async Task SaveAsync(T item)
        // {
        //     await mongoCollection.InsertOneAsync(item);
        // }
        //
        // public async Task UpdateItemByIdAsync(string id, T item)
        // {
        //     var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
        //     await mongoCollection.ReplaceOneAsync(filter, item);
        // }
    }
}
