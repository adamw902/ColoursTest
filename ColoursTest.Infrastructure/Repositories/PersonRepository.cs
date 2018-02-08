using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Extensions;
using ColoursTest.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace ColoursTest.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        public PersonRepository(IMongoConnectionFactory mongoConnectionFactory)
        {
            this.Database = mongoConnectionFactory.GetDatabase();
        }

        private IMongoDatabase Database { get; }

        public async Task<IEnumerable<Person>> GetAll()
        {
            var people = await this.Database.GetCollection<Person>("people").Find(Builders<Person>.Filter.Empty).ToListAsync();
            return people ?? new List<Person>();
        }

        public async Task<Person> GetById(Guid id)
        {
            var filter = Builders<Person>.Filter.Eq("Id", id);
            var person = await this.Database.GetCollection<Person>("people").Find(filter).SingleOrDefaultAsync();

            return person;
        }

        public async Task<Person> Insert(Person person)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Can't create null person.");
            }

            person.Id = await this.Database.GetCollection<Person>("people").Insert(person);

            return person;
        }
        
        public async Task Update(Person person)
        {
            if (person == null || person.Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(person), "Can't update null person.");
            }
            
            var filter = Builders<Person>.Filter.Eq(p => p.Id, person.Id);

            await this.Database.GetCollection<Person>("people").ReplaceOneAsync(filter, person);
        }
    }
}