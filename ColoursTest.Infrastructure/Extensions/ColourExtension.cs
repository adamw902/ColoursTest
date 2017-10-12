using System;
using System.Collections.Generic;
using System.Linq;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.Infrastructure.Extensions
{
    public static class ColourExtension
    {
        public static ColourDto ToColourDto(this Colour colour)
        {
            if (colour == null)
            {
                throw new ArgumentNullException(nameof(colour), "Cannot map null colour.");
            }

            var colourDto = new ColourDto
            {
                Id = colour.ColourId,
                Name = colour.Name,
                IsEnabled = colour.IsEnabled
            };

            return colourDto;
        }

        public static IEnumerable<ColourDto> ToColourDto(this IEnumerable<Colour> colours)
        {
            return colours.Select(colour => colour.ToColourDto()).ToList();
        }
    }
}