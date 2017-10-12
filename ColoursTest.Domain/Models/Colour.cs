using System;

namespace ColoursTest.Domain.Models
{
    public class Colour
    {
        public Colour(int colourId, string name, bool isEnabled)
        {
            this.ColourId = colourId;
            this.Name = name;
            this.IsEnabled = isEnabled;
        }
        
        public int ColourId { get; set; }
        
        public string Name { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}
