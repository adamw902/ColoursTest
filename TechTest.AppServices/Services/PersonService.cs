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

        public async Task<IEnumerable<Person>> GetAllPeople()
        {
            var people = (await this.People.GetAll()).ToList();
            foreach (var person in people)
            {
                person.FavouriteColours = (await this.Colours.GetByIds(person.FavouriteColourIds)).ToList();
            }

            return people;
        }

        public async Task<Person> GetPerson(Guid id)
        {
            var person = await this.People.GetById(id);
            person.FavouriteColours = (await this.Colours.GetByIds(person.FavouriteColourIds)).ToList();

            return person;
        }

        public async Task<Person> CreatePerson(CreateUpdatePerson request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Cannot create null person.");
            }

            IEnumerable<Colour> colours = new List<Colour>();
            if (request.FavouriteColourIds != null)
            {
                colours = await this.Colours.GetByIds(request.FavouriteColourIds);
            }

            var person = new Person(Guid.NewGuid(), request.FirstName, request.LastName, 
                                    request.IsAuthorised ?? false, request.IsValid ?? false, 
                                    request.IsEnabled ?? false, colours.Select(c => c.Id).ToList());
            
            await this.People.Insert(person);

            person.FavouriteColours = colours.ToList();

            return person;
        }

        public async Task<Person> UpdatePerson(Guid personId, CreateUpdatePerson request)
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

            var colourIds = person.FavouriteColourIds;
            if (request.FavouriteColourIds != null)
            {
                person.FavouriteColours = (await this.Colours.GetByIds(request.FavouriteColourIds)).ToList();
                colourIds = person.FavouriteColours.Select(c => c.Id).ToList();
            }

            person.FirstName = !string.IsNullOrWhiteSpace(request.FirstName) ? request.FirstName : person.FirstName;
            person.LastName = !string.IsNullOrWhiteSpace(request.LastName) ? request.LastName : person.LastName;
            person.IsAuthorised = request.IsAuthorised ?? person.IsAuthorised;
            person.IsValid = request.IsValid ?? person.IsEnabled;
            person.IsEnabled = request.IsEnabled ?? person.IsEnabled;
            person.FavouriteColourIds = colourIds;

            await this.People.Update(person);

            return person;
        }
    }
}
