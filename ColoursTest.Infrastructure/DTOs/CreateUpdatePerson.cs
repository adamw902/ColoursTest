using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ColoursTest.Infrastructure.DTOs
{
    public class CreateUpdatePerson
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool? IsAuthorised { get; set; }
        public bool? IsValid { get; set; }
        public bool? IsEnabled { get; set; }
        public List<int> FavouriteColours { get; set; }
    }
}