using System.Collections.Generic;
using ColoursTest.Domain.Models;

namespace ColoursTest.Infrastructure.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAuthorised { get; set; }

        public bool IsValid { get; set; }

        public bool IsEnabled { get; set; }

        public IEnumerable<ColourDto> FavouriteColours { get; set; }
    }
}