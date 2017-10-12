using System;
using System.Collections.Generic;
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
        public async void CreatePerson_NullCreateUpdatePerson_ThrowsArgumentNullException()
        {
            // Arrange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personService.CreatePerson(null));
        }

        [Fact]
        public void CreatePerson_ValidCreateUpdatePerson_PersonIsSaved()
        {
            // Arange
            var expectedPerson = this.ExpectedPerson;
            expectedPerson.PersonId = new int();

            var personRepository = Substitute.For<IPersonRepository>();

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            var comparer = Comparers.PersonComparer();

            // Act
            personService.CreatePerson(this.CreateUpdatePerson).Wait();

            // Assert
            personRepository.Received(1).Insert(Arg.Is<Person>(x => comparer.Equals(x, expectedPerson)));
        }

        [Fact]
        public void CreatePerson_ValidCreateUpdatePersonWithoutColours_ColoursAreNotRetrieved()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            var personWithoutColours = this.CreateUpdatePerson;
            personWithoutColours.FavouriteColours = null;

            // Act
            personService.CreatePerson(personWithoutColours).Wait();

            // Assert
            colourRepository.DidNotReceive().GetAll();
        }

        [Fact]
        public void CreatePerson_ValidCreateUpdatePersonWithColours_ColoursAreRetrieved()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            personService.CreatePerson(this.CreateUpdatePerson).Wait();

            // Assert
            colourRepository.Received(1).GetAll();
        }

        [Fact]
        public async void CreatePerson_ValidCreateUpdatePerson_ReturnsValidPerson()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.Insert(Arg.Any<Person>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            var person = await personService.CreatePerson(this.CreateUpdatePerson);
            
            // Assert
            Assert.Equal(this.ExpectedPerson, person, Comparers.PersonComparer());
        }

        [Fact]
        public async void UpdatePerson_NullCreateUpdatePerson_ThrowsArgumentNullException()
        {
            // Arrange
            var personRepository = Substitute.For<IPersonRepository>();
            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personService.UpdatePerson(new int(), null));
        }

        [Fact]
        public void UpdatePerson_ValidCreateUpdatePerson_GetsExistingPerson()
        {
            // Arange
            var personId = 1;

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(personId).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);
            
            // Act
            personService.UpdatePerson(personId, this.CreateUpdatePerson).Wait();

            // Assert
            personRepository.Received(1).GetById(personId);
        }

        [Fact]
        public async void UpdatePerson_InvalidPersonId_ReturnsNull()
        {
            // Arange
            var personId = 99999999;

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(personId).Returns(Task.FromResult(null as Person));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            var person = await personService.UpdatePerson(personId, this.CreateUpdatePerson);

            // Assert
            Assert.Null(person);
        }

        [Fact]
        public void UpdatePerson_ValidCreateUpdatePerson_UpdateIsCalled()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);
            
            var comparer = Comparers.PersonComparer();

            // Act
            personService.UpdatePerson(new int(), this.CreateUpdatePerson).Wait();

            // Assert
            personRepository.Received(1).Update(Arg.Is<Person>(x => comparer.Equals(x, this.ExpectedPerson)));
        }

        [Fact]
        public async void UpdatePerson_ValidCreateUpdatePerson_ValuesAreUpdated()
        {
            // Arange
            var personId = 1;
            var personToUpdate = new Person (1, "Old", "Name", false, false, false)
            {
                FavouriteColours = new List<Colour>()
            };

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(personId).Returns(Task.FromResult(personToUpdate));
            personRepository.Update(Arg.Any<Person>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);

            // Act
            var updatedPerson = await personService.UpdatePerson(personId, this.CreateUpdatePerson);

            // Assert
            Assert.Equal(this.ExpectedPerson, updatedPerson, Comparers.PersonComparer());
        }

        [Fact]
        public void UpdatePerson_ValidCreateUpdatePersonWithoutColours_ColoursAreNotUpdated()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();

            var personService = new PersonService(personRepository, colourRepository);

            var personWithoutColours = this.CreateUpdatePerson;
            personWithoutColours.FavouriteColours = null;

            // Act
            personService.UpdatePerson(new int(), personWithoutColours).Wait();

            // Assert
            colourRepository.DidNotReceive().GetAll();
        }

        [Fact]
        public void UpdatePerson_ValidCreateUpdatePersonWithColours_ColoursAreUpdated()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(this.ExpectedPerson));

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(this.Colours));

            var personService = new PersonService(personRepository, colourRepository);
            
            // Act
            personService.UpdatePerson(new int(), this.CreateUpdatePerson).Wait();

            // Assert
            colourRepository.Received(1).GetAll();
        }

        private CreateUpdatePerson CreateUpdatePerson { get; } =
            new CreateUpdatePerson
            {
                FirstName = "Test",
                LastName = "Person",
                IsAuthorised = true,
                IsEnabled = true,
                IsValid = true,
                FavouriteColours = new List<int> {1, 2}
            };

        private Person ExpectedPerson { get; } =
            new Person(1, "Test", "Person", true, true, true)
            {
                FavouriteColours = new List<Colour>
                {
                    new Colour(1, "blue", true),
                    new Colour(2, "red", true)
                }
            };

        private IEnumerable<Colour> Colours { get; } = 
            new List<Colour>
            {
                new Colour(1, "blue", true),
                new Colour(2, "red", true),
                new Colour(3, "green", true)
            };
    }
}