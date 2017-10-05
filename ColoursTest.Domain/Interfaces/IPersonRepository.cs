using ColoursTest.Domain.Models;

namespace ColoursTest.Domain.Interfaces
{
    public interface IPersonRepository : IReadableRepository<Person, int>, IWriteableRepository<Person>{}
}