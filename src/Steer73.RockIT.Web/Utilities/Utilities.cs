using Ganss.Xss;
using System.Collections.Generic;

namespace Steer73.RockIT.Web.Utilities
{
    public static class Helpers
    {
        public static string SanitizeDescription(string input)
        {
            // Configure sanitizer to preserve Quill-generated markup (links, lists, indents)
            var sanitizer = new HtmlSanitizer();

            // Allow tags commonly produced by our Quill configuration
            var tagsToAllow = new[]
            {
                "a", "p", "br", "strong", "em", "u", "ol", "ul", "li",
                "h1", "h2", "h3", "blockquote", "span"
            };
            foreach (var tag in tagsToAllow)
            {
                sanitizer.AllowedTags.Add(tag);
            }

            // Allow attributes needed for links and Quill classes/metadata
            var attributesToAllow = new[] { "href", "target", "rel", "class", "data-list", "data-indent", "data-checked", "dir" };
            foreach (var attr in attributesToAllow)
            {
                sanitizer.AllowedAttributes.Add(attr);
            }

            // Allow mail and telephone links
            sanitizer.AllowedSchemes.Add("mailto");
            sanitizer.AllowedSchemes.Add("tel");

            // Whitelist Quill CSS classes used for indentation/alignment
            var allowedClasses = new List<string>();
            for (var i = 1; i <= 10; i++)
            {
                allowedClasses.Add($"ql-indent-{i}");
            }
            allowedClasses.AddRange(new[]
            {
                "ql-align-center", "ql-align-right", "ql-align-justify"
            });
            foreach (var cls in allowedClasses)
            {
                sanitizer.AllowedClasses.Add(cls);
            }

            return sanitizer.Sanitize(input ?? string.Empty);
        }
    }
}
