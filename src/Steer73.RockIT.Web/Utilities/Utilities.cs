

namespace Steer73.RockIT.Web.Utilities
{
    using Ganss.Xss;

    public static class Helpers
    {
        public static string SanitizeDescription(string input)
        {
            var sanitizer = new HtmlSanitizer();

            var tagsToAllow = new[] { "p", "br", "strong", "em", "u", "ol", "ul", "li", "a", "span", "blockquote" };
            foreach (var tag in tagsToAllow)
                sanitizer.AllowedTags.Add(tag);

            var attributesToAllow = new[] { "href", "target", "rel", "class" };
            foreach (var attr in attributesToAllow)
                sanitizer.AllowedAttributes.Add(attr);

            sanitizer.AllowedSchemes.Add("mailto");
            sanitizer.AllowedSchemes.Add("tel");

            for (int i = 1; i <= 10; i++)
                sanitizer.AllowedClasses.Add($"ql-indent-{i}");

            sanitizer.AllowedClasses.Add("ql-align-center");
            sanitizer.AllowedClasses.Add("ql-align-right");
            sanitizer.AllowedClasses.Add("ql-align-justify");

            var sanitized = sanitizer.Sanitize(input ?? string.Empty);

            // Remove empty <span contenteditable="false"></span>
            sanitized = System.Text.RegularExpressions.Regex.Replace(
                sanitized,
                @"<span contenteditable=""false""></span>",
                string.Empty,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            return sanitized;
        }

    }
}