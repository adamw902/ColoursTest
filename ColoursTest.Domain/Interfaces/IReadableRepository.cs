using System.Collections.Generic;

namespace ColoursTest.Domain.Interfaces
{
    public interface IReadableRepository<out T, in TK> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(TK id);
    }
}