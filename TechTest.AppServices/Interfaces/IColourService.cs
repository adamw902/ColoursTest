using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Interfaces
{
    public interface IColourService
    {
        Colour CreateColour(CreateUpdateColour request);
        Colour UpdateColour(int colourId, CreateUpdateColour request);
    }
}