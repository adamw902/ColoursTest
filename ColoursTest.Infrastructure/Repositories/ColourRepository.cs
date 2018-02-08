using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Extensions;
using ColoursTest.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace ColoursTest.Infrastructure.Repositories
{
    public class ColourRepository : IColourRepository
    {
        public ColourRepository(IMongoConnectionFactory mongoConnectionFactory)
        {
            this.Database = mongoConnectionFactory.GetDatabase();
        }

        private IMongoDatabase Database { get; }

        public async Task<IEnumerable<Colour>> GetAll()
        {
            var colours = await this.Database.GetCollection<Colour>("colours").Find(Builders<Colour>.Filter.Empty).ToListAsync();
            return colours ?? new List<Colour>();
        }

        public Task<Colour> GetById(Guid id)
        {
            var filter = Builders<Colour>.Filter.Eq("Id", id);
            return this.Database.GetCollection<Colour>("colours").Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Colour>> GetByIds(IEnumerable<Guid> ids)
        {
            var filter = Builders<Colour>.Filter.Where(c => ids.Contains(c.Id));
            var colours = await this.Database.GetCollection<Colour>("colours").Find(filter).ToListAsync();
            return colours ?? new List<Colour>();
        }

        public async Task<Colour> Insert(Colour colour)
        {
            if (colour == null)
            {
                throw new ArgumentNullException(nameof(colour), "Can't create null colour.");
            }

            colour.Id = await this.Database.GetCollection<Colour>("colours").Insert(colour);

            return colour;
        }

        public Task Update(Colour colour)
        {
            if (colour == null || colour.Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(colour), "Can't update null colour.");
            }

            var filter = Builders<Colour>.Filter.Eq(s => s.Id, colour.Id);

            return this.Database.GetCollection<Colour>("colours").ReplaceOneAsync(filter, colour);
        }
    }
}