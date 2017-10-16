using System;
using ColoursTest.Domain.Models;

namespace ColoursTest.Domain.Interfaces
{
    public interface IPersonRepository : IMongoBaseRepository<Person, Guid> { }
}