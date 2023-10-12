using System;
using System.Collections.Generic;

namespace SF_Lang_Dictionary.Models;

public partial class Subtype
{
    public int StpId { get; set; }

    public string? Subtype1 { get; set; }

    public virtual Prefix? Prefix { get; set; }

    public virtual ICollection<Suffix> Suffixes { get; set; } = new List<Suffix>();
}
