using System;
using System.Collections.Generic;
using ColoursTest.Domain.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ColoursTest.Domain.Models
{
    public class Person : IEntity
    {
        public Person(Guid id, string firstName, string lastName, bool isAuthorised, bool isValid, bool isEnabled, IList<Guid> colourIds)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.IsAuthorised = isAuthorised;
            this.IsValid = isValid;
            this.IsEnabled = isEnabled;
            this.FavouriteColourIds = colourIds;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsAuthorised { get; set; }

        public bool IsValid { get; set; }

        public bool IsEnabled { get; set; }
        
        public IList<Guid> FavouriteColourIds { get; set; }
        
        [BsonIgnore]
        private IList<Colour> _favouriteColours;

        public IList<Colour> FavouriteColours
        {
            get => this._favouriteColours ?? new List<Colour>();
            set => this._favouriteColours = value;
        } 
    }
}