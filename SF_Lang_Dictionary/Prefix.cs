using System;
using System.Collections.Generic;

namespace SF_Lang_Dictionary;

public partial class Prefix
{
    public int PfxId { get; set; }

    public string? Prefix1 { get; set; }

    public int? MtpId { get; set; }

    public int? StpId { get; set; }

    public virtual Maintype? Mtp { get; set; }

    public virtual Subtype? Stp { get; set; }
}
