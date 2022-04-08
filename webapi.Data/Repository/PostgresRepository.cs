using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using webapi.Data.Interfaces;

namespace webapi.Data.Repository
{
    public class PostgresRepository<TAggregateRoot> : IPostgresRepository<TAggregateRoot> where TAggregateRoot : class
    {
        #region Properties

        public IPostgresUnitOfWork Context { get; set; }

        public DbSet<TAggregateRoot> Entities { get; set; }
        #endregion

        #region Constructor

        public PostgresRepository(IPostgresUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("context");

            Context = unitOfWork;
            Entities = unitOfWork.Set<TAggregateRoot>();
        }

        #endregion

        #region Methods
        public IQueryable<TAggregateRoot> Set
        {
            get { return Entities.AsNoTracking(); }
        }

        public IQueryable<TAggregateRoot> SetTrackeable
        {
            get { return Entities.AsNoTracking(); }
        }
        public IQueryable<TAggregateRoot> All()
        {
            return Entities;
        }

        public IQueryable<TAggregateRoot> AllReadOnly()
        {
            return Entities.AsNoTracking();
        }

        public bool Any(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Entities.Any(predicate);
        }

        public bool AnyReadOnly(Expression<Func<TAggregateRoot, bool>> predicate)
        {
            return Entities.AsNoTracking().Any(predicate);
        }


        public IQueryable<TAggregateRoot> AllIncluding(params Expression<Func<TAggregateRoot, object>>[] includeProperties)
        {
            IQueryable<TAggregateRoot> result = Entities;
            foreach (Expression<Func<TAggregateRoot, object>> property in includeProperties)
            {
                result = result.Include(property);
            }
            return result;
        }

        public bool AnyIncluding(Expression<Func<TAggregateRoot, bool>> predicate, params Expression<Func<TAggregateRoot, object>>[] includeProperties)
        {
            IQueryable<TAggregateRoot> result = Entities;
            foreach (Expression<Func<TAggregateRoot, object>> property in includeProperties)
            {
                result = result.Include(property);
            }
            return result.Any(predicate);
        }

        public IQueryable<TAggregateRoot> GetMany(Expression<Func<TAggregateRoot, bool>> where)
        {
            return Entities.Where(where);
        }

        public TAggregateRoot Get(Expression<Func<TAggregateRoot, bool>> where)
        {
            return Entities.Where(where).FirstOrDefault();
        }


        public virtual IQueryable<TAggregateRoot> Get(
            Expression<Func<TAggregateRoot, bool>> filter = null,
            Func<IQueryable<TAggregateRoot>, IOrderedQueryable<TAggregateRoot>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TAggregateRoot> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }


        public TAggregateRoot GetById(int id)
        {
            return Entities.Find(id);
        }

        public void Add(TAggregateRoot entity)
        {
            EntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                Entities.Add(entity);
            }
        }



        /// <summary>
        /// Updates all Entitites properties
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TAggregateRoot entity)
        {
            EntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Entities.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public void Delete(TAggregateRoot entity)
        {
            EntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                Entities.Attach(entity);
                Entities.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        #endregion
    }
}
