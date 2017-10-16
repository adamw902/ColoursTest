using System;

namespace ColoursTest.Infrastructure.DTOs
{
    public class ColourDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}