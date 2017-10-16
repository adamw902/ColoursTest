using System.Collections.Generic;
using System.IO;
using ColoursTest.Domain.Models;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace DatabaseMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var coloursData = JsonConvert.DeserializeObject<IEnumerable<Colour>>(File.ReadAllText("DBScripts/InsertColoursData.txt"));
            var peopleData = JsonConvert.DeserializeObject<IEnumerable<Person>>(File.ReadAllText("DBScripts/InsertPeopleData.txt"));

            var database = new MongoClient("mongodb://localhost:27017").GetDatabase("ColoursTest");

            database.GetCollection<Colour>("colours").InsertMany(coloursData);
            database.GetCollection<Person>("people").InsertMany(peopleData);
        }
    }
}
