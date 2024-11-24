namespace SF_Lang_Dictionary.Controllers;

/// <summary>
/// Utils class to help project development
/// </summary>
public static class Helper
{
    /// <summary>
    /// Char array with all the vocals
    /// </summary>
    public static char[] Vocals { get => "aAāĀeEēĒiIīĪoOōŌuUūŪäÄöÖüÜyY".ToCharArray(); }

    /// <summary>
    /// Char array with all the long vocals
    /// </summary>
    public static char[] ShortVocals { get => "aAeEiIoOuUäÄöÖüÜyY".ToCharArray(); }

    /// <summary>
    /// Char array with all the short vocals
    /// </summary>
    public static char[] LongVocals { get => "āĀēĒīĪōŌūŪ".ToCharArray(); }

    /// <summary>
    /// Char array with all the consonants
    /// </summary>
    public static char[] Consonants { get => "fFvVþÞðÐrRkKgGwWhHnNjJpPbBsSzZtTdDmMlL".ToCharArray(); }

    /// <summary>
    /// Char array with all the non liquid consonants
    /// </summary>
    public static char[] NonLiquidConsonants { get => "fFþÞðÐkKgGhHnNpPbBsSzZtTdDmM".ToCharArray(); }

    /// <summary>
    /// Char array with all the liquid consonants
    /// </summary>
    public static char[] LiquidConsonants { get => "rRwWYylL".ToCharArray(); }

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
    /// Char array with all the consonant IPA characters
    /// </summary>
    public static char[] IPAConsonants { get => "fvθðrkgwxhnjʦʤpbsztdmlʃ".ToCharArray(); }

    /// <summary>
    /// Char array with all the consonant IPA characters
    /// </summary>
    public static char[] IPAVocals { get => "uʊyaɑiɨoɔøeɛ".ToCharArray(); }

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
    /// <param name="s">The string to map as IPA characters</param>
    /// <returns></returns>
    public static string MapToIPA(this string? s)
    {
        if (s is null) return "";
        string res = "";
        s = s.ToLower();
        for (int i = 0; i < s.Length; i++)
        {
            bool lastChar = i == s.Length - 1;
            bool firstChar = i == 0;
            switch (s[i])
            {
                case 'þ':
                    res += 'θ';
                    break;
                case 'ð':
                    res += 'ð';
                    break;
                case 'u':
                    res += 'ʊ';
                    break;
                case 'ū':
                    res += 'ʊ';
                    res += 'ː';
                    break;
                case 'ü':
                    res += 'y';
                    break;
                case 'ā':
                    res += 'ɑ';
                    res += 'ː';
                    break;
                case 'ä':
                    res += 'ɑ';
                    break;
                case 'g':
                    // Sets [x] if the next character is 'h', otherwise sets [g]
                    if (!lastChar && s[i + 1] == 'h')
                    {
                        res += 'x';
                        i++;
                    }
                    else res += 'g';
                    break;
                case 'ī':
                    res += 'i';
                    res += 'ː';
                    break;
                case 'y':
                    // Sets [j] if the next character is a vocal, if not, if theres another 'y' character, sets [j] and [ɨ], otherwise sets [ɨ]
                    if (!lastChar && s[i + 1] == 'y')
                    {
                        res += 'j';
                        res += 'ɨ';
                        i++;
                    }
                    else if (!lastChar && s[i + 1] == Vocals.ToList().FirstOrDefault(c => c.Equals(s[i + 1])))
                    {
                        res += 'j';
                    }
                    else res += 'ɨ';
                    break;
                case 't':
                    // Sets [ʦ] if the next character is 'z', otherwise sets [t]
                    if (!lastChar && s[i + 1] == 'z')
                    {
                        res += 'ʦ';
                        i++;
                    }
                    else res += 't';
                    break;
                case 'j':
                    res += 'ʤ';
                    break;
                case 's':
                    // Sets [ʃ] if the next character is 'h', otherwise sets [s]
                    if (!lastChar && s[i + 1] == 'h')
                    {
                        res += 'ʃ';
                        i++;
                    }
                    else res += 's';
                    break;
                case 'ō':
                    res += 'ɔ';
                    res += 'ː';
                    break;
                case 'ö':
                    res += 'ø';
                    break;
                case 'ē':
                    res += 'ɛ';
                    res += 'ː';
                    break;
                default:
                    res += s[i];
                    break;
            }
        }
        return res;
    }

    /// <summary>
    /// Convert a string of IPA characters to a string of special characters
    /// </summary>
    /// <param name="s">The IPA string to convert</param>
    /// <returns></returns>
    public static string UnmapIPA(this string s)
    {
        string res = "";
        s = s.ToLower();
        for (int i = 0; i < s.Length; i++)
        {
            bool lastChar = i == s.Length - 1;
            bool firstChar = i == 0;
            switch (s[i])
            {
                case 'ʊ':
                    if (!lastChar && s[i + 1] == 'ː')
                    {
                        res += 'ū';
                        i++;
                    }
                    else res += 'u';
                    break;
                case 'y':
                    res += 'ü';
                    break;
                case 'θ':
                    res += 'þ';
                    break;
                case 'ð':
                    res += 'ð';
                    break;
                case 'ɑ':
                    if (!lastChar && s[i + 1] == 'ː')
                    {
                        res += 'ā';
                        i++;
                    }
                    else res += 'ä';
                    break;
                case 'x':
                    res += 'g';
                    res += 'h';
                    break;
                case 'i':
                    if (!lastChar && s[i + 1] == 'ː')
                    {
                        res += 'ī';
                        i++;
                    }
                    else res += 'i';
                    break;
                case 'ɨ':
                    res += 'y';
                    break;
                case 'j':
                    res += 'y';
                    break;
                case 'ʦ':
                    res += 't';
                    res += 'z';
                    break;
                case 'ʤ':
                    res += 'j';
                    break;
                case 'ʃ':
                    res += 's';
                    res += 'h';
                    break;
                case 'ɔ':
                    if (!lastChar && s[i + 1] == 'ː')
                    {
                        res += 'ō';
                        i++;
                    }
                    else throw new Exception("Every open 'o' should be long");
                    break;
                case 'ø':
                    res += 'ö';
                    break;
                case 'ɛ':
                    if (!lastChar && s[i + 1] == 'ː')
                    {
                        res += 'ē';
                        i++;
                    }
                    else throw new Exception("Every open 'e' should be long");
                    break;
                default:
                    res += s[i];
                    break;
            }
        }
        return res;
    }

    /// <summary>
    /// Calculates the syllables of a word and its tonal syllable
    /// <param name="word"/>The word to be analyzed</param>
    /// <returns>A list with the syllables, including the tonal one</returns>
    /// </summary>
    public static List<string> GetSyllables(this string word)
    {
        List<string> syllables = [];
        bool analyzing = true;
        int i = 0;
        int cnt = 0;
        string s = word.MapToIPA();
        var ipaShortVocals = new string(new string(ShortVocals).MapToIPA().Distinct().Where(c => !c.Equals('j')).ToArray());
        var ipaLongVocals = new string(new string(LongVocals).MapToIPA().Distinct().Where(c => !c.Equals('ː')).ToArray());
        var ipaLiquidConsonants = new string(new string(LiquidConsonants).ToString().MapToIPA().Distinct().Where(c => !c.Equals('ɨ')).ToArray());
        var ipaNonLiquidConsonants = new string(new string(IPAConsonants).ToString().MapToIPA().Distinct().Where(c => !ipaLiquidConsonants.Contains(c)).ToArray());

        bool isVocal(char c) => IPAVocals.Contains(c);
        bool isShortVocal(char c) => ipaShortVocals.Contains(c);
        bool isLongVocal(char c, char m) => ipaLongVocals.Contains(c) && m.Equals('ː');
        bool isConsonant(char c) => IPAConsonants.Contains(c);
        bool isLiquidConsonant(char c) => ipaLiquidConsonants.Contains(c);
        bool isNonLiquidConsonant(char c) => ipaNonLiquidConsonants.Contains(c);

        Console.WriteLine(s);

        do
        {
            string res = "";
            bool lastChar = cnt == word.Length - 1;
            bool firstChar = cnt == 0;
            bool monoSyllable = false;
            bool lastSyllable = false;

            // First syllable of the word
            if (firstChar)
            {
                // First consonants
                if (isNonLiquidConsonant(s[i]))
                {
                    res += s[i];
                    i++;
                    if (isNonLiquidConsonant(s[i]))
                    {
                        res += s[i];
                        i++;
                    }
                    if (isLiquidConsonant(s[i]))
                    {
                        res += s[i];
                        i++;
                        if (isLiquidConsonant(s[i]))
                        {
                            res += s[i];
                            i++;
                        }
                    }
                    else if (isLiquidConsonant(s[i]))
                    {
                        res += s[i];
                        i++;
                        if (isLiquidConsonant(s[i]))
                        {
                            res += s[i];
                            i++;
                        }
                    }
                }
                else if (isLiquidConsonant(s[i]))
                {
                    res += s[i];
                    i++;
                    if (isLiquidConsonant(s[i]))
                    {
                        res += s[i];
                        i++;
                    }
                }

                // Vocals (Up to 2 shorts, or 1 long)
                if (isVocal(s[i]) && isLongVocal(s[i], s[i + 1]))
                {
                    res += s[i];
                    res += 'ː';
                    i += 2;
                }
                else if (isVocal(s[i]) && isShortVocal(s[i]))
                {
                    res += s[i];
                    i++;
                    if (s.Length - i > 0 && isVocal(s[i]) && isShortVocal(s[i]))
                    {
                        res += s[i];
                        i++;
                    }
                }

                // Checks if the word is mono-syllabic (Up to 4 consonants, Example: "tlyawrst", or up to 2 vocals, as "Sae")
                if (s.Length - i > 0 && s.Length - i <= 4)
                {
                    int consonants = 0;
                    int vocals = 0;
                    foreach (char c in s[i..])
                    {
                        if (isConsonant(c)) consonants++;
                        if (isVocal(c)) vocals++;
                    }
                    if (consonants != 0 && consonants == s.Length - i && vocals == 0) monoSyllable = true;
                    if (monoSyllable) res += s[i..];
                }
                else if (s.Length - i <= 0) monoSyllable = true;

                // Last consonants
                if (s.Length - i > 0 && !monoSyllable)
                {
                    // Checks if next char is not a vocal, so this char belongs to this syllable
                    if (!isVocal(s[i + 1]))
                    {
                        if (isLiquidConsonant(s[i]))
                        {
                            res += s[i];
                            i++;
                            if (isLiquidConsonant(s[i]))
                            {
                                res += s[i];
                                i++;
                            }
                            else if (isNonLiquidConsonant(s[i]) && !isVocal(s[i + 1]))
                            {
                                res += s[i];
                                i++;
                            }
                        }
                        else if (isNonLiquidConsonant(s[i]) && isNonLiquidConsonant(s[i + 1]) && !isVocal(s[i + 1]))
                        {
                            res += s[i];
                            i++;
                        }
                    }                
                }
            }

            // Middle or Last Syllable
            else if (!firstChar)
            {
                // First consonants
                if (isNonLiquidConsonant(s[i]))
                {
                    res += s[i];
                    i++;
                    if (isLiquidConsonant(s[i]))
                    {
                        res += s[i];
                        i++;
                        if (isLiquidConsonant(s[i]))
                        {
                            res += s[i];
                            i++;
                        }
                    }
                    else if (isLiquidConsonant(s[i]))
                    {
                        res += s[i];
                        i++;
                        if (isLiquidConsonant(s[i]))
                        {
                            res += s[i];
                            i++;
                        }
                    }
                }
                else if (isLiquidConsonant(s[i]))
                {
                    res += s[i];
                    i++;
                    if (isLiquidConsonant(s[i]))
                    {
                        res += s[i];
                        i++;
                    }
                }

                // Vocals (Up to 2 shorts, or 1 long)
                if (isVocal(s[i]) && i + 1 < s.Length && isLongVocal(s[i], s[i + 1]))
                {
                    res += s[i];
                    res += 'ː';
                    i += 2;
                }
                else if (isVocal(s[i]) && isShortVocal(s[i]))
                {
                    res += s[i];
                    i++;
                    if (s.Length - i > 0 && isVocal(s[i]) && isShortVocal(s[i]))
                    {
                        res += s[i];
                        i++;
                    }
                }

                // Checks if the word is the last syllable (Up to 4 consonants, Example: "tlyawrst", or up to 2 vocals, as "Sae")
                if (s.Length - i > 0 && s.Length - i <= 4)
                {
                    int consonants = 0;
                    int vocals = 0;
                    foreach (char c in s[i..])
                    {
                        if (isConsonant(c)) consonants++;
                        if (isVocal(c)) vocals++;
                    }
                    if (consonants != 0 && consonants == s.Length - i && vocals == 0) lastSyllable = true;
                    if (lastSyllable) res += s[i..];
                }
                else if (s.Length - i <= 0) lastSyllable = true;

                // Last consonants
                if (s.Length - i > 0 && !lastSyllable)
                {
                    // Checks if next char is not a vocal, so this char belongs to this syllable
                    if (!isVocal(s[i + 1]))
                    {
                        if (isLiquidConsonant(s[i]))
                        {
                            res += s[i];
                            i++;
                            if (isLiquidConsonant(s[i]))
                            {
                                res += s[i];
                                i++;
                            }
                            else if (isNonLiquidConsonant(s[i]) && !isVocal(s[i + 1]))
                            {
                                res += s[i];
                                i++;
                            }
                        }
                        else if (isNonLiquidConsonant(s[i]) && isNonLiquidConsonant(s[i + 1]) && !isVocal(s[i + 1]))
                        {
                            res += s[i];
                            i++;
                        }
                    }
                }
            }

            syllables.Add(res);
            if (monoSyllable) analyzing = false;
            if (lastSyllable) analyzing = false;
            s = s[i..];
            cnt += i;
            i = 0;
        }
        while (analyzing);

        syllables = syllables.GetTonalSyllable().ToList();
        return syllables;
    }

    /// <summary>
    /// Get the tonal syllable of a word
    /// <param name="word"/>The list to syllables to be analyzed</param>
    /// <returns>IEnumerable with all the syllables and the tonal one</returns>
    /// </summary>
    public static IEnumerable<string> GetTonalSyllable(this IEnumerable<string> word)
    {
        bool isIPA = word.All(s => s.All(c => IPACharacters.Contains(c))); // Analyzes if the string list is an IPA string
        IEnumerable<string> syllables = [];
        string tonalSyllable = "";
        int syllableOrder = -1;

        if (!isIPA) word = word.Select(s => s.MapToIPA());

        // If the word has one or more long vocals, the tonal syllable is the first who have a long vocal
        if (word.Any(s => s.Any(c => c.Equals('ː'))))
        {
            tonalSyllable = word.First(s =>
            {
                syllableOrder++;
                return s.Any(c => c.Equals('ː'));
            });
        }

        // If the word has only one syllable, the tonal syllable is the first one
        else if (word.Count() == 1)
        {
            syllableOrder = 0;
            tonalSyllable = word.First();
        }

        // If the word has 2 syllables, the tonal syllable is the first one
        else if (word.Count() == 2)
        {
            syllableOrder = 0;
            tonalSyllable = word.First();
        }

        // If the word has 3 or more syllables, the tonal syllable is the antepenultimate one
        else if (word.Count() >= 3)
        {
            syllableOrder = 2;
            tonalSyllable = word.ElementAt(word.Count() - 3);
        }

        syllables = word.Select((s, i) => i == syllableOrder ? '\'' + s : s);
        return syllables;
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