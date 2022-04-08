using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using webapi.Data.Interfaces;

namespace webapi.Data
{
    public class SqliteUnitOfWork<T> :IDisposable, ISqliteRepository<T> where T : class, new()
    {
        /// <summary>
        ///     SQLite connection.
        /// </summary>
        private SQLiteAsyncConnection sqlConnection;

        public SqliteUnitOfWork(SQLiteAsyncConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        /// <inheritdoc/>
        public AsyncTableQuery<T> AsQueryable() =>
            sqlConnection.Table<T>();

        /// <inheritdoc/>
        public async Task<List<T>> GetAsync() =>
            await sqlConnection.Table<T>().ToListAsync();

        /// <inheritdoc/>
        public async Task<List<T>> GetAsync<TValue>(Expression<Func<T, bool>> predicate = null, Expression<Func<T, TValue>> orderBy = null)
        {
            var query = sqlConnection.Table<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync(object id) =>
            await sqlConnection.FindAsync<T>(id);

        /// <inheritdoc/>
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate) =>
            await sqlConnection.FindAsync(predicate);

        /// <inheritdoc/>
        public async Task<int> InsertAsync(T entity) =>
            await sqlConnection.InsertAsync(entity);

        /// <inheritdoc/>
        public async Task<int> UpdateAsync(T entity) =>
            await sqlConnection.UpdateAsync(entity);

        /// <inheritdoc/>
        public async Task<int> DeleteAsync(T entity) =>
            await sqlConnection.DeleteAsync(entity);

        public async void Dispose()
        {
            await sqlConnection.CloseAsync();
        }
    }
}
