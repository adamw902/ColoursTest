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

            var colour = new Colour(0, request.Name, request.IsEnabled ?? false);

            return await this.Colours.Insert(colour);
        }

        public async Task<Colour> UpdateColour(int colourId, CreateUpdateColour request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot update null colour.");
            }

            var colour = await this.Colours.GetById(colourId);

            if (colour == null)
            {
                return null;
            }

            colour.Name = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : colour.Name;
            colour.IsEnabled = request.IsEnabled ?? colour.IsEnabled;

            return await this.Colours.Update(colour);
        }
    }
}