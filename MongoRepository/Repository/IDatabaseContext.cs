using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoRepository.Repository
{
    public interface IDatabaseContext
    {
        Task SaveAsync<T>(string databaseName, T item) where T : class;
        Task SaveManyAsync<T>(string databaseName, List<T> items) where T : class;
        Task<List<T>> GetItemsAsync<T>(string databaseName) where T : class;
        Task<T> GetItemByIdAsync<T>(string databaseName, string id) where T : class;
        Task<List<T>> GetItemsByConditionAsync<T>(string databaseName, Expression<Func<T, bool>> predicate) where T : class;
        Task UpdateItemByIdAsync<T>(string databaseName, string id, T item) where T : class;
        Task DeleteItemByIdAsync<T>(string databaseName, string id) where T : class;
        Task DeleteItemsAsync<T>(string databaseName) where T : class;
        Task<List<Dictionary<string, object>>> GetFieldsOfItemsByAsync<T>(string databaseName, List<string> fields) where T : class;
        Task<List<TDest>> GetProjectedItemsAsync<TSource, TDest>(string databaseName, ProjectionDefinition<TSource, TDest> projectionDef)
            where TSource : class
            where TDest : class;
        Task ExecuteTransactionAsync(Func<IClientSessionHandle, Task> transactionalOperations);
    }
}
