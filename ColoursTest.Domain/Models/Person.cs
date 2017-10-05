using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ColoursTest.Domain.Exceptions;

namespace ColoursTest.Domain.Models
{
    public class Person
    {
        public Person() { }
        public Person(string firstName, string lastName, bool? isAuthorised, bool? isValid, bool? isEnabled, List<Colour> favouriteColours)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new IncorrectFormatException("First Name must have a value.");
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new IncorrectFormatException("Last Name must have a value");
            }
            this.FirstName = firstName;
            this.LastName = lastName;
            this.IsAuthorised = isAuthorised ?? false;
            this.IsValid = isValid ?? false;
            this.IsEnabled = isEnabled ?? false;
            this.FavouriteColours = favouriteColours;
        }

        public int PersonId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool IsAuthorised { get; set; }
        public bool IsValid { get; set; }
        public bool IsEnabled { get; set; }

        public List<Colour> FavouriteColours { get; set; }
    }
}