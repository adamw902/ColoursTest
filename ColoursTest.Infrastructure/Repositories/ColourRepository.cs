using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColoursTest.Domain.Interfaces;
using ColoursTest.Domain.Models;
using ColoursTest.Infrastructure.Interfaces;
using Dapper;

namespace ColoursTest.Infrastructure.Repositories
{
    public class ColourRepository : IColourRepository
    {
        public ColourRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.DbConnectionFactory = dbConnectionFactory;
        }

        private IDbConnectionFactory DbConnectionFactory { get; }

        public async Task<IEnumerable<Colour>> GetAll()
        {
            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                return await connection.QueryAsync<Colour>("SELECT * FROM [Colours];");
            }
        }

        public async Task<Colour> GetById(int colourId)
        {
            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                var selectColour = $"SELECT * FROM [Colours] WHERE ColourId = {colourId};";
                return await connection.QuerySingleOrDefaultAsync<Colour>(selectColour);
            }
        }

        public async Task<Colour> Insert(Colour colour)
        {
            if (colour == null)
            {
                throw new ArgumentNullException(nameof(colour), "Can't create null colour.");
            }

            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                var insertColour = @"INSERT INTO [Colours] (Name, IsEnabled) VALUES (@Name, @IsEnabled);
                                     SELECT CAST(SCOPE_IDENTITY() as int);";
                colour.ColourId = await connection.QuerySingleAsync<int>(insertColour, colour);
                return colour;
            }
        }

        public async Task<Colour> Update(Colour colour)
        {
            if (colour == null || colour.ColourId == 0)
            {
                throw new ArgumentNullException(nameof(colour), "Can't update null colour.");
            }

            using (var connection = this.DbConnectionFactory.GetConnection())
            {
                var updateColour = $@"
                        UPDATE [Colours]
                           SET Name = @Name, 
                               IsEnabled = @IsEnabled
                         WHERE ColourId = {colour.ColourId};";

                await connection.ExecuteAsync(updateColour, colour);
            }
            return colour;
        }
    }
}