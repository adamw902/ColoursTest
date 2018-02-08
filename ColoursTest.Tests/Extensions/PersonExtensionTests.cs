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
            // Arrange
            var expectedPersonDto = new PersonDto
            {
                Id = Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE"),
                FirstName = "Test",
                LastName = "Person",
                IsAuthorised = true,
                IsEnabled = true,
                IsValid = true,
                FavouriteColours = new List<ColourDto>()
            };

            // Act
            var personDto = this.Person.ToPersonDto();

            // Assert
            Assert.Equal(expectedPersonDto, personDto, Comparers.PersonDtoComparer());
        }

        [Fact]
        public void ToPersonDtoCollection_ValidPersonCollection_MapsToValidPersonDtoCollection()
        {
            // Arange
            var people = new List<Person>
            {
                this.Person
            }.AsEnumerable();

            var expectedPeopleDto = new List<PersonDto>
            {
                new PersonDto
                {
                    Id = Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE"),
                    FirstName = "Test",
                    LastName = "Person",
                    IsAuthorised = true,
                    IsValid = true,
                    IsEnabled = true,
                    FavouriteColours = new List<ColourDto>()
                }
            }.AsEnumerable();

            // Act
            var peopleDto = people.ToPersonDto();

            // Assert
            Assert.Equal(expectedPeopleDto, peopleDto, Comparers.PersonDtoComparer());
        }

        private Person Person { get; } = new Person (Guid.Parse("51724787-A908-45CD-ABAA-EF4DA771F9EE"), "Test", "Person", true, true, true, new List<Guid>())
        {
            FavouriteColours = new List<Colour>()
        };
    }
}
