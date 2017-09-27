using ColoursTest.Data.DTOs;
using ColoursTest.Data.Models;

namespace ColoursTest.Data.Interfaces
{
    public interface IPersonRepository : IBaseRepository<Person, int>
    {
        Person Update(int personId, UpdatePersonDto updatePersonDto);
    }
}