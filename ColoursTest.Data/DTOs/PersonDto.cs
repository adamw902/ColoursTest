using System.Collections.Generic;
using ColoursTest.Data.Models;

namespace ColoursTest.Data.DTOs
{
    public class PersonDto
    {
        public PersonDetailsDto PersonDetails { get; set; }
        public List<Colour> FavouriteColours { get; set; }
    }
}