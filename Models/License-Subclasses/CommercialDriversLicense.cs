/// <summary>
/// Represents a commercial driver's license (CDL), combining driver and commercial endorsements.
/// </summary>
public class CommercialDriversLicense : DriversLicense
{
    /// <summary>Gets or sets the commercial endorsements on the license (e.g., hazmat, tanker).</summary>
    public string? Endorsements { get; set; }
}
