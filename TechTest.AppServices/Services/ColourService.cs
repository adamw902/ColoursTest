using System;
using System.Threading.Tasks;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Services
{
    public class ColourService : IColourService
    {
        public ColourService(IColourRepository colours)
        {
            this.Colours = colours;
        }

        private IColourRepository Colours { get; }

        public async Task<Colour> CreateColour(CreateUpdateColour request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot create null colour.");
            }

            var colour = new Colour(Guid.NewGuid(), request.Name, request.IsEnabled ?? false);

            await this.Colours.Insert(colour);

            return colour;
        }

        public async Task<Colour> UpdateColour(Guid id, CreateUpdateColour request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot update null colour.");
            }

            var colour = await this.Colours.GetById(id);

            if (colour == null)
            {
                return null;
            }

            colour.Name = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : colour.Name;
            colour.IsEnabled = request.IsEnabled ?? colour.IsEnabled;

            await this.Colours.Update(colour);

            return colour;
        }
    }
}