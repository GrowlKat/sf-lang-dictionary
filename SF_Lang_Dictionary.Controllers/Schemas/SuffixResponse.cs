namespace SF_Lang_Dictionary.Controllers.Schemas
{
    /// <summary>
    /// Suffix response schema, used to return the suffixes from the database with a simplified schema
    /// </summary>
    public class SuffixResponse
    {
        public required int SfxId { get; set; }
        public string? Suffix { get; set; } 
        public string? Maintype { get; set; }
        public string? Subtype { get; set; }
        public int? MtpId { get; set; }
        public int? StpId { get; set; }
    }
}
