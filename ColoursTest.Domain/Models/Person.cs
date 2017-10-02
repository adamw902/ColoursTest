using System.Collections.Generic;

namespace ColoursTest.Domain.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAuthorised { get; set; }
        public bool IsValid { get; set; }
        public bool IsEnabled { get; set; }
        
        public List<Colour> FavouriteColours { get; set; }
    }
}