using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using Dapper;

namespace ColoursTest.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        public PersonRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.DbConnectionFactory = dbConnectionFactory;
        }

        private IDbConnectionFactory DbConnectionFactory { get; }

        public async Task<IEnumerable<Person>> GetAll()
        {
            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                var selectPeopleAndColours = @"
                            SELECT P.*, 
                                   C.*
                              FROM [People] P
                        INNER JOIN [FavouriteColours] FC
                                ON P.PersonId = FC.PersonId
                        INNER JOIN [Colours] C
                                ON FC.ColourId = C.ColourId;";

                var results = await connection.QueryAsync<Person, Colour, Person>(selectPeopleAndColours,
                    (person, colour) =>
                    {
                        person.FavouriteColours = person.FavouriteColours ?? new List<Colour>();
                        person.FavouriteColours.Add(colour);
                        return person;
                    }, splitOn: "PersonId, ColourId");

                var groupedResults = results.GroupBy(r => r.PersonId)
                    .Select(group =>
                        {
                            var groupedPerson = group.First();
                            groupedPerson.FavouriteColours = group.Select(c => c.FavouriteColours.Single()).ToList();
                            return groupedPerson;
                        }
                    );
                return groupedResults;
            }
        }

        public async Task<Person> GetById(int personId)
        {
            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                var selectPerson = $"SELECT * FROM [People] WHERE PersonId = {personId};";
                var person = (await connection.QueryAsync<Person>(selectPerson)).SingleOrDefault();

                if (person == null)
                {
                    return null;
                }

                var selectPersonColours = $@"
                            SELECT C.*
                              FROM [Colours] C 
                        INNER JOIN [FavouriteColours] FC 
                                ON C.ColourId = FC.ColourId 
                             WHERE FC.PersonId = {personId};";

                var colours = await connection.QueryAsync<Colour>(selectPersonColours);
                person.FavouriteColours = (IList<Colour>) colours;

                return person;
            }
        }

        public async Task<Person> Insert(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Can't create null person.");
            }

            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var insertPerson = @"
                            INSERT INTO [People] (FirstName, LastName, IsAuthorised, IsValid, IsEnabled)
                                 VALUES (@FirstName, @LastName, @IsAuthorised, @IsValid, @IsEnabled);
                                 SELECT CAST(SCOPE_IDENTITY() as int);";

                    person.PersonId = (await connection.QueryAsync<int>(insertPerson, person, transaction)).Single();

                    var insertFavouriteColours =
                        person.FavouriteColours
                            .Aggregate(string.Empty, (current, colour) =>
                                $"{current}INSERT INTO[FavouriteColours] (PersonId, ColourId) VALUES({person.PersonId}, {colour.ColourId});");

                    await connection.ExecuteAsync(insertFavouriteColours, null, transaction);
                    transaction.Commit();
                }
                return person;
            }
        }


        public async Task<Person> Update(Person person)
        {
            if (person == null || person.PersonId == 0)
            {
                throw new ArgumentNullException(nameof(person), "Can't update null person.");
            }

            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    var updatePerson = $@"
                            UPDATE [People] 
                               SET FirstName = @FirstName, 
                                   LastName = @LastName, 
                                   IsAuthorised = @IsAuthorised, 
                                   IsValid = @IsValid,
                                   IsEnabled = @IsEnabled
                             WHERE PersonId = {person.PersonId};";

                    var deletePersonColours = $"DELETE FROM [FavouriteColours] WHERE PersonId = {person.PersonId};";

                    var insertFavouriteColours =
                        person.FavouriteColours
                            .Aggregate(string.Empty, (current, colour) =>
                                $"{current}INSERT INTO[FavouriteColours] (PersonId, ColourId) VALUES({person.PersonId}, {colour.ColourId});");

                    updatePerson = $"{updatePerson}{deletePersonColours}{insertFavouriteColours}";

                    await connection.ExecuteAsync(updatePerson, person, transaction);

                    transaction.Commit();
                }
                return person;
            }
        }
    }
}