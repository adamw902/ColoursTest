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
            var personDto = new PersonDto
            {
                PersonDetails = new PersonDetailsDto
                {
                    Id = person.PersonId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    IsEnabled = person.IsEnabled,
                    IsValid = person.IsValid,
                    IsAuthorised = person.IsAuthorised
                },
                FavouriteColours = person.FavouriteColours
            };
            return personDto;
        }

        public static List<PersonDto> ToPersonDto(this IEnumerable<Person> people)
        {
            return people.Select(person => person.ToPersonDto()).ToList();
        }
    }
}