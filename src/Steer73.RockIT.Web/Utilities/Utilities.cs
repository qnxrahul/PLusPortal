using Ganss.Xss;

namespace Steer73.RockIT.Web.Utilities
{
    public static class Helpers
    {
        public static string SanitizeDescription(string input)
        {
            // The default sanitization done is good enough. See details here: https://github.com/mganss/HtmlSanitizer
            var sanitizer = new HtmlSanitizer();
            
            return sanitizer.Sanitize(input);
        }
    }
}
