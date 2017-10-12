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
        public async void GetAll_ReturnsOkWithCollectionOfPersonDtos()
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
            Assert.Equal(expectedPeopleDto, peopleDto, Comparers.PeopleDtoComparer());
        }

        [Fact]
        public void GetOne_RetrievesPerson()
        {
            // Arange
            var PersonId = 1;

            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            // Act
            peopleController.Get(PersonId).Wait();

            // Assert
            personRepository.Received(1).GetById(PersonId);
        }

        [Fact]
        public async void GetOne_IncorrectPersonId_ReturnsNotFound()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(null as Person));

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            // Act
            var result = await peopleController.Get(new int());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetOne_ValidPersonId_ReturnsOkWithPersonDto()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();
            personRepository.GetById(Arg.Any<int>()).Returns(Task.FromResult(this.Person));

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            // Act
            var result = await peopleController.Get(new int());

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<PersonDto>(okObjectResult.Value);

            var personDto = (PersonDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedPersonDto, personDto, Comparers.PersonDtoComparer());
        }

        [Fact]
        public async void Post_InvalidRequestModel_ReturnsBadRequest()
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
        public void Post_ValidRequestModel_CallsCreatePerson()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();
            personService.CreatePerson(Arg.Any<CreateUpdatePerson>())
                         .Returns(Task.FromResult(this.Person));

            var peopleController = new PeopleController(personRepository, personService);

            var comparer = Comparers.CreateUpdatePersonComparer();

            // Act
            peopleController.Post(this.CreateUpdatePerson).Wait();

            // Assert
            personService.Received(1).CreatePerson(Arg.Is<CreateUpdatePerson>(x => comparer.Equals(x, this.CreateUpdatePerson)));
        }

        [Fact]
        public async void Post_ValidRequestModel_ReturnsOkWithPersonDto()
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
        public void Put_CallsUpdatePerson()
        {
            // Arange
            var PersonId = 1;

            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();

            var peopleController = new PeopleController(personRepository, personService);

            var comparer = Comparers.CreateUpdatePersonComparer();

            // Act
            peopleController.Put(PersonId, this.CreateUpdatePerson).Wait();

            // Assert
            personService.Received(1)
                .UpdatePerson(PersonId,
                    Arg.Is<CreateUpdatePerson>(x => comparer.Equals(x, this.CreateUpdatePerson)));
        }

        [Fact]
        public async void Put_InvalidPersonId_ReturnsNotFound()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();
            personService.UpdatePerson(Arg.Any<int>(), Arg.Any<CreateUpdatePerson>())
                         .Returns(Task.FromResult(null as Person));

            var personController = new PeopleController(personRepository, personService);

            // Act
            var result = await personController.Put(new int(), this.CreateUpdatePerson);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Put_ValidRequest_ReturnsOkWithPersonDto()
        {
            // Arange
            var personRepository = Substitute.For<IPersonRepository>();

            var personService = Substitute.For<IPersonService>();
            personService.UpdatePerson(Arg.Any<int>(), Arg.Any<CreateUpdatePerson>())
                .Returns(Task.FromResult(this.Person));

            var personController = new PeopleController(personRepository, personService);

            // Act
            var result = await personController.Put(new int(), this.CreateUpdatePerson);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            var okObjectResult = (OkObjectResult)result;
            Assert.IsType<PersonDto>(okObjectResult.Value);

            var personDto = (PersonDto)okObjectResult.Value;
            Assert.Equal(this.ExpectedPersonDto, personDto, Comparers.PersonDtoComparer());
        }

        private Person Person { get; } = 
            new Person(1, "Test", "Person", true, true, true)
            {
                FavouriteColours = new List<Colour>
                {
                    new Colour(1, "blue", true),
                    new Colour(2, "red", true)
                }
            };

        private PersonDto ExpectedPersonDto { get; } =
            new PersonDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Person",
                IsEnabled = true,
                IsValid = true,
                IsAuthorised = true,
                FavouriteColours = new List<ColourDto>
                {
                    new ColourDto
                    {
                        Id = 1,
                        Name = "blue",
                        IsEnabled = true
                    },
                    new ColourDto
                    {
                        Id = 2,
                        Name = "red",
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
                FavouriteColours = new List<int> { 1, 2 }
            };
    }
}