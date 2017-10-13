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
            expectedColour.ColourId = new int();

            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = new ColourService(colourRepository);

            var comparer = Comparers.ColourComparer();

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
            Assert.Equal(this.ExpectedColour, colour, Comparers.ColourComparer());
        }

        [Fact]
        public async Task UpdateColour_NullCreateUpdateColour_ThrowsArgumentNullException()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = new ColourService(colourRepository);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => colourService.UpdateColour(new int(), null));
        }

        [Fact]
        public async Task UpdateColour_ValidCreateUpdateColour_GetsExistingColour()
        {
            // Arange
            var colourId = 1;

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(colourId).Returns(Task.FromResult(this.ExpectedColour));

            var colourService = new ColourService(colourRepository);

            // Act
            await colourService.UpdateColour(colourId, this.CreateUpdateColour);

            // Assert
            await colourRepository.Received(1).GetById(colourId);
        }

        [Fact]
        public async Task UpdateColour_InvalidColourId_ReturnsNull()
        {
            // Arange
            var colourId = 9999999;

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(colourId).Returns(Task.FromResult(null as Colour));

            var colourService = new ColourService(colourRepository);

            // Act
            var colour = await colourService.UpdateColour(colourId, this.CreateUpdateColour);

            // Assert
            Assert.Null(colour);
        }

        [Fact]
        public async Task UpdateColour_ValidCreateUpdateColour_UpdateIsCalled()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(this.ExpectedColour));

            var colourService = new ColourService(colourRepository);

            var comparer = Comparers.ColourComparer();

            // Act
            await colourService.UpdateColour(new int(), this.CreateUpdateColour);

            // Assert
            await colourRepository.Update(Arg.Is<Colour>(x => comparer.Equals(x, this.ExpectedColour)));
        }

        [Fact]
        public async Task UpdateColour_ValidCreateUpdateColour_ValuesAreUpdated()
        {
            // Arange
            var colourId = 1;
            var colourToUpdate = new Colour(1, "Old", false);

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(colourToUpdate));
            colourRepository.Update(Arg.Any<Colour>()).Returns(this.ExpectedColour);

            var colourService = new ColourService(colourRepository);

            // Act
            var updatedColour = await colourService.UpdateColour(colourId, this.CreateUpdateColour);

            // Assert
            Assert.Equal(this.ExpectedColour, updatedColour, Comparers.ColourComparer());
        }

        private CreateUpdateColour CreateUpdateColour { get; } =
            new CreateUpdateColour
            {
                Name = "Test",
                IsEnabled = true
            };

        private Colour ExpectedColour { get; } = new Colour(1, "Test", true);
    }
}