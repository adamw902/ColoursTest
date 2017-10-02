using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Interfaces
{
    public interface IPersonService
    {
        Person UpdatePerson(int personId, UpdatePerson request);
    }
}