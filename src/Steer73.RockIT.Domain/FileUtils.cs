using System.Collections.Generic;

namespace Steer73.RockIT;

/// <summary>
/// Helper methods for files
/// </summary>
public static class FileUtils
{
    /// <summary>
    /// Gets content type (mime type) from file extension
    /// </summary>
    /// <param name="fileExtension"></param>
    /// <returns>Content type if <paramref name="fileExtension"/> is valid </returns>
    public static string GetContentType(string fileExtension)
    {
        if (string.IsNullOrWhiteSpace(fileExtension)) { return ""; }

        var extensionToMimeTypeMap = new Dictionary<string, string>
        {
            { ".csv", "text/csv" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".pdf", "application/pdf" },
            { ".txt", "text/plain" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
        };

        var extension = fileExtension.ToLowerInvariant();
        if (!extension.StartsWith("."))
        {
            extension = $".{extension}";
        }

        return extensionToMimeTypeMap.ContainsKey(extension) ?
            extensionToMimeTypeMap[extension] : "application/octet-stream";
    }
}
