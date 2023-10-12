using System;
using System.Collections.Generic;

namespace SF_Lang_Dictionary.Models;

public partial class Suffix
{
    public int SfxId { get; set; }

    public string? Suffix1 { get; set; }

    public int? MtpId { get; set; }

    public int? StpId { get; set; }

    public virtual Maintype? Mtp { get; set; }

    public virtual Subtype? Stp { get; set; }
}
