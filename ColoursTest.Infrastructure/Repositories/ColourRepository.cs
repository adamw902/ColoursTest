using System;
using System.Collections.Generic;
using System.Linq;
using ColoursTest.Domain.Exceptions;
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
                return connection.Query<Colour>("SELECT * FROM [Colours];");
            }
        }

        public Colour GetById(int colourId)
        {
            using (var connection = this.ConnectionFactory.GetConnection())
            {
                var selectColour = "SELECT * FROM [Colours] WHERE ColourId = @ColourId;";
                var colour = connection.Query<Colour>(selectColour, new {ColourId = colourId}).SingleOrDefault();
                if (colour == null)
                {
                    throw new IncorrectIdException("Colour does not exist with the given id.");
                }
                return colour;
            }
        }

        public Colour Insert(Colour colour)
        {
            if (colour == null)
            {
                throw new IncorrectFormatException("Can't create null colour.");
            }

            using (var connection = this.ConnectionFactory.GetConnection())
            {
                var insertColour = @"INSERT INTO [Colours] (Name, IsEnabled) VALUES (@Name, @IsEnabled);
                                     SELECT CAST(SCOPE_IDENTITY() as int);";
                colour.ColourId = connection.Query<int>(insertColour, colour).Single();
                return colour;
            }
        }

        public Colour Update(Colour colour)
        {
            if (colour == null)
            {
                throw new IncorrectFormatException("Can't update null colour.");
            }
            this.GetById(colour.ColourId);

            using (var connection = this.ConnectionFactory.GetConnection())
            {
                var updateColour = @"UPDATE [Colours]
                                     SET Name = @Name, IsEnabled = @IsEnabled
                                     WHERE ColourId = @ColourId;";
                connection.Execute(updateColour, colour);
            }
            return colour;
        }
    }
}