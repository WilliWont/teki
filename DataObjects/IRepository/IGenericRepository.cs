using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataObjects.IRepository
{
    public interface IGenericRepository<T> where T :class
    {
        // Here is 8 basic methods for an entity.
        T GetByID(int ID);
        T GetByID(Guid ID);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
