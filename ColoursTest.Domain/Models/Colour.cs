using System;
using ColoursTest.Domain.Interfaces;

namespace ColoursTest.Domain.Models
{
    public class Colour : IEntity
    {
        public Colour(Guid id, string name, bool isEnabled)
        {
            this.Id = id;
            this.Name = name;
            this.IsEnabled = isEnabled;
        }
        
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
