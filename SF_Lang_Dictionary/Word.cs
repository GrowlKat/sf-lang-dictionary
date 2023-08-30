using System;
using System.Collections.Generic;

namespace SF_Lang_Dictionary;

public partial class Word
{
    public int WordId { get; set; }

    public int? FirstWord { get; set; }

    public int? SecondWord { get; set; }

    public string Meaning { get; set; } = null!;
}
