using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using ColoursTest.Data.Constants;
using ColoursTest.Data.DTOs;
using ColoursTest.Data.Interfaces;
using ColoursTest.Data.Models;
using Dapper;

namespace ColoursTest.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        public IEnumerable<Person> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(SystemVariables.ConnectionString))
            {
                var results = connection
                    .Query<Person, Colour, Person>(Queries.PersonQueries.SelectPeopleAndColours,
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
            using (IDbConnection connection = new SqlConnection(SystemVariables.ConnectionString))
            {
                var person = connection.Query<Person>(Queries.PersonQueries.SelectPerson, new {PersonId = personId.ToString()}).SingleOrDefault();
                if (person == null)
                {
                    return null;
                }

                var colours = connection.Query<Colour>(Queries.PersonQueries.SelectPersonColours, new {PersonId = personId.ToString()}).ToList();
                person.FavouriteColours = colours;
                return person;
            }
        }

        public Person Insert(Person person)
        {
            throw new System.NotImplementedException();
        }

        public Person Update(int personId, UpdatePersonDto updatePersonDto)
        {
            using (IDbConnection connection = new SqlConnection(SystemVariables.ConnectionString))
            {
                Person person = GetById(personId);

                if (person == null)
                {
                    return null;
                }

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(Queries.PersonQueries.UpdatePersonDetails, new {updatePersonDto.IsEnabled, updatePersonDto.IsAuthorised, updatePersonDto.IsValid, personId}, transaction);
                        connection.Execute(Queries.PersonQueries.DeletePersonColours, new { personId }, transaction);
                        connection.Execute(Queries.PersonQueries.InsertPersonColours(updatePersonDto.FavouriteColours, personId), null, transaction);
                        transaction.Commit();
                        connection.Close();
                    }
                    catch (SqlException ex)
                    {
                        Debug.WriteLine(ex.Message);
                        transaction.Rollback();
                        return null;
                    }
                }
                var updatedPerson = GetById(personId);
                return updatedPerson;
            }
        }
    }
}