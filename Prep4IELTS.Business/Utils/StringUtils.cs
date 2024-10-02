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
    
    public static bool CompareAnswers(string correctAnswer, string userAnswer)
    {
        // Normalize both answers: remove special characters, normalize spaces, and convert to upper case
        string normalizedCorrectAnswer = NormalizeAnswer(correctAnswer);
        string normalizedUserAnswer = NormalizeAnswer(userAnswer);

        return normalizedCorrectAnswer == normalizedUserAnswer;
    }

    private static string NormalizeAnswer(string answer)
    {
        // Remove special characters, normalize spaces, and convert to uppercase
        return Regex.Replace(answer, @"[^a-zA-Z0-9\s]", "") // Remove special characters
            .Trim()                                // Remove spaces
            .ToUpper()                              // Convert to uppercase
            .Replace("\\s+", " ");                  // Normalize all spaces to single spaces
    }
}