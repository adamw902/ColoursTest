using System;
using System.Collections.Generic;
using System.Linq;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.Infrastructure.Extensions
{
    public static class PersonExtension
    {
        public static PersonDto ToPersonDto(this Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Cannot map null person.");
            }

            var personDto = new PersonDto
            {
                Id = person.PersonId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                IsEnabled = person.IsEnabled,
                IsValid = person.IsValid,
                IsAuthorised = person.IsAuthorised,
                FavouriteColours = person.FavouriteColours.ToColourDto()
            };

            return personDto;
        }

        public static IEnumerable<PersonDto> ToPersonDto(this IEnumerable<Person> people)
        {
            return people.Select(person => person.ToPersonDto()).ToList();
        }
    }
}