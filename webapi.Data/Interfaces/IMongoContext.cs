
using MongoDB.Driver;

namespace webapi.Data.Interfaces
{
    public interface IMongoContext: IDisposable
    {
        MongoClient MongoClient { get; set; }
        IClientSessionHandle Session { get; set; }

        void AddCommand(Func<Task> func);
        IMongoCollection<T> GetCollection<T>(string name);
        Task<int> SaveChanges();
    }
}