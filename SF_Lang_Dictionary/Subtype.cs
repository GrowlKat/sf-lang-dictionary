using System;
using System.Collections.Generic;

namespace SF_Lang_Dictionary;

public partial class Subtype
{
    public int StpId { get; set; }

    public string? Subtype1 { get; set; }

    public int? MtpId { get; set; }

    public virtual Maintype? Mtp { get; set; }

    public virtual Prefix? Prefix { get; set; }

    public ICollection<Suffix> Suffixes { get; set; } = new List<Suffix>();
}
