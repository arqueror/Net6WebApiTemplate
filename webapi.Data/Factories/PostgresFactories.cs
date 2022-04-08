using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Data.Interfaces;

namespace webapi.Data.Factories
{
    public static class PostgresFactories
    {

        public static TContext Create<TContext>(string connectionString)
            where TContext : class, IPostgresUnitOfWork
        {
            var optionsBuilder = new DbContextOptionsBuilder<PostgresContext>();
            optionsBuilder.UseNpgsql(connectionString);  //If MSSQL needed, just change it for Use UseSqlServer()
            Type dataContextType = typeof(TContext);
            var context = (TContext)Activator.CreateInstance(dataContextType, optionsBuilder.Options);
            context.EnsureCreated();
            return context;
        }
    }
}
