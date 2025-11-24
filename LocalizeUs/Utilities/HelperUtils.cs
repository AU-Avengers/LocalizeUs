
using System.Text.RegularExpressions;

namespace LocalizeUs.Utilities;

public static class HelperUtils
{
    /// <summary>
    /// Gets a proper string for an enum. (with spaces).
    /// </summary>
    /// <param name="enum">The enum you would like to change.</param>
    /// <returns>A proper string for the enum.</returns>
    public static string ToDisplayString(this Enum @enum)
    {
        var regex = new Regex(@"([^\^])([A-Z][a-z$])");
        return regex.Replace(@enum.ToString(), m => $"{m.Groups[1].Value} {m.Groups[2].Value}");
    }
}