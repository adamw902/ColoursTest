using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColoursTest.Domain.Models;

namespace ColoursTest.Domain.Interfaces
{
    public interface IColourRepository : IMongoBaseRepository<Colour, Guid>
    {
        Task<IEnumerable<Colour>> GetByIds(IEnumerable<Guid> ids);
    }
}