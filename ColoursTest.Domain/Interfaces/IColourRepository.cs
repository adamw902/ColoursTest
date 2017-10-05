using ColoursTest.Domain.Models;

namespace ColoursTest.Domain.Interfaces
{
    public interface IColourRepository : IReadableRepository<Colour, int>, IWriteableRepository<Colour>{}
}