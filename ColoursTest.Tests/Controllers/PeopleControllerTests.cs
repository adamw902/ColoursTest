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
    public class PeopleControllerTests
    {
        [Fact]
        public void GetAll_RetrievesAllPersons()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            var personService = Substitute.For<IPersonService>();

            var personsController = new PeopleController(personRepository, personService);

            // Act
            personsController.Get().Wait();

            // Assert
            personRepository.Received(1).GetAll();
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithCollectionOfPersonDtos()
        {
            // Arange
            var people = new List<Person>
            {
                this.Person,
                this.Person
            }.AsEnumerable();

            var expectedPeopleDto = new List<PersonDto>
            {
                this.ExpectedPersonDto,
                this.ExpectedPersonDto
            }.AsEnumerable();

            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetAll().Returns(Task.FromResult(people));

            var personService = Substitute.For<IPersonService>();

            var personsController = new PeopleController(personRepository, personService);

            // Act
            var result = await personsController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<List<PersonDto>>(okObjectResult.Value);

            var peopleDto = (IEnumerable<PersonDto>)okObjectResult.Value;
            Assert.Equal(expectedPeopleDto, peopleDto, Comparers.PersonDtoComparer());
        }

        [Fact]
        public async Task GetOne_RetrievesPerson()
        {
            // Arange
            var personId = Guid.NewGuid();

            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            // Act
            await peopleController.Get(personId);

            // Assert
            await personRepository.Received(1).GetById(personId);
        }

        [Fact]
        public async Task GetOne_IncorrectPersonId_ReturnsNotFound()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(null as Person));

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            // Act
            var result = await peopleController.Get(Guid.Empty);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetOne_ValidPersonId_ReturnsOkWithPersonDto()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<Guid>()).Returns(Task.FromResult(this.Person));

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            // Act
            var result = await peopleController.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<PersonDto>(okObjectResult.Value);

            var personDto = (PersonDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedPersonDto, personDto, Comparers.PersonDtoComparer());
        }

        [Fact]
        public async Task Post_InvalidRequestModel_ReturnsBadRequest()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);
            peopleController.ModelState.AddModelError("", "Error");

            // Act
            var result = await peopleController.Post(new CreateUpdatePerson());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Post_ValidRequestModel_CallsCreatePerson()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();
            personService.CreatePerson(Arg.Any<CreateUpdatePerson>())
                         .Returns(Task.FromResult(this.Person));

            var peopleController = new PeopleController(personRepository, personService);

            var comparer = Comparers.CreateUpdatePersonComparer();

            // Act
            await peopleController.Post(this.CreateUpdatePerson);

            // Assert
            await personService.Received(1).CreatePerson(Arg.Is<CreateUpdatePerson>(x => comparer.Equals(x, this.CreateUpdatePerson)));
        }

        [Fact]
        public async Task Post_ValidRequestModel_ReturnsOkWithPersonDto()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();
            personService.CreatePerson(Arg.Any<CreateUpdatePerson>()).Returns(Task.FromResult(this.Person));

            var peopleController = new PeopleController(personRepository, personService);

            // Act
            var result = await peopleController.Post(this.CreateUpdatePerson);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<PersonDto>(okObjectResult.Value);

            var personDto = (PersonDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedPersonDto, personDto, Comparers.PersonDtoComparer());
        }

        [Fact]
        public async Task Put_CallsUpdatePerson()
        {
            // Arange
            var personId = Guid.NewGuid();

            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            var comparer = Comparers.CreateUpdatePersonComparer();

            // Act
            await peopleController.Put(personId, this.CreateUpdatePerson);

            // Assert
            await personService.Received(1)
                    .UpdatePerson(personId,
                        Arg.Is<CreateUpdatePerson>(x => comparer.Equals(x, this.CreateUpdatePerson)));
        }

        [Fact]
        public async Task Put_InvalidPersonId_ReturnsNotFound()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();
            personService.UpdatePerson(Arg.Any<Guid>(), Arg.Any<CreateUpdatePerson>())
                         .Returns(Task.FromResult(null as Person));

            var personController = new PeopleController(personRepository, personService);

            // Act
            var result = await personController.Put(Guid.Empty, this.CreateUpdatePerson);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Put_ValidRequest_ReturnsOkWithPersonDto()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();
            personService.UpdatePerson(Arg.Any<Guid>(), Arg.Any<CreateUpdatePerson>())
                .Returns(Task.FromResult(this.Person));

            var personController = new PeopleController(personRepository, personService);

            // Act
            var result = await personController.Put(Guid.NewGuid(), this.CreateUpdatePerson);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<PersonDto>(okObjectResult.Value);

            var personDto = (PersonDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedPersonDto, personDto, Comparers.PersonDtoComparer());
        }

        private Person Person { get; } = 
            new Person(Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE"), "Test", "Person", true, true, true)
            {
                FavouriteColours = new List<Colour>
                {
                    new Colour(Guid.Parse("439FFD3C-B37D-40BB-9A9E-A48838C1AF23"), "Blue", true),
                    new Colour(Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF"), "Red", true)
                }
            };

        private PersonDto ExpectedPersonDto { get; } =
            new PersonDto
            {
                Id = Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE"),
                FirstName = "Test",
                LastName = "Person",
                IsEnabled = true,
                IsValid = true,
                IsAuthorised = true,
                FavouriteColours = new List<ColourDto>
                {
                    new ColourDto
                    {
                        Id = Guid.Parse("439FFD3C-B37D-40BB-9A9E-A48838C1AF23"),
                        Name = "Blue",
                        IsEnabled = true
                    },
                    new ColourDto
                    {
                        Id = Guid.Parse("5B42FFD4-31E0-40C7-8CD3-442E485577AF"),
                        Name = "Red",
                        IsEnabled = true
                    }
                }
            };

        private CreateUpdatePerson CreateUpdatePerson { get; } =
            new CreateUpdatePerson
            {
                FirstName = "Test",
                LastName = "Person",
                IsAuthorised = true,
                IsValid = true,
                IsEnabled = true,
                FavouriteColours = new List<string> { "Blue", "Red" }
            };
    }
}