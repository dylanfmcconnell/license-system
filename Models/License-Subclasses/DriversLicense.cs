/// <summary>
/// Represents a standard driver's license with personal identification details.
/// </summary>
public class DriversLicense : License
{
    /// <summary>Gets the vehicle class the holder is authorized to operate.</summary>
    public required string VehicleClass { get; init; }

    /// <summary>Gets the eye color of the license holder.</summary>
    public required string EyeColor { get; init; }

    /// <summary>Gets or sets the sex designation on the license.</summary>
    public required Sex Sex { get; set; }

    /// <summary>Gets or sets the height in feet component of the license holder.</summary>
    public required int HeightFeet { get; set; }

    /// <summary>Gets or sets the height in inches component of the license holder.</summary>
    public required int HeightInches { get; set; }

    /// <summary>Gets or sets whether the license holder is an organ donor.</summary>
    public required bool OrganDonor { get; set; }

    /// <summary>Gets or sets any driving restrictions noted on the license.</summary>
    public string? Restrictions { get; set; }
}
