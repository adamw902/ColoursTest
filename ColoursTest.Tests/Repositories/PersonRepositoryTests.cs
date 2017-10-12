using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Tests.Shared.Comparers;
using NSubstitute;
using Xunit;

namespace ColoursTest.Tests.Repositories
{
    public class PersonRepositoryTests
    {
        public PersonRepositoryTests()
        {
            DatabaseSetup.Setup("PersonRepositoryTestsDB");
        }

        [Fact]
        public async void GetAll_ReturnsCollectionOfPeople()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var people = await personRepository.GetAll();

            // Assert
            Assert.NotEmpty(people);
        }

        [Fact]
        public async void GetById_IncorrectPersonId_ReturnsNull()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var colour = await personRepository.GetById(new int());

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async void GetById_ValidPersonId_ReturnsCorrectPerson()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var person = await personRepository.GetById(this.ExpectedPerson.PersonId);

            // Assert
            Assert.Equal(this.ExpectedPerson, person, Comparers.PersonComparer());
        }

        [Fact]
        public async void Insert_NullPerson_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Insert(null));
        }

        [Fact]
        public async void Insert_ValidPerson_ReturnsPersonWithPersonId()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var person = await personRepository.Insert(this.PersonToInsert);

            // Assert
            Assert.NotEqual(0, person.PersonId);
        }

        [Fact]
        public async void Insert_ValidPerson_InsertsPersonAndColoursSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var person = await personRepository.Insert(this.PersonToInsert);
            var persistedPerson = await personRepository.GetById(person.PersonId);

            // Assert
            Assert.NotNull(persistedPerson);
            Assert.Equal(this.PersonToInsert.FavouriteColours, persistedPerson.FavouriteColours, Comparers.ColoursComparer());
        }

        [Fact]
        public async void Update_NullPerson_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Update(null));
        }

        [Fact]
        public async void Update_InvalidPersonId_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Update(this.PersonToInsert));
        }

        [Fact]
        public async void Update_ValidPerson_UpdatesPersonSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            var personToUpdate = this.ExpectedPerson;
            personToUpdate.IsEnabled = false;

            // Act
            personRepository.Update(personToUpdate).Wait();
            var person = await personRepository.GetById(personToUpdate.PersonId);

            // Assert
            Assert.Equal(personToUpdate, person, Comparers.PersonComparer());
        }

        [Fact]
        public async void Update_ValidPerson_UpdatesFavouriteColoursSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            var personToUpdate = this.ExpectedPerson;
            personToUpdate.FavouriteColours.Add(new Colour(3, "Blue", true));

            // Act
            personRepository.Update(personToUpdate).Wait();
            var person = await personRepository.GetById(personToUpdate.PersonId);

            // Assert
            Assert.Equal(personToUpdate.FavouriteColours, person.FavouriteColours, Comparers.ColoursComparer());
        }

        private SqlConnection Connection => new SqlConnection("data source=localhost;initial catalog=PersonRepositoryTestsDB;integrated security=true;MultipleActiveResultSets=True;");

        private Person PersonToInsert { get; } = 
            new Person(0, "Inserted", "Person", true, true, true)
            {
                FavouriteColours = new List<Colour>
                {
                    new Colour(3, "Blue", true)
                }
            };

        private Person ExpectedPerson { get; } =
            new Person(1, "Test", "1", true, true, true)
            {
                FavouriteColours = new List<Colour>
                {
                    new Colour(1, "Red", true),
                    new Colour(2, "Green", true)
                }
            };
    }
}