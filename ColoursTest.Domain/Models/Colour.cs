using System;
using System.ComponentModel.DataAnnotations;

namespace ColoursTest.Domain.Models
{
    public class Colour
    {
        public Colour() { }
        public Colour(string name, bool? isEnabled)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Name must have a value.");
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
