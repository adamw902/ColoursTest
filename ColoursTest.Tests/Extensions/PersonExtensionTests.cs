using System;
using System.Collections.Generic;
using System.Linq;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;
using ColoursTest.Infrastructure.Extensions;
using ColoursTest.Tests.Shared.Comparers;
using Xunit;

namespace ColoursTest.Tests.Extensions
{
    public class PersonExtensionTests
    {
        [Fact]
        public void ToPersonDto_NullPerson_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => (null as Person).ToPersonDto());
        }

        [Fact]
        public void ToPersonDtoSingle_ValidPerson_MapsToValidPersonDto()
        {
            // Act
            var personDto = this.Person.ToPersonDto();

            // Assert
            Assert.Equal(this.ExpectedPersonDto, personDto, Comparers.PersonDtoComparer());
        }

        [Fact]
        public void ToPersonDtoCollection_ValidPersonCollection_MapsToValidPersonDtoCollection()
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

            // Act
            var peopleDto = people.ToPersonDto();

            // Assert
            Assert.Equal(expectedPeopleDto, peopleDto, Comparers.PersonDtoComparer());
        }

        private Person Person { get; } =
            new Person (1, "Test", "Person", true, true, true)
            {
                FavouriteColours = new List<Colour>()
            };

        private PersonDto ExpectedPersonDto { get; }
            = new PersonDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Person",
                IsAuthorised = true,
                IsEnabled = true,
                IsValid = true,
                FavouriteColours = new List<ColourDto>()
            };
    }
}
