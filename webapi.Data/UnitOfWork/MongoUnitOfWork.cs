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
    //public class MongoUnitOfWork : IMongoUnitOfWork
    //{
    //    private readonly IMongoContext _context;

    //    public MongoUnitOfWork(IMongoContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<bool> Commit()
    //    {
    //        return (await _context.SaveChanges()) > 0;
    //    }

    //    public void Dispose()
    //    {
    //        _context.Dispose();
    //    }
    //}
}
