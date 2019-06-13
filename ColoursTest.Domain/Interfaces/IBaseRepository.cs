using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColoursTest.Domain.Interfaces
{
    public interface IBaseRepository<T, in TK> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(TK id);
        Task<T> Insert(T item);
        Task Update(T item);
    }
}