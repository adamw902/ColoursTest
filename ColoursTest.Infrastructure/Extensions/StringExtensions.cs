using System.Linq;

namespace ColoursTest.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string text)
        {
            var forwardText = text.Trim().Replace(" ", "").ToLower();
            var reversedText = string.Join("", forwardText.Reverse());
            return forwardText.Equals(reversedText);
        }
    }
}