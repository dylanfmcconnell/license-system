/// <summary>
/// Represents a grouping or category of license types (e.g., "Commercial", "Personal").
/// </summary>
public class LicenseCategory
{
    /// <summary>Gets or sets the unique identifier for the category.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the name of the category.</summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>Gets the collection of license types belonging to this category.</summary>
    public List<LicenseType> LicenseTypes { get; } = [];

    /// <summary>Gets or sets an optional description of the category.</summary>
    public string? Description { get; set; }
}
