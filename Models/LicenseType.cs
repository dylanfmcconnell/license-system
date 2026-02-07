/// <summary>
/// Represents a specific type of license (e.g., "Standard Driver's License", "Commercial Vehicle License").
/// </summary>
public class LicenseType
{
    /// <summary>Gets or sets the unique identifier for the license type.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the display name of the license type.</summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the foreign key identifier of the parent category.</summary>
    public int CategoryId { get; set; } // For Dapper

    /// <summary>Gets or sets the navigation property to the parent category.</summary>
    public LicenseCategory? Category { get; set; }

    /// <summary>Gets or sets the class name used for Dapper mapping to resolve the <see cref="LicenseClass"/> type.</summary>
    public string LicenseClassName { get; set; } = "License"; // For Dapper

    /// <summary>Gets or sets the CLR type of the license subclass to instantiate (e.g., <see cref="DriversLicense"/>).</summary>
    public required Type LicenseClass { get; set; }

    /// <summary>Gets or sets the expiration period in months, or <c>null</c> if the license does not expire.</summary>
    public int? ExpirationTime { get; set; }

    /// <summary>Gets or sets the fee amount for this license type.</summary>
    public decimal? Fee { get; set; }

    /// <summary>Gets or sets an optional description of the license type.</summary>
    public string? Description { get; set; }
}
