using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SQLite;
using webapi.Data.Exceptions;
using webapi.Data.Interfaces;

namespace webapi.Data
{
    public class SqlLiteContext : ISqlLiteContext, IDisposable
    {
        private readonly SQLiteAsyncConnection sqlConnection;

        public SqlLiteContext(string dbPath, string encriptionKey = "")
        {
            if (!string.IsNullOrEmpty(encriptionKey))
            {
                var options = new SQLiteConnectionString(dbPath, true, key: encriptionKey);
                sqlConnection = new SQLiteAsyncConnection(options);
            }
            else
            {
                sqlConnection = new SQLiteAsyncConnection(dbPath);
            }
        }

        /// <summary>
        ///     Validates if a given entity of type <typeparamref name="T"/> exists in SQLite database.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="connection"></param>
        /// <returns>bool value indicating if table exists</returns>
        /// <exception cref="SqLiteServiceException">SQLite service exception</exception>
        private async Task<bool> TableExistsAsync<T>(SQLiteAsyncConnection connection)
        {
            string query = "SELECT name FROM sqlite_master WHERE type='table' AND name=?";
            string commandResult = null;
            try
            {
                commandResult = await connection.ExecuteScalarAsync<string>(query, typeof(T).Name);
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return commandResult != null;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync<T>(T entity) where T : class, new()
        {
            var result = false;

            try
            {
                var repository = new SqliteUnitOfWork<T>(sqlConnection);
                if (await TableExistsAsync<T>(sqlConnection))
                {
                    var deleted = await repository.DeleteAsync(entity);
                    result = (deleted == 1);
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync<T>(Func<T, bool> predicate) where T : class, new()
        {
            var result = false;

            try
            {
                var repository = new SqliteUnitOfWork<T>(sqlConnection);

                if (await TableExistsAsync<T>(sqlConnection))
                {
                    IEnumerable<T> allEntities = await repository.GetAsync();
                    if (allEntities.Any())
                    {
                        allEntities = allEntities.Where(predicate);
                        if (!allEntities.Any())
                        {
                            result = false;
                        }

                        var deletedRecords = 0;

                        foreach (var entity in allEntities)
                        {
                            var deleted = await repository.DeleteAsync(entity);
                            if (deleted == 1)
                            {
                                deletedRecords++;
                            }
                        }

                        result = (deletedRecords == allEntities.Count());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<List<T>> GetAllAsync<T>() where T : class, new()
        {
            var result = new List<T>();

            try
            {
                var repository = new SqliteUnitOfWork<T>(sqlConnection);

                if (await TableExistsAsync<T>(sqlConnection))
                {
                    result = await repository.GetAsync();
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<List<T>> GetAsync<T>(Func<T, bool> predicate) where T : class, new()
        {
            var result = new List<T>();

            try
            {
                var repository = new SqliteUnitOfWork<T>(sqlConnection);

                if (await TableExistsAsync<T>(sqlConnection))
                {
                    result = await repository.GetAsync();
                    if (result.Any())
                    {
                        result = result.Where(predicate).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(object id) where T : class, new()
        {
            T result = null;

            try
            {
                var repository = new SqliteUnitOfWork<T>(sqlConnection);

                if (await TableExistsAsync<T>(sqlConnection))
                {
                    result = await repository.GetAsync(id);
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync<T>(T entity, object id) where T : class, new()
        {
            var result = false;

            try
            {
                var repository = new SqliteUnitOfWork<T>(sqlConnection);

                if (!await TableExistsAsync<T>(sqlConnection))
                {
                    result = false;
                }

                var storedRecord = await repository.GetAsync(id);
                if (storedRecord != null)
                {
                    var updatedRecord = await repository.UpdateAsync(entity);
                    result = (updatedRecord == 1);
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> InsertAsync<T>(T entity) where T : class, new()
        {
            var result = false;

            try
            {
                var repository = new SqliteUnitOfWork<T>(sqlConnection);

                if (!await TableExistsAsync<T>(sqlConnection))
                {
                    await sqlConnection.CreateTableAsync<T>();
                    var insertedRecord = await repository.InsertAsync(entity);
                    result = (insertedRecord == 1);
                }
                else
                {
                    var insertedRecord = await repository.InsertAsync(entity);
                    result = (insertedRecord == 1);
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> InsertAsync<T>(IEnumerable<T> entities) where T : class, new()
        {
            if (entities == null || !entities.Any())
            {
                return false;
            }

            var result = false;
            var entity = new T();
            try
            {
                var inertedEntities = 0;
                for (int i = 0; i < entities.Count(); i++)
                {
                    entity = entities.ElementAt(i) as T;
                    var repository = new SqliteUnitOfWork<T>(sqlConnection);
                    if (!await TableExistsAsync<T>(sqlConnection))
                    {
                        await sqlConnection.CreateTableAsync<T>();
                        var insertedRecord = await repository.InsertAsync(entity);
                        result = (insertedRecord == 1);
                    }
                    else
                    {
                        var insertedRecord = await repository.InsertAsync(entity);
                        if (insertedRecord == 1)
                        {
                            inertedEntities++;
                        }
                    }
                }

                result = (inertedEntities == entities.Count());
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteTableAsync<T>()
        {
            var result = false;

            try
            {
                if (await TableExistsAsync<T>(sqlConnection))
                {
                    string query = $"DROP TABLE {typeof(T).Name}";
                    await sqlConnection.ExecuteScalarAsync<string>(query);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new SqLiteServiceException(ex);
            }

            return result;
        }

        public async void Dispose()
        {
            await sqlConnection?.CloseAsync();
        }
    }
}
