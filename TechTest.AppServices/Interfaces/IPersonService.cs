using System.Threading.Tasks;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Interfaces
{
    public interface IPersonService
    {
        Task<Person> CreatePerson(CreateUpdatePerson request);
        Task<Person> UpdatePerson(int personId, CreateUpdatePerson request);
    }
}