using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Interfaces
{
    public interface IPersonService
    {
        Person CreatePerson(CreateUpdatePerson request);
        Person UpdatePerson(int personId, CreateUpdatePerson request);
    }
}