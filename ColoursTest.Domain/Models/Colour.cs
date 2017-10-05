using System;
using System.ComponentModel.DataAnnotations;
using ColoursTest.Domain.Exceptions;

namespace ColoursTest.Domain.Models
{
    public class Colour
    {
        public Colour() { }
        public Colour(string name, bool? isEnabled)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IncorrectFormatException("Name must have a value.");
            }
            this.Name = name;
            this.IsEnabled = isEnabled ?? false;
        }

        [Key]
        public int ColourId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
