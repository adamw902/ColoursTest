using System.Collections.Generic;

namespace ColoursTest.Domain.Models
{
    public class Person
    {
        public Person(int personId, string firstName, string lastName, bool isAuthorised, bool isValid, bool isEnabled)
        {
            this.PersonId = personId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.IsAuthorised = isAuthorised;
            this.IsValid = isValid;
            this.IsEnabled = isEnabled;
        }

        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAuthorised { get; set; }

        public bool IsValid { get; set; }

        public bool IsEnabled { get; set; }

        public IList<Colour> FavouriteColours { get; set; }
    }
}