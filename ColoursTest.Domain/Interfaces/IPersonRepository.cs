using ColoursTest.Domain.Models;

namespace ColoursTest.Domain.Interfaces
{
    public interface IPersonRepository : IBaseRepository<Person, int>
    {
    }
}