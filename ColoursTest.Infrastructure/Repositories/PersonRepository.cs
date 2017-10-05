using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using Dapper;

namespace ColoursTest.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        public PersonRepository(IConnectionFactory connectionFactory)
        {
            this.ConnectionFactory = connectionFactory;
        }

        private IConnectionFactory ConnectionFactory { get; }

        public IEnumerable<Person> GetAll()
        {
            using (var connection = this.ConnectionFactory.GetConnection())
            {
                var selectPeopleAndColours = @"SELECT P.*, C.* FROM [People] P
                                               INNER JOIN [FavouriteColours] FC
                                               ON P.PersonId = FC.PersonId
                                               INNER JOIN [Colours] C
                                               ON FC.ColourId = C.ColourId;";
                var results = connection
                    .Query<Person, Colour, Person>(selectPeopleAndColours,
                    (person, colour) =>
                    {
                        person.FavouriteColours = person.FavouriteColours ?? new List<Colour>();
                        person.FavouriteColours.Add(colour);
                        return person;
                    }, splitOn: "PersonId, ColourId")
                    .GroupBy(r => r.PersonId).Select(group =>
                    {
                        var groupedPerson = group.First();
                        groupedPerson.FavouriteColours = group.Select(c => c.FavouriteColours.Single()).ToList();
                        return groupedPerson;
                    }
                );
                return results;
            }
        }

        public Person GetById(int personId)
        {
            using (var connection = this.ConnectionFactory.GetConnection())
            {
                var selectPerson = "SELECT * FROM [People] WHERE PersonId = @PersonId;";
                var person = connection.Query<Person>(selectPerson, new {PersonId = personId.ToString()}).SingleOrDefault();
                if (person == null)
                {
                    throw new Exception("No person found with the given id");
                }

                var selectPersonColours = @"SELECT C.* FROM [Colours] C 
                                            INNER JOIN [FavouriteColours] FC 
                                            ON C.ColourId = FC.ColourId 
                                            WHERE FC.PersonId = @PersonId;";
                var colours = connection.Query<Colour>(selectPersonColours, new {PersonId = personId.ToString()}).ToList();
                person.FavouriteColours = colours;
                return person;
            }
        }

        public Person Insert(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Can't create null person.");
            }

            using (var connection = this.ConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var insertPerson = @"INSERT INTO [People] (FirstName, LastName, IsAuthorised, IsValid, IsEnabled)
                                             VALUES(@FirstName, @LastName, @IsAuthorised, @IsValid, @IsEnabled);
                                             SELECT CAST(SCOPE_IDENTITY() as int);";
                        person.PersonId = connection.Query<int>(insertPerson, person, transaction).Single();
                        
                        this.InsertColours(person.PersonId, person.FavouriteColours, connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        throw new Exception("Failed to save person's favourite colours, one or more ColourId's may be incorrect.");
                    }
                }
                return person;
            }
        }


        public Person Update(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Can't update null person.");
            }

            using (var connection = this.ConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updatePersonDetails = @"UPDATE [People] 
                                                    SET FirstName = @FirstName, LastName = @LastName,
                                                        IsAuthorised = @IsAuthorised, IsValid = @IsValid,
                                                        IsEnabled = @IsEnabled
                                                    WHERE PersonId = @PersonId;";
                        connection.Execute(updatePersonDetails, person, transaction);

                        var deletePersonColours = "DELETE FROM [FavouriteColours] WHERE PersonId = @PersonId;";
                        connection.Execute(deletePersonColours, new { person.PersonId }, transaction);

                        this.InsertColours(person.PersonId, person.FavouriteColours, connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        throw new Exception("Failed to save person's favourite colours, one or more ColourId's may be incorrect.");
                    }
                }
                return person;
            }
        }

        private void InsertColours(int personId, IEnumerable<Colour> colours, IDbConnection connection, IDbTransaction transaction)
        {
            var insertFavouriteColour = "INSERT INTO [FavouriteColours] (PersonId, ColourId) VALUES (@PersonId, @ColourId);";
            foreach (var colour in colours)
            {
                connection.Execute(insertFavouriteColour, new { personId, colour.ColourId }, transaction);
            }
        }
    }
}