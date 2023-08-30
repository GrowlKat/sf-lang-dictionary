using System;
using System.Collections.Generic;

namespace SF_Lang_Dictionary;

public partial class Maintype
{
    public int MtpId { get; set; }

    public string? Maintype1 { get; set; }

    public virtual ICollection<Prefix> Prefixes { get; set; } = new List<Prefix>();

    public virtual ICollection<Subtype> Subtypes { get; set; } = new List<Subtype>();

    public virtual ICollection<Suffix> Suffixes { get; set; } = new List<Suffix>();
}
