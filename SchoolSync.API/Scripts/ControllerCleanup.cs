using System.Text.RegularExpressions;

namespace SchoolSync.API.Scripts;

public static class ControllerCleanup
{
    public static string RemoveTryCatchBlocks(string controllerContent)
    {
        // Remove try-catch blocks and keep only the content inside try block
        var tryPattern = @"try\s*\{([^}]*(?:\{[^}]*\}[^}]*)*)\}\s*catch\s*\([^)]*\)\s*\{[^}]*(?:\{[^}]*\}[^}]*)*\}(?:\s*catch\s*\([^)]*\)\s*\{[^}]*(?:\{[^}]*\}[^}]*)*\})*";

        // This regex finds try-catch blocks and replaces them with just the try content
        var result = Regex.Replace(controllerContent, tryPattern, "$1", RegexOptions.Singleline);

        // Clean up extra whitespace
        result = Regex.Replace(result, @"\n\s*\n\s*\n", "\n\n", RegexOptions.Multiline);

        return result;
    }
}
