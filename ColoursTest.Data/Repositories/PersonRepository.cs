using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using Dapper;

namespace ColoursTest.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private IConnectionFactory ConnectionFactory { get; }

        public PersonRepository(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public IEnumerable<Person> GetAll()
        {
            using (var connection = ConnectionFactory.GetConnection())
            {
                string selectPeopleAndColours = @"SELECT P.*, C.* FROM [People] P
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
            using (IDbConnection connection = ConnectionFactory.GetConnection())
            {
                var selectPerson = "SELECT * FROM [People] WHERE PersonId = @PersonId;";
                var person = connection.Query<Person>(selectPerson, new {PersonId = personId.ToString()}).SingleOrDefault();
                if (person == null)
                {
                    return null;
                }

                var selectPersonColours = "SELECT C.* FROM [Colours] C INNER JOIN [FavouriteColours] FC ON C.ColourId = FC.ColourId WHERE FC.PersonId = @PersonId;";
                var colours = connection.Query<Colour>(selectPersonColours, new {PersonId = personId.ToString()}).ToList();
                person.FavouriteColours = colours;
                return person;
            }
        }

        public Person Insert(Person person)
        {
            throw new System.NotImplementedException();
        }


        public Person Update(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Cant update null person");
            }

            using (IDbConnection connection = this.ConnectionFactory.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updatePersonDetails = "UPDATE [People] SET IsEnabled = @IsEnabled, IsAuthorised = @IsAuthorised, IsValid = @IsValid WHERE PersonId = @PersonId;";
                        connection.Execute(updatePersonDetails,
                            new
                            {
                                person.IsEnabled,
                                person.IsAuthorised,
                                person.IsValid,
                                person.PersonId
                            }, transaction);
                        var deletePersonColours = "DELETE FROM [FavouriteColours] WHERE PersonId = @PersonId;";
                        connection.Execute(deletePersonColours, new { person.PersonId }, transaction);

                        var insertPersonColour = "INSERT INTO [FavouriteColours] (PersonId, ColourId) VALUES (@PersonId, @ColourId);";
                        foreach (var favouriteColour in person.FavouriteColours)
                        {
                            connection.Execute(insertPersonColour, new { person.PersonId, favouriteColour.ColourId }, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        //todo: log this
                        throw new Exception("Attempted to update person who does not exist.");
                    }
                }
                var updatedPerson = GetById(person.PersonId);
                return updatedPerson;
            }
        }
    }
}