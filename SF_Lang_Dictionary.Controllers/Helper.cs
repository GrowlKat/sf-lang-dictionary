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
    public static char[] Consonants { get => "fFþÞðÐrRkKgGwWhHnNjJpPbBsSzZtTdDmMlL".ToCharArray(); }

    /// <summary>
    /// Char array with all the special characters
    /// </summary>
    public static char[] SpecialCharacters { get => "āĀēĒīĪōŌūŪäÄöÖüÜþÞðÐʊθɑɨʦʤʃɔøɛ".ToCharArray(); }

    /// <summary>
    /// Char array with all the special characters used in wrting
    /// </summary>
    public static char[] SpecialWritingCharacters { get => "āĀēĒīĪōŌūŪäÄöÖüÜþÞðÐ".ToCharArray(); }

    /// <summary>
    /// Char array with all the special characters used in IPA pronunciation
    /// </summary>
    public static char[] IPACharacters { get => "fvuʊyθðaɑrkgwxhniɨjʦʤpbsztdmlʃoɔøeɛː'.".ToCharArray(); }

    /// <summary>
    /// Char array with all the special characters used in IPA pronunciation
    /// </summary>
    public static char[] SpecialIPACharacters { get => "ðÐʊθɑɨʦʤʃɔøɛː".ToCharArray(); }

    public static Dictionary<string, string> AvailableLanguages
    {
        get => new()
        {
            { "sf", "Selenian" },
            { "en", "English" },
        };
    }

    /// <summary>
    /// Capitalizes a word by putting it's first character as a mayus
    /// </summary>
    /// <param name="s">The string to be capitalized</param>
    /// <returns>The capitalized string</returns>
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

    /// <summary>
    /// Maps a character to it's IPA pronunciation
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string MapToIPA(this string s)
    {
        string res = "";
        // TODO: Add all the special characters to the switch and fix it's way to find characters
        foreach (char c in s)
        {
            switch (s)
            {
                case "ð":
                    res += "ð";
                    break;
                case "Ð":
                    res += "ð";
                    break;
                case "þ":
                    res += "θ";
                    break;
                case "Þ":
                    res += 'θ';
                    break;
                case "ū":
                    res += 'ʊ';
                    res += 'ː';
                    break;
                case "Ū":
                    res += 'ʊ';
                    res += 'ː';
                    break;
                case "ü":
                    res += 'y';
                    break;
                case "Ü":
                    res += 'y';
                    break;
                case "ā":
                    res += 'a';
                    res += 'ː';
                    break;
                case "Ā":
                    res += 'a';
                    res += 'ː';
                    break;
                case "ä":
                    res += 'ɑ';
                    break;
                case "Ä":
                    res += 'ɑ';
                    break;
                case "ī":
                    res += 'i';
                    res += 'ː';
                    break;
                case "Ī":
                    res += 'i';
                    res += 'ː';
                    break;
                case "tz":
                    res += 'ʦ';
                    break;
                case "Tz":
                    res += 'ʦ';
                    break;
                case "TZ":
                    res += 'ʦ';
                    break;
                case "j":
                    res += 'ʤ';
                    break;
                case "J":
                    res += 'ʤ';
                    break;
                case "sh":
                    res += 'ʃ';
                    break;
                case "Sh":
                    res += 'ʃ';
                    break;
                case "SH":
                    res += 'ʃ';
                    break;
                case "ō":
                    res += 'ɔ';
                    res += 'ː';
                    break;
                case "Ō":
                    res += 'ɔ';
                    res += 'ː';
                    break;
                case "ö":
                    res += 'ø';
                    break;
                case "Ö":
                    res += 'ø';
                    break;
                case "ē":
                    res += 'ɛ';
                    res += 'ː';
                    break;
                case "Ē":
                    res += 'ɛ';
                    res += 'ː';
                    break;
                default:
                    break;
            }
        }
        return res;
    }
}

/// <summary>
/// All the cases that a word can have
/// </summary>
public enum Case
{
    Nominative,
    Accusative,
    Dative,
    Genitive
}

/// <summary>
/// All the declensions that a word can have
/// </summary>
public enum Declension
{
    Strong = 1,
    Soft = 2
}

/// <summary>
/// Enumerates the type of words
/// </summary>
public enum MaintypeEnum
{
    Noun,
    Case,
    Verb,
    Adjective,
    Adverb,
    Pronoun,
    Preposition,
    Conjunction,
    Interjection,
}