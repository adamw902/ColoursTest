using System.Collections.Generic;

namespace ColoursTest.Domain.Interfaces
{
    public interface IBaseRepository<T, in TK> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(TK id);
        T Insert(T item);
        T Update(T item);
    }
}