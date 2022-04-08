using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace webapi.Data.Interfaces
{
    public interface ISqliteRepository<T> where T : class, new()
    {
        /// <summary>
        ///    Retrieves all found entities of type <typeparamref name="T"/>.
        /// </summary>
        Task<List<T>> GetAsync();

        /// <summary>
        ///     Retrieves an entity based on the entity PK value.
        /// </summary>
        Task<T> GetAsync(object id);

        /// <summary>
        ///     Retrieves all entities that matches provided predicate and then orders them using provided expression.
        /// </summary>
        Task<List<T>> GetAsync<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null);

        /// <summary>
        ///      Retrieves all entities that matches provided predicate.
        /// </summary>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        ///     Returns a queryable interface to the table of type <typeparamref name="T"/>.
        /// </summary>
        AsyncTableQuery<T> AsQueryable();

        /// <summary>
        ///     Inserts an entity of type <typeparamref name="T"/> to SQLite database.
        /// </summary>
        Task<int> InsertAsync(T entity);

        /// <summary>
        ///     Updates an entity of type <typeparamref name="T"/>.
        /// </summary>
        Task<int> UpdateAsync(T entity);

        /// <summary>
        ///     Deletes an entity of type <typeparamref name="T"/> from SQLite database.
        /// </summary>
        Task<int> DeleteAsync(T entity);
    }
}
