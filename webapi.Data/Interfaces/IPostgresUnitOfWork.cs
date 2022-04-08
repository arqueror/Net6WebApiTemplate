using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace webapi.Data.Interfaces
{
    public interface IPostgresUnitOfWork
    {
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
        EntityEntry Entry<T>(T o) where T : class;
        void Dispose();
        bool EnsureCreated();
    }
}
