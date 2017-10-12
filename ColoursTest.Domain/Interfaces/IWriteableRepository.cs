using System.Threading.Tasks;

namespace ColoursTest.Domain.Interfaces
{
    public interface IWriteableRepository<T> where T : class
    {
        Task<T> Insert(T item);

        Task<T> Update(T item);
    }
}