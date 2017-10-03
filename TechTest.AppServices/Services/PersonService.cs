using System;
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

        public Person UpdatePerson(int personId, UpdatePerson request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Update person request is null.");
            }

            var person = this.People.GetById(personId);
            var colours = this.Colours.GetAll().Where(c => request.FavouriteColours.Contains(c.ColourId));

            person.IsAuthorised = request.IsAuthorised;
            person.IsEnabled = request.IsEnabled;
            person.IsValid = request.IsValid;
            person.FavouriteColours = colours.ToList();

            return this.People.Update(person);
        }
    }
}
