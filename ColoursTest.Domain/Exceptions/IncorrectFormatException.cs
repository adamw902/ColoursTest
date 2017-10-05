using System;

namespace ColoursTest.Domain.Exceptions
{
    public class IncorrectFormatException : Exception
    {
        public IncorrectFormatException(string message)
        {
            this.CustomMessage = message;
        }

        public string CustomMessage { get; set; }
    }
}