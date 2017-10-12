using System;
using System.Data.SqlClient;
using NSubstitute;
using Xunit;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using ColoursTest.Infrastructure.Repositories;
using ColoursTest.Tests.Shared.Comparers;

namespace ColoursTest.Tests.Repositories
{
    public class ColourRepositoryTests
    {
        public ColourRepositoryTests()
        {
            DatabaseSetup.Setup("ColourRepositoryTestsDB");
        }

        [Fact]
        public async void GetAll_ReturnsCollectionOfColours()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colours = await colourRepository.GetAll();
            
            // Assert
            Assert.NotEmpty(colours);
        }

        [Fact]
        public async void GetById_IncorrectColourId_ReturnsNull()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.GetById(new int());

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async void GetById_CorrectColourId_ReturnsCorrectColour()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.GetById(this.ExpectedColour.ColourId);

            // Assert
            Assert.Equal(this.ExpectedColour, colour, Comparers.ColourComparer());
        }

        [Fact]
        public async void Insert_NullColour_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Insert(null));
        }

        [Fact]
        public async void Insert_ValidColour_ReturnsColourWithColourId()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.Insert(this.ColourToInsert);

            // Assert
            Assert.NotEqual(0, colour.ColourId);
        }

        [Fact]
        public async void Insert_ValidColour_InsertsColourSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            // Act
            var colour = await colourRepository.Insert(this.ColourToInsert);
            var persistedColour = await colourRepository.GetById(colour.ColourId);
            
            // Assert
            Assert.NotNull(persistedColour);
        }

        [Fact]
        public async void Update_NullColour_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Update(null));
        }

        [Fact]
        public async void Update_InvalidColourId_ThrowsArgumentNullException()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();

            var colourRepository = new ColourRepository(connectionFactory);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourRepository.Update(this.ColourToInsert));
        }

        [Fact]
        public async void Update_ValidColour_UpdatesSuccessfully()
        {
            // Arange
            var connectionFactory = Substitute.For<IConnectionFactory>();
            connectionFactory.GetConnection().Returns(x => this.Connection);

            var colourRepository = new ColourRepository(connectionFactory);

            var colourToUpdate = await colourRepository.GetById(this.ExpectedColour.ColourId);
            colourToUpdate.IsEnabled = false;

            // Act
            colourRepository.Update(colourToUpdate).Wait();
            var colour = await colourRepository.GetById(colourToUpdate.ColourId);

            // Assert
            Assert.Equal(colourToUpdate, colour, Comparers.ColourComparer());
        }

        private SqlConnection Connection => new SqlConnection("data source=localhost;initial catalog=ColourRepositoryTestsDB;integrated security=true;");

        private Colour ColourToInsert => new Colour(0, "Test", true);
        private Colour ExpectedColour => new Colour(1, "Red", true);
    }
}