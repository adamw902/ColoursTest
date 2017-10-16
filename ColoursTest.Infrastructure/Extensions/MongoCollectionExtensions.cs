using System;
using System.Threading.Tasks;
using ColoursTest.Domain.Interfaces;
using MongoDB.Driver;

namespace ColoursTest.Infrastructure.Extensions
{
    public static class MongoCollectionExtensions
    {
        public static async Task<Guid> Insert<T>(this IMongoCollection<T> collection, IEntity document)
        {
            var filter = Builders<T>.Filter.Eq("Id", document.Id);
            var colour = await collection.Find(filter).SingleOrDefaultAsync();
            if (colour != null)
            {
                document.Id = Guid.NewGuid();
            }

            await collection.InsertOneAsync((T)document);
            return document.Id;
        }
    }
}