using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ColoursTest.Data.Constants;
using ColoursTest.Data.Interfaces;
using ColoursTest.Data.Models;
using Dapper;

namespace ColoursTest.Data.Repositories
{
    public class ColourRepository : IColourRepository
    {
        public IEnumerable<Colour> GetAll()
        {
            using (IDbConnection connection = new SqlConnection(SystemVariables.ConnectionString))
            {
                var colours = connection.Query<Colour>(Queries.ColourQueries.SelectColours);
                return colours;
            }
        }

        public Colour GetById(int colourId)
        {
            using (IDbConnection connection = new SqlConnection(SystemVariables.ConnectionString))
            {
                var colour = connection.Query<Colour>(Queries.ColourQueries.SelectColour, new {ColourId = colourId});
                return colour.SingleOrDefault();
            }
        }

        public Colour Insert(Colour item)
        {
            throw new NotImplementedException();
        }

        public void Update(Colour item)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
