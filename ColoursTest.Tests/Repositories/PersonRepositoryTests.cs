using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Tests.Shared.Comparers;
using MongoDB.Driver;
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
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(x => this.Database);

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
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(x => this.Database);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var colour = await personRepository.GetById(Guid.Empty);

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async Task GetById_ValidPersonId_ReturnsCorrectPerson()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(x => this.Database);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            var person = await personRepository.GetById(this.ExpectedPerson.Id);

            // Assert
            Assert.Equal(this.ExpectedPerson, person, Comparers.PersonComparer());
        }

        [Fact]
        public async Task Insert_NullPerson_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Insert(null));
        }

        [Fact]
        public async Task Insert_ValidPerson_InsertsPersonSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(x => this.Database);

            var personRepository = new PersonRepository(connectionFactory);

            // Act
            await personRepository.Insert(this.PersonToInsert);

            var filter = Builders<Person>.Filter.Eq("Id", this.PersonToInsert.Id);
            var persistedPerson = await this.Database.GetCollection<Person>("people").Find(filter).SingleOrDefaultAsync();

            // Assert
            Assert.NotNull(persistedPerson);
        }

        [Fact]
        public async Task Update_NullPerson_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(x => this.Database);

            var personRepository = new PersonRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Update(null));
        }

        [Fact]
        public async Task Update_InvalidPersonId_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();

            var personRepository = new PersonRepository(connectionFactory);

            var invalidPerson = this.PersonToInsert;
            invalidPerson.Id = Guid.Empty;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => personRepository.Update(this.PersonToInsert));
        }

        [Fact]
        public async Task Update_ValidPerson_UpdatesPersonSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(x => this.Database);

            var personRepository = new PersonRepository(connectionFactory);

            var personToUpdate = this.ExpectedPerson;
            personToUpdate.IsEnabled = false;

            // Act
            await personRepository.Update(personToUpdate);

            var filter = Builders<Person>.Filter.Eq("Id", personToUpdate.Id);
            var person = await this.Database.GetCollection<Person>("people").Find(filter).SingleOrDefaultAsync();

            // Assert
            Assert.Equal(personToUpdate, person, Comparers.PersonComparer());
        }

        private Person PersonToInsert { get; } =
            new Person(Guid.Parse("4F4E0E5B-ECB7-44DE-B33C-0A65949C81E7"), 
                       "Inserted", "Person", true, true, true, 
                       new List<Guid>
                       {
                           Guid.Parse("439FFD3C-B37D-40BB-9A9E-A48838C1AF23")
                       });

        private Person ExpectedPerson { get; } =
            new Person(Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE"), 
                       "Test", "1", true, true, true,
                       new List<Guid>
                       {
                           Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF"),
                           Guid.Parse("95D03170-349C-4003-B131-661526C8BD06")
                       });
    }
}