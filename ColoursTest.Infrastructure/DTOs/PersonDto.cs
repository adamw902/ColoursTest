using System;
using System.Collections.Generic;
using System.Linq;
using ColoursTest.Infrastructure.Extensions;

namespace ColoursTest.Infrastructure.DTOs
{
    public class PersonDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";

        public bool IsPalindrome => this.FullName.IsPalindrome();

        public bool IsAuthorised { get; set; }

        public bool IsValid { get; set; }

        public bool IsEnabled { get; set; }

        public IEnumerable<ColourDto> FavouriteColours { get; set; }

        public string FavouriteColoursString => string.Join(", ", this.FavouriteColours.Select(c => c.Name));
    }
}