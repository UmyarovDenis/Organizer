using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataTransferService.DataAccessLayer.Repositories.Interfaces
{
    internal interface IGenericRepository<TEntity> : IRepository where TEntity : class
    {
        IList<TEntity> GetAll();
        IList<TEntity> GetAll(IEnumerable<Expression<Func<TEntity, object>>> navigationProperties);
        TEntity GetItem(Func<TEntity, bool> where);
        TEntity GetItem(Func<TEntity, bool> where, IEnumerable<Expression<Func<TEntity, object>>> navigationProperties);
        IList<TEntity> Create(IEnumerable<TEntity> items);
        void Update(IEnumerable<TEntity> items);
        void Remove(IEnumerable<TEntity> items);
    }
}
