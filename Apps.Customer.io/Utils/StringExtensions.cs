using System.Text.Json;

namespace Apps.Customer.io.Utils;
public static class StringExtensions
{
    public static bool IsJson(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = input.Trim();

        if (input.StartsWith("<"))
            return false;

        if ((input.StartsWith("{") && input.EndsWith("}")) ||
            (input.StartsWith("[") && input.EndsWith("]")))
        {
            try
            {
                JsonDocument.Parse(input);
                return true;
            }
            catch { }
        }

        return false;
    }
}
