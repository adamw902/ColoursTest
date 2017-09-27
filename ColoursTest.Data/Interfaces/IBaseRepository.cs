using System.Collections.Generic;
using System.Data;

namespace ColoursTest.Data.Interfaces
{
    public interface IBaseRepository<T, in TK> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(TK id);
        T Insert(T item);
    }
}