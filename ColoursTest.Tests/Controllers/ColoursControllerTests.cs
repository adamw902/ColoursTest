using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColoursTest.AppServices.Interfaces;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Tests.Shared.Comparers;
using ColoursTest.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace ColoursTest.Tests.Controllers
{
    public class ColoursControllerTests
    {
        [Fact]
        public void GetAll_RetrievesAllColours()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            var colourService = Substitute.For<IColourService>();

            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            coloursController.Get().Wait();

            // Assert
            colourRepository.Received(1).GetAll();
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithCollectionOfColourDtos()
        {
            // Arange
            var colours = new List<Colour>
            {
                this.Colour,
                this.Colour
            }.AsEnumerable();

            var expectedColoursDto = new List<ColourDto>
            {
                this.ExpectedColourDto,
                this.ExpectedColourDto
            }.AsEnumerable();

            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetAll().Returns(Task.FromResult(colours));

            var colourService = Substitute.For<IColourService>();

            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await coloursController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult) result;
            Assert.IsAssignableFrom<IEnumerable<ColourDto>>(okObjectResult.Value);

            var coloursDto = (IEnumerable<ColourDto>) okObjectResult.Value;
            Assert.Equal(expectedColoursDto, coloursDto, Comparers.ColourDtoComparer());
        }

        [Fact]
        public async Task GetOne_RetrievesColour()
        {
            // Arange
            var id = Guid.NewGuid();

            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();

            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            await coloursController.Get(id);

            // Assert
            await colourRepository.Received(1).GetById(id);
        }

        [Fact]
        public async Task GetOne_IncorrectId_ReturnsNotFound()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(null as Colour));

            var colourService = Substitute.For<IColourService>();
            
            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await coloursController.Get(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOne_ValidId_ReturnsOkWithColourDto()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.Colour));

            var colourService = Substitute.For<IColourService>();

            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await coloursController.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult) result;
            Assert.IsType<ColourDto>(okObjectResult.Value);

            var colourDto = (ColourDto) okObjectResult.Value;
            Assert.Equal(this.ExpectedColourDto, colourDto, Comparers.ColourDtoComparer());
        }

        [Fact]
        public async Task Post_InvalidRequestModel_ReturnsBadRequest()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();

            var colourController = new ColoursController(colourRepository, colourService);
            colourController.ModelState.AddModelError("", "Error");

            // Act
            var result = await colourController.Post(new CreateUpdateColour());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_ValidRequestModel_CallsCreateColour()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();
            colourService.CreateColour(Arg.Any<CreateUpdateColour>()).Returns(Task.FromResult(this.Colour));

            var colourController = new ColoursController(colourRepository, colourService);

            var comparer = Comparers.CreateUpdateColourComparer();

            // Act
            await colourController.Post(this.CreateUpdateColour);

            // Assert
            await colourService.Received(1).CreateColour(Arg.Is<CreateUpdateColour>(x => comparer.Equals(x, this.CreateUpdateColour)));
        }

        [Fact]
        public async Task Post_ValidRequestModel_ReturnsOkWithColourDto()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();
            colourService.CreateColour(Arg.Any<CreateUpdateColour>()).Returns(Task.FromResult(this.Colour));

            var colourController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await colourController.Post(this.CreateUpdateColour);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<ColourDto>(okObjectResult.Value);

            var colourDto = (ColourDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedColourDto, colourDto, Comparers.ColourDtoComparer());
        }

        [Fact]
        public async Task Put_CallsUpdateColour()
        {
            // Arange
            var id = Guid.NewGuid();

            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();

            var colourController = new ColoursController(colourRepository, colourService);

            var comparer = Comparers.CreateUpdateColourComparer();

            // Act
            await colourController.Put(id, this.CreateUpdateColour);

            // Assert
            await colourService.Received(1)
                    .UpdateColour(id,
                        Arg.Is<CreateUpdateColour>(x => comparer.Equals(x, this.CreateUpdateColour)));
        }

        [Fact]
        public async Task Put_InvalidId_ReturnsNotFound()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();
            colourService.UpdateColour(Arg.Any<Guid>(), Arg.Any<CreateUpdateColour>()).Returns(Task.FromResult(null as Colour));

            var colourController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await colourController.Put(Guid.Empty, this.CreateUpdateColour);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_ValidRequest_ReturnsOkWithColourDto()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();
            colourService.UpdateColour(Arg.Any<Guid>(), Arg.Any<CreateUpdateColour>()).Returns(Task.FromResult(this.Colour));

            var colourController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await colourController.Put(this.Colour.Id, this.CreateUpdateColour);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<ColourDto>(okObjectResult.Value);

            var colourDto = (ColourDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedColourDto, colourDto, Comparers.ColourDtoComparer());
        }

        private Colour Colour { get; } = new Colour(Guid.Parse("439FFD3C-B37D-40BB-9A9E-A48838C1AF23"), "Blue", true);

        private ColourDto ExpectedColourDto { get; } =
            new ColourDto
            {
                Id = Guid.Parse("439FFD3C-B37D-40BB-9A9E-A48838C1AF23"),
                Name = "Blue",
                IsEnabled = true
            };

        private CreateUpdateColour CreateUpdateColour { get; } =
            new CreateUpdateColour
            {
                Name = "Test",
                IsEnabled = true
            };
    }
}