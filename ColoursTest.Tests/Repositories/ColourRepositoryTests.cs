using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NSubstitute;
using Xunit;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Tests.Shared.Comparers;
using Dapper;

namespace ColoursTest.Tests.Repositories
{
    public class ColourRepositoryTests : BaseRepositoryTest
    {
        public ColourRepositoryTests() : base("ColourRepositoryTestsDB") {}

        [Fact]
        public async Task GetAll_ReturnsCollectionOfColours()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

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
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.GetById(new int());

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async Task GetById_CorrectColourId_ReturnsCorrectColour()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.GetById(this.ExpectedColour.ColourId);

            // Assert
            Assert.Equal(this.ExpectedColour, colour, Comparers.ColourComparer());
        }

        [Fact]
        public async Task Insert_NullColour_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Insert(null));
        }

        [Fact]
        public async Task Insert_ValidColour_ReturnsColourWithColourId()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.Insert(this.ColourToInsert);

            // Assert
            Assert.NotEqual(0, colour.ColourId);
        }

        [Fact]
        public async Task Insert_ValidColour_InsertsColourSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.Insert(this.ColourToInsert);

            Colour persistedColour;
            using (var connection = connectionFactory.GetConnection())
            {
                var getById = $"SELECT * FROM [Colours] WHERE ColourId = {colour.ColourId}";
                persistedColour = await connection.QuerySingleOrDefaultAsync<Colour>(getById);
            }

            // Assert
            Assert.NotNull(persistedColour);
        }

        [Fact]
        public async Task Update_NullColour_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Update(null));
        }

        [Fact]
        public async Task Update_InvalidColourId_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Update(this.ColourToInsert));
        }

        [Fact]
        public async Task Update_ValidColour_UpdatesSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IDbConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            var colourToUpdate = await colourRepository.GetById(this.ExpectedColour.ColourId);
            colourToUpdate.IsEnabled = false;

            // Act
            await colourRepository.Update(colourToUpdate);
            
            Colour colour;
            using (var connection = connectionFactory.GetConnection())
            {
                var getById = $"SELECT * FROM [Colours] WHERE ColourId = {colourToUpdate.ColourId}";
                colour = await connection.QuerySingleOrDefaultAsync<Colour>(getById);
            }

            // Assert
            Assert.Equal(colourToUpdate, colour, Comparers.ColourComparer());
        }
        
        private Colour ColourToInsert => new Colour(0, "Test", true);
        private Colour ExpectedColour => new Colour(1, "Red", true);
    }
}