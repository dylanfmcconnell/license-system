public class LicenseType
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; } // For Dapper
    public LicenseCategory? Category { get; set; }
    public string LicenseClassName { get; set; } = "License"; // For Dapper
    public required Type LicenseClass { get; set; }
    public int? ExpirationTime { get; set; }
    public decimal? Fee { get; set; }
    public string? Description { get; set; }
}
