using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Person> CreatePerson(CreateUpdatePerson request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot create null person.");
            }

            var colours = new List<Colour>();

            if (request.FavouriteColours != null)
            {
                colours = (await this.Colours.GetAll()).Where(c => request.FavouriteColours.Contains(c.ColourId)).ToList();
            }

            var person = new Person(0, request.FirstName, request.LastName, request.IsAuthorised ?? false,
                                    request.IsValid ?? false, request.IsEnabled ?? false);

            person.FavouriteColours = colours;

            return await this.People.Insert(person);
        }

        public async Task<Person> UpdatePerson(int personId, CreateUpdatePerson request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot update null person.");
            }

            var person = await this.People.GetById(personId);

            if (person == null)
            {
                return null;
            }

            var colours = request.FavouriteColours != null
                            ? (await this.Colours.GetAll()).Where(c => request.FavouriteColours.Contains(c.ColourId))
                            : person.FavouriteColours;
            
            person.FirstName = !string.IsNullOrWhiteSpace(request.FirstName) ? request.FirstName : person.FirstName;
            person.LastName = !string.IsNullOrWhiteSpace(request.LastName) ? request.LastName : person.LastName;
            person.IsAuthorised = request.IsAuthorised ?? person.IsAuthorised;
            person.IsValid = request.IsValid ?? person.IsEnabled;
            person.IsEnabled = request.IsEnabled ?? person.IsEnabled;
            person.FavouriteColours = colours.ToList();

            return await this.People.Update(person);
        }
    }
}
