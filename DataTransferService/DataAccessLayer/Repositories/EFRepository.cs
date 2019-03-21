using DataTransferService.DataAccessLayer.Models;
using DataTransferService.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace DataTransferService.DataAccessLayer.Repositories
{
    internal class EFRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected IList<Expression<Func<TEntity, object>>> _navProps;

        public EFRepository()
        {
            _navProps = new List<Expression<Func<TEntity, object>>>();
        }
        public IList<TEntity> GetAll()
        {
            return GetAll(_navProps);
        }
        public virtual IList<TEntity> GetAll(IEnumerable<Expression<Func<TEntity, object>>> navigationProperties)
        {
            IList<TEntity> list;

            using(var context = new PolitermUsers())
            {
                IQueryable<TEntity> dbQuery = context.Set<TEntity>();

                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include(navigationProperty);
                }

                list = dbQuery.AsNoTracking().ToList();
            }

            return list;
        }
        public virtual TEntity GetItem(Func<TEntity, bool> where)
        {
            return GetItem(where, _navProps);
        }
        public virtual TEntity GetItem(Func<TEntity, bool> where, IEnumerable<Expression<Func<TEntity, object>>> navigationProperties)
        {
            TEntity entity = null;
            
            using(var context = new PolitermUsers())
            {
                IQueryable<TEntity> dbQuery = context.Set<TEntity>();

                foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include(navigationProperty);
                }

                entity = dbQuery.AsNoTracking().FirstOrDefault(where);
            }

            return entity;
        }
        public virtual IList<TEntity> Create(IEnumerable<TEntity> items)
        {
            using (var context = new PolitermUsers())
            {
                DbSet<TEntity> set = context.Set<TEntity>();

                foreach (TEntity entity in items)
                {
                    set.Add(entity);
                }

                context.SaveChanges();
            }

            return items.ToList();
        }
        public virtual void Update(IEnumerable<TEntity> items)
        {
            ExecuteOperation(EntityState.Modified, items);
        }
        public virtual void Remove(IEnumerable<TEntity> items)
        {
            ExecuteOperation(EntityState.Deleted, items);
        }
        private void ExecuteOperation(EntityState entityState, IEnumerable<TEntity> items)
        {
            using (var context = new PolitermUsers())
            {
                foreach (TEntity entity in items)
                {
                    context.Entry(entity).State = entityState;
                }

                context.SaveChanges();
            }
        }
    }
}
