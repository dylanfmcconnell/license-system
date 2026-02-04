public class LicenseCategory
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public List<LicenseType> LicenseTypes { get; } = [];
    public string? Description { get; set; }
}