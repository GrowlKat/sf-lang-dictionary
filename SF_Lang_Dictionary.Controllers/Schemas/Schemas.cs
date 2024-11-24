namespace SF_Lang_Dictionary.Controllers.Schemas;

public class SuffixSchema
{
    public int SfxId { get; set; }

    public string? Suffix1 { get; set; }

    public string? Mtp { get; set; }

    public string? Stp { get; set; }
}

/// <summary>
/// Request object for the Tags search endpoints
/// </summary>
public class TagsRequest
{
    public required List<string> tags { get; set; }
}

public class IPARequestResponse
{
    public required string response { get; set; }
}