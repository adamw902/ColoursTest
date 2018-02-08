using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeople();
        Task<Person> GetPerson(Guid id);
        Task<Person> CreatePerson(CreateUpdatePerson request);
        Task<Person> UpdatePerson(Guid personId, CreateUpdatePerson request);
    }
}