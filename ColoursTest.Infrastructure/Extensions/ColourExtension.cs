using System.Collections.Generic;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.Infrastructure.Extensions
{
    public static class ColourExtension
    {
        public static ColourDto ToColourDto(this Colour colour)
        {
            var colourDto = new ColourDto
            {
                Id = colour.ColourId,
                Name = colour.Name,
                IsEnabled = colour.IsEnabled
            };
            return colourDto;
        }

        public static List<ColourDto> ToColoursDto(this IEnumerable<Colour> colours)
        {
            var coloursDto = new List<ColourDto>();
            foreach (var colour in colours)
            {
                coloursDto.Add(colour.ToColourDto());
            }
            return coloursDto;
        }
    }
}
