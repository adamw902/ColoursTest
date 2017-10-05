using System;
using System.Collections.Generic;
using System.Linq;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.AppServices.Services
{
    public class PersonService : IPersonService
    {
        public PersonService(IPersonRepository people, IColourRepository colours)
        {
            this.People = people;
            this.Colours = colours;
        }

        private IPersonRepository People { get; }
        private IColourRepository Colours { get; }

        public Person CreatePerson(CreateUpdatePerson request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot create null person.");
            }

            List<Colour> colours = new List<Colour>();
            if (request.FavouriteColours != null)
            {
                colours = this.Colours.GetAll().Where(c => request.FavouriteColours.Contains(c.ColourId)).ToList();
            }
            var person = new Person(request.FirstName, request.LastName, request.IsAuthorised,
                                    request.IsValid, request.IsEnabled, colours);
            return this.People.Insert(person);
        }

        public Person UpdatePerson(int personId, CreateUpdatePerson request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot update null person.");
            }

            var person = this.People.GetById(personId);
            var colours = request.FavouriteColours != null
                            ? this.Colours.GetAll().Where(c => request.FavouriteColours.Contains(c.ColourId))
                            : person.FavouriteColours;
            
            person.FirstName = !string.IsNullOrWhiteSpace(request.FirstName) ? request.FirstName : person.FirstName;
            person.LastName = !string.IsNullOrWhiteSpace(request.LastName) ? request.LastName : person.LastName;
            person.IsAuthorised = request.IsAuthorised ?? person.IsAuthorised;
            person.IsValid = request.IsValid ?? person.IsEnabled;
            person.IsEnabled = request.IsEnabled ?? person.IsEnabled;
            person.FavouriteColours = colours.ToList();

            return this.People.Update(person);
        }
    }
}
