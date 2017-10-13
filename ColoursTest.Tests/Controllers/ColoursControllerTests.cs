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
            Assert.IsType<List<ColourDto>>(okObjectResult.Value);

            var coloursDto = (IEnumerable<ColourDto>) okObjectResult.Value;
            Assert.Equal(expectedColoursDto, coloursDto, Comparers.ColourDtoComparer());
        }

        [Fact]
        public async Task GetOne_RetrievesColour()
        {
            // Arange
            var colourId = 1;

            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();

            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            await coloursController.Get(colourId);

            // Assert
            await colourRepository.Received(1).GetById(colourId);
        }

        [Fact]
        public async Task GetOne_IncorrectColourId_ReturnsNotFound()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(null as Colour));

            var colourService = Substitute.For<IColourService>();
            
            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await coloursController.Get(new int());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOne_ValidColourId_ReturnsOkWithColourDto()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();
            colourRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(this.Colour));

            var colourService = Substitute.For<IColourService>();

            var coloursController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await coloursController.Get(new int());

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
            var colourId = 1;

            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();

            var colourController = new ColoursController(colourRepository, colourService);

            var comparer = Comparers.CreateUpdateColourComparer();

            // Act
            await colourController.Put(colourId, this.CreateUpdateColour);

            // Assert
            await colourService.Received(1)
                    .UpdateColour(colourId,
                        Arg.Is<CreateUpdateColour>(x => comparer.Equals(x, this.CreateUpdateColour)));
        }

        [Fact]
        public async Task Put_InvalidColourId_ReturnsNotFound()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();
            colourService.UpdateColour(Arg.Any<int>(), Arg.Any<CreateUpdateColour>())
                         .Returns(Task.FromResult(null as Colour));

            var colourController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await colourController.Put(new int(), this.CreateUpdateColour);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_ValidRequest_ReturnsOkWithColourDto()
        {
            // Arange
            var colourRepository = Substitute.For<IColourRepository>();

            var colourService = Substitute.For<IColourService>();
            colourService.UpdateColour(Arg.Any<int>(), Arg.Any<CreateUpdateColour>())
                         .Returns(Task.FromResult(this.Colour));

            var colourController = new ColoursController(colourRepository, colourService);

            // Act
            var result = await colourController.Put(new int(), this.CreateUpdateColour);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<ColourDto>(okObjectResult.Value);

            var colourDto = (ColourDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedColourDto, colourDto, Comparers.ColourDtoComparer());
        }

        private Colour Colour { get; } = new Colour(1, "blue", true);

        private ColourDto ExpectedColourDto { get; } =
            new ColourDto
            {
                Id = 1,
                Name = "blue",
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