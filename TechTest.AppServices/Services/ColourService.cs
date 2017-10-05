using System;
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

        public Colour CreateColour(CreateUpdateColour request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot update null colour.");
            }
            var colour = new Colour(request.Name, request.IsEnabled);
            return this.Colours.Insert(colour);
        }

        public Colour UpdateColour(int colourId, CreateUpdateColour request)
        {
            var colour = this.Colours.GetById(colourId);

            colour.Name = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : colour.Name;
            colour.IsEnabled = request.IsEnabled ?? colour.IsEnabled;

            return this.Colours.Update(colour);
        }
    }
}