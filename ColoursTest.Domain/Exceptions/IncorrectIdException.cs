using System;

namespace ColoursTest.Domain.Exceptions
{
    public class IncorrectIdException : Exception
    {
        public IncorrectIdException(string message)
        {
            this.CustomMessage = message;
        }

        public string CustomMessage { get; }
    }
}