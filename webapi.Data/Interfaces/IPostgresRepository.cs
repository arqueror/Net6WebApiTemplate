using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace webapi.Data.Interfaces
{
    public interface IPostgresRepository<TAggregateRoot> where TAggregateRoot : class
    {
        IPostgresUnitOfWork Context { get; set; }
        DbSet<TAggregateRoot> Entities { get; set; }
        IQueryable<TAggregateRoot> All();
        IQueryable<TAggregateRoot> AllReadOnly();
        bool Any(Expression<Func<TAggregateRoot, bool>> predicate);
        bool AnyReadOnly(Expression<Func<TAggregateRoot, bool>> predicate);
        IQueryable<TAggregateRoot> AllIncluding(params Expression<Func<TAggregateRoot, object>>[] includeProperties);
        bool AnyIncluding(Expression<Func<TAggregateRoot, bool>> predicate, params Expression<Func<TAggregateRoot, object>>[] includeProperties);
        IQueryable<TAggregateRoot> GetMany(Expression<Func<TAggregateRoot, bool>> where);
        TAggregateRoot Get(Expression<Func<TAggregateRoot, bool>> where);

        IQueryable<TAggregateRoot> Get(
            Expression<Func<TAggregateRoot, bool>> filter = null,
            Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>> orderBy = null,
            string includeProperties = "");

        TAggregateRoot GetById(int id);
        void Add(TAggregateRoot entity);
        void Update(TAggregateRoot entity);
        void Delete(TAggregateRoot entity);
        void Delete(int id);
    }
}

