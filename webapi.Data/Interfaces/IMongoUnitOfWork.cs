using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Data.Interfaces
{
    public interface IMongoUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
