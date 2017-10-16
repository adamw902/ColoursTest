using System.Collections.Generic;
using System.IO;
using ColoursTest.Domain.Models;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ColoursTest.Tests.Repositories
{
    public static class DatabaseSetup
    {
        public static void Setup(string databaseName)
        {
            var coloursData = JsonConvert.DeserializeObject<IEnumerable<Colour>>(File.ReadAllText("Repositories/DBScripts/InsertColoursData.txt"));
            var peopleData = JsonConvert.DeserializeObject<IEnumerable<Person>>(File.ReadAllText("Repositories/DBScripts/InsertPeopleData.txt"));

            var database = new MongoClient("mongodb://localhost:27017").GetDatabase(databaseName);

            database.GetCollection<Colour>("colours").InsertMany(coloursData);
            database.GetCollection<Person>("people").InsertMany(peopleData);
        }

        public static void TearDown(string databaseName)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            client.DropDatabase(databaseName);
        }
    }
}