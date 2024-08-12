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
            try
            {
                return Client.GetDatabase(databaseName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private IMongoCollection<T> GetCollection<T>(string databaseName) where T : class
        {
            try
            {
                var collection = GetDatabase(databaseName).GetCollection<T>(typeof(T).Name.ToLower());
                return collection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SaveAsync<T>(string databaseName, T item) where T : class
        {
            try
            {
                var collection = GetCollection<T>(databaseName);
                await collection.InsertOneAsync(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task SaveManyAsync<T>(string databaseName, List<T> items) where T : class
        {
            try
            {
                var collection = GetCollection<T>(databaseName);
                await collection.InsertManyAsync(items);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<T>> GetItemsAsync<T>(string databaseName) where T : class
        {
            try
            {
                var collection = GetCollection<T>(databaseName);
                return await collection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<T> GetItemByIdAsync<T>(string databaseName, string id) where T : class
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", new ObjectId());
                var collection = GetCollection<T>(databaseName);
                return await collection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<T>> GetItemsByConditionAsync<T>(string databaseName, Expression<Func<T, bool>> predicate) where T : class
        {
            try
            {
                var collection = GetCollection<T>(databaseName);
                return await collection.Find(predicate).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteItemByIdAsync<T>(string databaseName, string id) where T : class
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                var collection = GetCollection<T>(databaseName);
                await collection.DeleteOneAsync(filter);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task DeleteItemsAsync<T>(string databaseName) where T : class
        {
            try
            {
                var collection = GetCollection<T>(databaseName);
                await collection.DeleteManyAsync(new BsonDocument());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateItemByIdAsync<T>(string databaseName, string id, T item) where T : class
        {
            try
            {
                var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
                var collection = GetCollection<T>(databaseName);
                await collection.ReplaceOneAsync(filter, item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetFieldsOfItemsByAsync<T>(string databaseName, List<string> fields) where T : class
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<TDest>> GetProjectedItemsAsync<TSource, TDest>(string databaseName, ProjectionDefinition<TSource, TDest> projectionDef)
            where TSource : class
            where TDest : class
        {
            try
            {
                var collection = GetCollection<TSource>(databaseName);
                var result = await collection.Find(new BsonDocument()).Project(projectionDef).ToListAsync();
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task ExecuteTransactionAsync(Func<IClientSessionHandle, Task> transactionalOperations)
        {
            using (var session = await Client.StartSessionAsync())
            {
                session.StartTransaction();

                try
                {
                    await transactionalOperations(session);
                    await session.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    await session.AbortTransactionAsync();
                    throw;
                }
            }
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
