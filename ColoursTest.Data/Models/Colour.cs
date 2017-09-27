using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ColoursTest.Data.Models
{
    public class Colour
    {
        [Key]
        public int ColourId { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
