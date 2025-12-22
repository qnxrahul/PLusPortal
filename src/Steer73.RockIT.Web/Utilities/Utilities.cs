

namespace Steer73.RockIT.Web.Utilities
{
    using Ganss.Xss;

    public static class Helpers
    {
        public static string SanitizeDescription(string input)
        {
            var sanitizer = new HtmlSanitizer();

            // Quill outputs list formatting using <ol>/<li data-list="bullet|ordered|checked"> and indentation via CSS classes (ql-indent-N).
            // We need to preserve these attributes/classes so multi-level bullets render correctly in both editor and viewer.
            var tagsToAllow = new[]
            {
                "p", "br",
                "strong", "em", "u",
                "h1", "h2", "h3",
                "ol", "ul", "li",
                "a", "span",
                "blockquote"
            };
            foreach (var tag in tagsToAllow)
                sanitizer.AllowedTags.Add(tag);

            var attributesToAllow = new[] { "href", "target", "rel", "class", "data-list" };
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