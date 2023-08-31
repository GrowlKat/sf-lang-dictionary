namespace SF_Lang_Dictionary.Controllers;

/// <summary>
/// Utils class to help project development
/// </summary>
public static class Helper
{
    /// <summary>
    /// Char array with all the vocals
    /// </summary>
    public static char[] Vocals { get => "āĀēĒīĪōŌūŪyYäÄöÖüÜ".ToCharArray(); }

    /// <summary>
    /// Char array with all the consonants
    /// </summary>
    public static char[] Consonants { get => "fFþÞðÐrRkKgGwWhHnNjJpPbBsSzZtTmMlL".ToCharArray(); }

    /// <summary>
    /// Char array with all the special characters
    /// </summary>
    public static char[] SpecialCharacters { get => "āĀēĒīĪōŌūŪäÄöÖüÜþÞðÐʊθɑɨʦʤʃɔøɛ".ToCharArray(); }

    /// <summary>
    /// Capitalizes a word by putting it's first character as a mayus
    /// </summary>
    /// <param name="s">The string to be capitalized</param>
    /// <returns></returns>
    public static string Capitalize(this string s)
    {
        return s[0].ToString().ToUpper() + s[..1];
    }

    /// <summary>
    /// Checks if a string ends in a vocal
    /// </summary>
    /// <param name="s">The string to be analyzed</param>
    /// <returns>The string ended with a vocal?</returns>
    public static bool EndsWithVocal(this string s)
    {
        bool endInVocal = false;

        // Analyzes every character in vocals array, if some of these matches, sets the return value as true and breaks the loop
        foreach (char c in Vocals)
        {
            if (s[^1] == c)
            {
                endInVocal = true;
                break; 
            }
        }
        return endInVocal;
    }
}