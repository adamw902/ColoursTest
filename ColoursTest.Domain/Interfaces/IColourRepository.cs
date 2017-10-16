using System;
using ColoursTest.Domain.Models;

namespace ColoursTest.Domain.Interfaces
{
    public interface IColourRepository : IMongoBaseRepository<Colour, Guid>
    {
    }
}