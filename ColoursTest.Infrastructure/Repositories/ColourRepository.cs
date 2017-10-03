using System;
using System.Collections.Generic;
using System.Linq;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using Dapper;

namespace ColoursTest.Infrastructure.Repositories
{
    public class ColourRepository : IColourRepository
    {
        public ColourRepository(IConnectionFactory connectionFactory)
        {
            this.ConnectionFactory = connectionFactory;
        }

        private IConnectionFactory ConnectionFactory { get; }

        public IEnumerable<Colour> GetAll()
        {
            using (var connection = this.ConnectionFactory.GetConnection())
            {
                var colours = connection.Query<Colour>("SELECT * FROM [Colours];");
                return colours;
            }
        }

        public Colour GetById(int colourId)
        {
            using (var connection = this.ConnectionFactory.GetConnection())
            {
                var colour = connection.Query<Colour>("SELECT * FROM [Colours] WHERE ColourId = @ColourId;", new {ColourId = colourId});
                return colour.SingleOrDefault();
            }
        }

        public Colour Insert(Colour item)
        {
            throw new NotImplementedException();
        }

        public Colour Update(Colour item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}