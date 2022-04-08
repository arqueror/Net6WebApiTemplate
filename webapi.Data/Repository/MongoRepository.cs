using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.Data.Interfaces;

namespace webapi.Data.Repository
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext _context;
        protected IMongoCollection<TEntity> _dbSet;

        public MongoRepository(IMongoContext context)
        {
            try
            {
                _context = context;
                _dbSet = _context.GetCollection<TEntity>(typeof(TEntity).Name);
            }
            catch(Exception ex)
            {
                throw new MongoException($"Exception thrown when reading {typeof(TEntity)} in {nameof(MongoRepository<TEntity>)}.Ctor()", ex);
            }
        }

        public virtual void Add(TEntity obj)
        {
            _context.AddCommand(() => _dbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            var data = await _dbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await _dbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj, Guid id)
        {
            _context.AddCommand(() => _dbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", id), obj));
        }

        public virtual void Remove(Guid id)
        {
            _context.AddCommand(() => _dbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id)));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
