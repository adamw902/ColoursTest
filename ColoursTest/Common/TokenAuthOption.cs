using System;
using Microsoft.IdentityModel.Tokens;

namespace ColoursTest.Web.Common
{
    public class TokenAuthOption
    {
        public static string Audience { get; } = "localhost:52621";
        public static string Issuer { get; } = "localhost:52621";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RsaKeyHelper.GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromMinutes(15);
        public static string TokenType { get; } = "Bearer";
    }
}