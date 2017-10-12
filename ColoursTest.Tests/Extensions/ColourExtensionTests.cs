using System;
using System.Collections.Generic;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Infrastructure.Extensions;
using ColoursTest.Tests.Shared.Comparers;
using Xunit;

namespace ColoursTest.Tests.Extensions
{
    public class ColourExtensionTests
    {
        [Fact]
        public void ToColourDtoSingle_NullColour_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => (null as Colour).ToColourDto());
        }

        [Fact]
        public void ToColourDtoSingle_ValidColour_ReturnsValidColourDto()
        {
            // Act
            var colourDto = this.Colour.ToColourDto();

            // Assert
            Assert.Equal(this.ExpectedColourDto, colourDto, Comparers.ColourDtoComparer());
        }

        [Fact]
        public void ToColourDtoCollection_ValidColourCollection_ReturnsValidColourDtoCollection()
        {
            // Arange
            var colours = new List<Colour>
            {
                this.Colour,
                this.Colour,
                this.Colour,
                this.Colour
            };

            var expectedColoursDto = new List<ColourDto>
            {
                this.ExpectedColourDto,
                this.ExpectedColourDto,
                this.ExpectedColourDto,
                this.ExpectedColourDto
            };

            // Act
            var coloursDto = colours.ToColourDto();

            // Assert
            Assert.Equal(expectedColoursDto, coloursDto, Comparers.ColoursDtoComparer());
        }

        private Colour Colour { get; } = new Colour(1, "Test", true);

        private ColourDto ExpectedColourDto { get; } =
            new ColourDto
            {
                Id = 1,
                Name = "Test",
                IsEnabled = true
            };
    }
}