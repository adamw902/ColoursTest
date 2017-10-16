using System;
using System.Threading.Tasks;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Interfaces
{
    public interface IColourService
    {
        Task<Colour> CreateColour(CreateUpdateColour request);

        Task<Colour> UpdateColour(Guid id, CreateUpdateColour request);
    }
}