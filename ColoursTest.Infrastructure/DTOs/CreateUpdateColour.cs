using System.ComponentModel.DataAnnotations;

namespace ColoursTest.Infrastructure.DTOs
{
    public class CreateUpdateColour
    {
        [Required]
        public string Name { get; set; }

        public bool? IsEnabled { get; set; }
    }
}