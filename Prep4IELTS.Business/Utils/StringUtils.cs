using System.Text.RegularExpressions;

namespace Prep4IELTS.Business.Utils;

public static class StringUtils
{
    public static string RemoveSymbols(string input)
    {
        // Special pattern in string
        string pattern = @"[\'~`!@#\$%\^&\*\(\)_\+\-=\[\]\{\};:<>\\\/\|]";
        // Replace symbols with an empty string
        string cleanedInput = Regex.Replace(input, pattern, "");
        
        return cleanedInput;
    }
}