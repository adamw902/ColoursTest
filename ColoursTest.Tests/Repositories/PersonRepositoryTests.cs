using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Tests.Shared.Comparers;
using Dapper;
using NSubstitute;
using Xunit;

namespace ColoursTest.Tests.Repositories
{
    public class PersonRepositoryTests : BaseRepositoryTest<PersonRepositoryTests>
    {
        [Fact]
        public async Task GetAll_ReturnsCollectionOfPeople()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var people = await personRepository.GetAll();

            // Assert
            Assert.NotEmpty(people);
        }

        [Fact]
        public async Task GetById_IncorrectPersonId_ReturnsNull()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var colour = await personRepository.GetById(new int());

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async Task GetById_ValidPersonId_ReturnsCorrectPerson()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var person = await personRepository.GetById(this.ExpectedPerson.PersonId);

            // Assert
            Assert.Equal(this.ExpectedPerson, person, Comparers.PersonComparer());
        }

        [Fact]
        public async Task Insert_NullPerson_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Insert(null));
        }

        [Fact]
        public async Task Insert_ValidPerson_ReturnsPersonWithPersonId()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var person = await personRepository.Insert(this.PersonToInsert);

            // Assert
            Assert.NotEqual(0, person.PersonId);
        }

        [Fact]
        public async Task Insert_ValidPerson_InsertsPersonAndColoursSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var person = await personRepository.Insert(this.PersonToInsert);

            Person persistedPerson;
            using (var connection = connectionFactory.GetConnection())
            {
                var getPerson = $"SELECT * FROM [People] WHERE PersonId = {person.PersonId}";
                var getFavouriteColours = $@"
                            SELECT C.*
                              FROM [Colours] C 
                        INNER JOIN [FavouriteColours] FC 
                                ON C.ColourId = FC.ColourId 
                             WHERE FC.PersonId = {person.PersonId};";
                persistedPerson = await connection.QuerySingleOrDefaultAsync<Person>(getPerson);
                var colours = await connection.QueryAsync<Colour>(getFavouriteColours);
                persistedPerson.FavouriteColours = colours.ToList();
            }

            // Assert
            Assert.Equal(this.PersonToInsert, persistedPerson, Comparers.PersonComparer());
        }

        [Fact]
        public async Task Update_NullPerson_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Update(null));
        }

        [Fact]
        public async Task Update_InvalidPersonId_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Update(this.PersonToInsert));
        }

        [Fact]
        public async Task Update_ValidPerson_UpdatesPersonAndColoursSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var personRepository = new PersonRepository(connectionFactory);

            var personToUpdate = this.ExpectedPerson;
            personToUpdate.IsEnabled = false;

            // Act
            await personRepository.Update(personToUpdate);

            Person person;
            using (var connection = connectionFactory.GetConnection())
            {
                var getPerson = $"SELECT * FROM [People] WHERE PersonId = {personToUpdate.PersonId}";
                var getFavouriteColours = $@"
                            SELECT C.*
                              FROM [Colours] C 
                        INNER JOIN [FavouriteColours] FC 
                                ON C.ColourId = FC.ColourId 
                             WHERE FC.PersonId = {personToUpdate.PersonId};";
                person = await connection.QuerySingleOrDefaultAsync<Person>(getPerson);
                var colours = await connection.QueryAsync<Colour>(getFavouriteColours);
                person.FavouriteColours = colours.ToList();
            }

            // Assert
            Assert.Equal(personToUpdate, person, Comparers.PersonComparer());
        }

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