using System;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Tests.Shared.Comparers;
using MongoDB.Driver;

namespace ColoursTest.Tests.Repositories
{
    public class ColourRepositoryTests : BaseRepositoryTest<ColourRepositoryTests>
    {
        [Fact]
        public async Task GetAll_ReturnsCollectionOfColours()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(this.Database);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colours = await colourRepository.GetAll();
            
            // Assert
            Assert.NotEmpty(colours);
        }

        [Fact]
        public async Task GetById_IncorrectColourId_ReturnsNull()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(this.Database);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.GetById(Guid.Empty);

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async Task GetById_CorrectColourId_ReturnsCorrectColour()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(this.Database);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.GetById(this.ExpectedColour.Id);

            // Assert
            Assert.Equal(this.ExpectedColour, colour, Comparers.ColourComparer());
        }

        [Fact]
        public async Task Insert_NullColour_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Insert(null));
        }

        [Fact]
        public async Task Insert_ValidColour_InsertsColourSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(this.Database);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            await colourRepository.Insert(this.ColourToInsert);

            var filter = Builders<Colour>.Filter.Eq("Id", this.ColourToInsert.Id);
            var persistedColour = await this.Database.GetCollection<Colour>("colours").Find(filter).FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(persistedColour);
        }

        [Fact]
        public async Task Update_NullColour_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Update(null));
        }

        [Fact]
        public async Task Update_InvalidId_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            var colour = this.ColourToInsert;
            colour.Id = Guid.Empty;

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Update(colour));
        }

        [Fact]
        public async Task Update_ValidColour_UpdatesSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IMongoConnectionFactory>();
            connectionFactory.GetDatabase().Returns(this.Database);

            var colourRepository = new ColourRepository(connectionFactory);

            var colourToUpdate = this.ExpectedColour;
            colourToUpdate.IsEnabled = false;

            // Act
            await colourRepository.Update(colourToUpdate);

            var filter = Builders<Colour>.Filter.Eq("Id", colourToUpdate.Id);
            var colour = await this.Database.GetCollection<Colour>("colours").Find(filter).FirstOrDefaultAsync();

            // Assert
            Assert.Equal(colourToUpdate, colour, Comparers.ColourComparer());
        }
        
        private Colour ColourToInsert { get; } = new Colour(Guid.Parse("10D05D73-B660-4DBC-AAFA-D7305B2B4E6A"), "Test", true);
        private Colour ExpectedColour { get; } = new Colour(Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF"), "Red", true);
    }
}