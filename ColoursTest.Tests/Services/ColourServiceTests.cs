using System;
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
    public class ColourServiceTests
    {
        [Fact]
        public async Task CreateColour_NullCreateUpdateColour_ThrowsArgumentNullException()
        {
            // Arrange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = new ColourService(colourRepository);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourService.CreateColour(null));
        }

        [Fact]
        public async Task CreateColour_ValidCreateUpdateColour_ColourIsSaved()
        {
            // Arange
            var expectedColour = this.ExpectedColour;
            expectedColour.Id = Guid.Empty;

            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = new ColourService(colourRepository);

            var comparer = Comparers.ColourWithNewIdComparer();

            // Act
            await colourService.CreateColour(this.CreateUpdateColour);

            // Assert
            await colourRepository.Received(1).Insert(Arg.Is<Colour>(x => comparer.Equals(x, expectedColour)));
        }

        [Fact]
        public async Task CreateColour_ValidCreateUpdateColour_ReturnsValidColour()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.Insert(Arg.Any<Colour>()).Returns(Task.FromResult(this.ExpectedColour));

            var colourService = new ColourService(colourRepository);

            // Act
            var colour = await colourService.CreateColour(this.CreateUpdateColour);

            // Assert
            Assert.Equal(this.ExpectedColour, colour, Comparers.ColourWithNewIdComparer());
        }

        [Fact]
        public async Task UpdateColour_NullCreateUpdateColour_ThrowsArgumentNullException()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = new ColourService(colourRepository);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourService.UpdateColour(Guid.Empty, null));
        }

        [Fact]
        public async Task UpdateColour_ValidCreateUpdateColour_GetsExistingColour()
        {
            // Arange
            var id = Guid.NewGuid();

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.ExpectedColour));

            var colourService = new ColourService(colourRepository);

            // Act
            await colourService.UpdateColour(id, this.CreateUpdateColour);

            // Assert
            await colourRepository.Received(1).GetById(id);
        }

        [Fact]
        public async Task UpdateColour_InvalidColourId_ReturnsNull()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(null as Colour));

            var colourService = new ColourService(colourRepository);

            // Act
            var colour = await colourService.UpdateColour(Guid.Empty, this.CreateUpdateColour);

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async Task UpdateColour_ValidCreateUpdateColour_UpdateIsCalled()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.ExpectedColour));

            var colourService = new ColourService(colourRepository);

            var comparer = Comparers.ColourComparer();

            // Act
            await colourService.UpdateColour(Guid.NewGuid(), this.CreateUpdateColour);

            // Assert
            await colourRepository.Received(1).Update(Arg.Is<Colour>(x => comparer.Equals(x, this.ExpectedColour)));
        }

        [Fact]
        public async Task UpdateColour_ValidCreateUpdateColour_ValuesAreUpdated()
        {
            // Arange
            var id = this.ExpectedColour.Id;
            var colourToUpdate = new Colour(id, "Old", false);

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(colourToUpdate));

            var colourService = new ColourService(colourRepository);

            // Act
            var updatedColour = await colourService.UpdateColour(id, this.CreateUpdateColour);

            // Assert
            Assert.Equal(this.ExpectedColour, updatedColour, Comparers.ColourComparer());
        }

        private CreateUpdateColour CreateUpdateColour { get; } =
            new CreateUpdateColour
            {
                Name = "Test",
                IsEnabled = true
            };

        private Colour ExpectedColour { get; } = new Colour(Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF"), "Test", true);
    }
}