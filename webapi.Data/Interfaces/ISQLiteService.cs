using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Data.Interfaces
{
    public interface ISqlLiteContext
    {
        /// <summary>
        ///     Deletes an entity of type <typeparamref name="T"/> from SQLite database.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity to delete</param>
        /// <returns>bool value indicating if delete was successful</returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<bool> DeleteAsync<T>(T entity) where T : class, new();

        /// <summary>
        ///     Deletes all entities of type <typeparamref name="T"/> from SQLite database that matches provided predicate.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="predicate">Predicate to evaluate</param>
        /// <returns>bool value indicating if delete was successful</returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<bool> DeleteAsync<T>(Func<T, bool> predicate) where T : class, new();

        /// <summary>
        ///     Gets all entities of type <typeparamref name="T"/> from SQLite database.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>List of type <typeparamref name="T"/> containing the found entities </returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<List<T>> GetAllAsync<T>() where T : class, new();

        /// <summary>
        ///      Gets a list of entities of type <typeparamref name="T"/> from SQLite database that matches provided predicate.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="predicate">Predicate to evaluate</param>
        /// <returns>List of type <typeparamref name="T"/> containing the found entities that matches provided predicate</returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<List<T>> GetAsync<T>(Func<T, bool> predicate) where T : class, new();

        /// <summary>
        ///      Retrieves an entity using provided id <typeparamref name="T"/> from SQLite database. Use of this method requires
        ///      that the given type have a Primary Key designated that matches provided <paramref name="id"/> value.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="id">Entity PK value</param>
        /// <returns>Entity of type <typeparamref name="T"/></returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<T> GetAsync<T>(object id) where T : class, new();

        /// <summary>
        ///      Updates an entity of type <typeparamref name="T"/> from SQLite database. Use of this method requires
        ///      that the given type have a Primary Key designated that matches provided <paramref name="id"/> value.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity to update</param>
        /// <param name="id">Entity PK value</param>
        /// <returns>bool value indicating if entity was updated successfully</returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<bool> UpdateAsync<T>(T entity, object id) where T : class, new();

        /// <summary>
        ///   Inserts a new entity of type <typeparamref name="T"/> to SQLite database.
        /// </summary>
        /// <typeparam name="T">bool value indicating if entity was updated successfully</typeparam>
        /// <param name="entity">Entity to insert</param>
        /// <returns>int value</returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<bool> InsertAsync<T>(T entity) where T : class, new();

        /// <summary>
        ///   Inserts a list of entities of type <typeparamref name="T"/> to SQLite database.
        /// </summary>
        /// <typeparam name="T">bool value indicating if entity was updated successfully</typeparam>
        /// <param name="entities">Entities to insert</param>
        /// <returns>int value</returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<bool> InsertAsync<T>(IEnumerable<T> entities) where T : class, new();

        /// <summary>
        ///      Deletes a table of type <typeparamref name="T"/> if exists in SQLite database.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>bool value indicating if delete was successful</returns>
        /// <exception cref="webapi.Data.Exceptions.SqLiteServiceException">SQLite service exception</exception>
        Task<bool> DeleteTableAsync<T>();
    }
}
