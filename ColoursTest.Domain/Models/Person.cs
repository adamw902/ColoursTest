using System;
using System.Collections.Generic;
using ColoursTest.Domain.Interfaces;

namespace ColoursTest.Domain.Models
{
    public class Person : IEntity
    {
        public Person(Guid id, string firstName, string lastName, bool isAuthorised, bool isValid, bool isEnabled)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.IsAuthorised = isAuthorised;
            this.IsValid = isValid;
            this.IsEnabled = isEnabled;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAuthorised { get; set; }

        public bool IsValid { get; set; }

        public bool IsEnabled { get; set; }

        public IList<Colour> FavouriteColours { get; set; }
    }
}