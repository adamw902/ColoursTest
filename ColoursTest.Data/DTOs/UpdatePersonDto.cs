using System.Collections.Generic;

namespace ColoursTest.Data.DTOs
{
    public class UpdatePersonDto
    {
        public bool IsAuthorised { get; set; }
        public bool IsValid { get; set; }
        public bool IsEnabled { get; set; }
        public List<int> FavouriteColours { get; set; }
    }
}