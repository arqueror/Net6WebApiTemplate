using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Data.Interfaces;
using webapi.Data.Repository;

namespace webapi.Data
{
    public static class MongoFactories
    {
        public static IMongoRepository<TRepository> CreateRepository<TContext,TRepository>(TContext context)
              where TContext : class, IMongoContext
              where TRepository : class
        {
            //Exceptions thrown by this fabric will be of type Exception always because of reflection
            Type repositoryType = typeof(MongoRepository<TRepository>);
            var mongoRepository = (IMongoRepository<TRepository>)Activator.CreateInstance(repositoryType, context);

            return mongoRepository;

        }

        public static TContext CreateContext<TContext>(IConfiguration config)
       where TContext : class, IMongoUnitOfWork
        {
            //Exceptions thrown by this fabric will be of type Exception always because of reflection
            Type dataContextType = typeof(TContext);
            var mongoContext = (TContext)Activator.CreateInstance(dataContextType, config);

            return mongoContext;

        }

        public static IMongoRepository<TRepository> CreateRepository<TRepository>(this IMongoContext context)
               where TRepository : class
        {
            //Exceptions thrown by this fabric will be of type Exception always because of reflection
            Type repositoryType = typeof(MongoRepository<TRepository>);
            var mongoRepository = (IMongoRepository<TRepository>)Activator.CreateInstance(repositoryType, context);

            return mongoRepository;

        }
    }
}
