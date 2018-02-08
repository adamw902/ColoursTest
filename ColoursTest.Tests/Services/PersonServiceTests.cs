using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColoursTest.AppServices.Services;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Tests.Shared.Comparers;
using NSubstitute;
using Xunit;

namespace ColoursTest.Tests.Services
{
    public class PersonServiceTests
    {
        [Fact]
        public async Task CreatePerson_NullCreateUpdatePerson_ThrowsArgumentNullException()
        {
            // Arrange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personService.CreatePerson(null));
        }

        [Fact]
        public async Task CreatePerson_ValidCreateUpdatePerson_InsertIsCalled()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetByIds(Arg.Any<List<Guid>>()).Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            var comparer = Comparers.PersonWithNewIdComparer();

            // Act
            await personService.CreatePerson(this.CreateUpdatePerson);

            // Assert
            await personRepository.Received(1).Insert(Arg.Is<Person>(x => comparer.Equals(x, this.ExpectedPerson)));
        }

        [Fact]
        public async Task CreatePerson_ValidCreateUpdatePersonWithoutColours_ColoursAreNotRetrieved()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            var personWithoutColours = this.CreateUpdatePerson;
            personWithoutColours.FavouriteColourIds = null;

            // Act
            await personService.CreatePerson(personWithoutColours);

            // Assert
            await colourRepository.DidNotReceive().GetAll();
        }

        [Fact]
        public async Task CreatePerson_ValidCreateUpdatePersonWithColours_ColoursAreRetrieved()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            await personService.CreatePerson(this.CreateUpdatePerson);

            // Assert
            await colourRepository.Received(1).GetByIds(Arg.Is<List<Guid>>(x => x.SequenceEqual(this.CreateUpdatePerson.FavouriteColourIds)));
        }

        [Fact]
        public async Task CreatePerson_ValidCreateUpdatePerson_ReturnsValidPerson()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.Insert(Arg.Any<Person>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetByIds(Arg.Any<List<Guid>>()).Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            var person = await personService.CreatePerson(this.CreateUpdatePerson);
            
            // Assert
            Assert.Equal(this.ExpectedPerson, person, Comparers.PersonWithNewIdComparer());
        }

        [Fact]
        public async Task UpdatePerson_NullCreateUpdatePerson_ThrowsArgumentNullException()
        {
            // Arrange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personService.UpdatePerson(Guid.NewGuid(), null));
        }

        [Fact]
        public async Task UpdatePerson_ValidCreateUpdatePerson_GetsExistingPerson()
        {
            // Arange
            var personId = Guid.NewGuid();

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);
            
            // Act
            await personService.UpdatePerson(personId, this.CreateUpdatePerson);

            // Assert
            await personRepository.Received(1).GetById(personId);
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId_ReturnsNull()
        {
            // Arange
            var personId = Guid.Empty;

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(null as Person));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            var person = await personService.UpdatePerson(personId, this.CreateUpdatePerson);

            // Assert
            Assert.Null(person);
        }

        [Fact]
        public async Task UpdatePerson_ValidCreateUpdatePerson_UpdateIsCalled()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetByIds(Arg.Any<List<Guid>>()).Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);
            
            var comparer = Comparers.PersonComparer();

            // Act
            await personService.UpdatePerson(Guid.NewGuid(), this.CreateUpdatePerson);

            // Assert
            await personRepository.Received(1).Update(Arg.Is<Person>(x => comparer.Equals(x, this.ExpectedPerson)));
        }

        [Fact]
        public async Task UpdatePerson_ValidCreateUpdatePerson_ValuesAreUpdated()
        {
            // Arange
            var personId = Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE");
            var personToUpdate = new Person(personId, "Old", "Name", false, false, false, new List<Guid>());

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(personToUpdate));
            personRepository.Update(Arg.Any<Person>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetByIds(Arg.Any<List<Guid>>()).Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            var updatedPerson = await personService.UpdatePerson(personId, this.CreateUpdatePerson);

            // Assert
            Assert.Equal(this.ExpectedPerson, updatedPerson, Comparers.PersonComparer());
        }

        [Fact]
        public async Task UpdatePerson_ValidCreateUpdatePersonWithoutColours_ColoursAreNotUpdated()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            var personWithoutColours = this.CreateUpdatePerson;
            personWithoutColours.FavouriteColourIds = null;

            // Act
            await personService.UpdatePerson(Guid.NewGuid(), personWithoutColours);

            // Assert
            await colourRepository.DidNotReceive().GetAll();
        }

        [Fact]
        public async Task UpdatePerson_ValidCreateUpdatePersonWithColours_ColoursAreUpdated()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetByIds(Arg.Any<List<Guid>>()).Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);
            
            // Act
            await personService.UpdatePerson(Guid.NewGuid(), this.CreateUpdatePerson);

            // Assert
            await colourRepository.Received(1).GetByIds(Arg.Is<List<Guid>>(x => x.SequenceEqual(this.CreateUpdatePerson.FavouriteColourIds)));
        }

        private CreateUpdatePerson CreateUpdatePerson { get; } =
            new CreateUpdatePerson
            {
                FirstName = "Test",
                LastName = "Person",
                IsAuthorised = true,
                IsEnabled = true,
                IsValid = true,
                FavouriteColourIds = new List<Guid>
                {
                    Guid.Parse("95D03170-349C-4003-B131-661526C8BD06"),
                    Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF") 
                }
            };

        private Person ExpectedPerson { get; } =
            new Person(Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE"), "Test", "Person", true, true, true,
                       new List<Guid>
                       {
                           Guid.Parse("95D03170-349C-4003-B131-661526C8BD06"),
                           Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF")
                       })
            {
                FavouriteColours = new List<Colour>
                {
                    new Colour(Guid.Parse("95D03170-349C-4003-B131-661526C8BD06"), "Green", true),
                    new Colour(Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF"), "Red", true)
                }
            };
    
        private IEnumerable<Colour> Colours { get; } = 
            new List<Colour>
            {
                new Colour(Guid.Parse("95D03170-349C-4003-B131-661526C8BD06"), "Green", true),
                new Colour(Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF"), "Red", true)
            };
    }
}