using System;

namespace ColoursTest.Infrastructure.DTOs
{
    public class LoginSuccess
    {
        public DateTime RequestAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; }
        public string Token { get; set; }
    }
}