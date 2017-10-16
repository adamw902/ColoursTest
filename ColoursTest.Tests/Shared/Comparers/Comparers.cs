using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.DTOs;

namespace ColoursTest.Tests.Shared.Comparers
{
    public static class Comparers
    {
        public static IEqualityComparer<PersonDto> PersonDtoComparer()
        {
            return new GenericComparer<PersonDto>(
                (x, y) => x.Id == y.Id,
                (x, y) => string.Equals(x.FirstName, y.FirstName),
                (x, y) => string.Equals(x.LastName, y.LastName),
                (x, y) => x.IsEnabled == y.IsEnabled,
                (x, y) => x.IsAuthorised == y.IsAuthorised,
                (x, y) => x.IsValid == y.IsValid,
                (x, y) => x.FavouriteColours.SequenceEqual(y.FavouriteColours, ColourDtoComparer())
            );
        }

        public static IEqualityComparer<ColourDto> ColourDtoComparer()
        {
            return new GenericComparer<ColourDto>(
                (x, y) => x.Id == y.Id,
                (x, y) => string.Equals(x.Name, y.Name),
                (x, y) => x.IsEnabled == y.IsEnabled
            );
        }

        public static IEqualityComparer<IEnumerable<Colour>> ColoursComparer()
        {
            return new GenericComparer<IEnumerable<Colour>>(
                (x, y) => x.SequenceEqual(y, ColourComparer())
            );
        }

        public static IEqualityComparer<Colour> ColourComparer()
        {
            return new GenericComparer<Colour>(
                (x, y) => x.Id == y.Id,
                (x, y) => string.Equals(x.Name, y.Name),
                (x, y) => x.IsEnabled == y.IsEnabled
            );
        }

        public static IEqualityComparer<Colour> ColourWithNewIdComparer()
        {
            return new GenericComparer<Colour>(
                (x, y) => x.Id != Guid.Empty,
                (x, y) => string.Equals(x.Name, y.Name),
                (x, y) => x.IsEnabled == y.IsEnabled
            );
        }

        public static IEqualityComparer<Person> PersonComparer()
        {
            return new GenericComparer<Person>(
                (x, y) => x.Id == y.Id,
                (x, y) => string.Equals(x.FirstName, y.FirstName),
                (x, y) => string.Equals(x.LastName, y.LastName),
                (x, y) => x.IsEnabled == y.IsEnabled,
                (x, y) => x.IsAuthorised == y.IsAuthorised,
                (x, y) => x.IsValid == y.IsValid,
                (x, y) => x.FavouriteColours.SequenceEqual(y.FavouriteColours, ColourComparer())
            );
        }

        public static IEqualityComparer<Person> PersonWithNewIdComparer()
        {
            return new GenericComparer<Person>(
                (x, y) => x.Id != Guid.Empty,
                (x, y) => string.Equals(x.FirstName, y.FirstName),
                (x, y) => string.Equals(x.LastName, y.LastName),
                (x, y) => x.IsEnabled == y.IsEnabled,
                (x, y) => x.IsAuthorised == y.IsAuthorised,
                (x, y) => x.IsValid == y.IsValid,
                (x, y) => x.FavouriteColours.SequenceEqual(y.FavouriteColours, ColourComparer())
            );
        }

        public static IEqualityComparer<CreateUpdateColour> CreateUpdateColourComparer()
        {
            return new GenericComparer<CreateUpdateColour>(
                (x, y) => string.Equals(x.Name, y.Name),
                (x, y) => x.IsEnabled == y.IsEnabled
            );
        }

        public static IEqualityComparer<CreateUpdatePerson> CreateUpdatePersonComparer()
        {
            return new GenericComparer<CreateUpdatePerson>(
                (x, y) => string.Equals(x.FirstName, y.FirstName),
                (x, y) => string.Equals(x.LastName, y.LastName),
                (x, y) => x.IsEnabled == y.IsEnabled,
                (x, y) => x.IsAuthorised == y.IsAuthorised,
                (x, y) => x.IsValid == y.IsValid,
                (x, y) => x.FavouriteColours.SequenceEqual(y.FavouriteColours)
            );
        }
    }
}