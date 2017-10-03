using System.Collections.Generic;
using ColoursTest.Domain.Models;

namespace ColoursTest.Infrastructure.DTOs
{
    public class PersonDto
    {
        public PersonDetailsDto PersonDetails { get; set; }
        public List<Colour> FavouriteColours { get; set; }
    }
}