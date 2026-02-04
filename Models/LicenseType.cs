public class LicenseType
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required LicenseCategory Category { get; init;}
    public required Type LicenseClass { get; init; }
    public int? ExpirationTime { get; set; }
    public decimal? Fee { get; set; }
    public string? Description { get; set; }
}