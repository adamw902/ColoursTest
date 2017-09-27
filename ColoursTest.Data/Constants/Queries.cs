using System.Collections.Generic;

namespace ColoursTest.Data.Constants
{
    public static class Queries
    {
        public static class ColourQueries
        {
            public const string SelectColours = "SELECT * FROM [Colours];";
            public const string SelectColour = "SELECT * FROM [Colours] WHERE ColourI = @ColourId";
        }

        public static class PersonQueries
        {
            public const string SelectPerson = "SELECT * FROM [People] WHERE PersonId = @PersonId";
            public const string SelectPersonColours = "SELECT C.* FROM [Colours] C INNER JOIN [FavouriteColours] FC ON C.ColourId = FC.ColourId WHERE FC.PersonId = @PersonId;";
            public const string SelectPeopleAndColours = "SELECT P.*, C.* FROM [People] P INNER JOIN [FavouriteColours] FC ON P.PersonId = FC.PersonId INNER JOIN [Colours] C ON FC.ColourId = C.ColourId";
            public const string UpdatePersonDetails = "UPDATE [People] SET IsEnabled = @IsEnabled, IsAuthorised = @IsAuthorised, IsValid = @IsValid WHERE PersonId = @PersonId;";
            public const string DeletePersonColours = "DELETE FROM [FavouriteColours] WHERE PersonId = @PersonId;";
            public const string InsertPersonColour = "INSERT INTO [FavouriteColours] (PersonId, ColourId) VALUES (@PersonId, @ColourId);";

            public static string InsertPersonColours(List<int> colourIds, int personId)
            {
                var query = string.Empty;
                foreach (var colourId in colourIds)
                {
                    var insertColour = InsertPersonColour.Replace("@PersonId", personId.ToString()).Replace("@ColourId", colourId.ToString());
                    query = $"{query}{insertColour}";
                }
                return query;
            }
        }
    }
}