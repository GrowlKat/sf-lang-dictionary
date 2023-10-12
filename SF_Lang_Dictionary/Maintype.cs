using System;
using System.Collections.Generic;

namespace SF_Lang_Dictionary.Models;

public partial class Maintype
{
    public int MtpId { get; set; }

    public string? Maintype1 { get; set; }

    public virtual ICollection<Prefix> Prefixes { get; set; } = new List<Prefix>();

    public virtual ICollection<Suffix> Suffixes { get; set; } = new List<Suffix>();
}
