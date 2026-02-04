public class DriversLicense : License
{
    public required string VehicleClass { get; init; }
    public required string EyeColor { get; init; }
    public required Sex Sex { get; set; }
    public required int HeightFeet { get; set; }
    public required int HeightInches { get; set; }
    public required bool OrganDonor { get; set; }
    public string? Restrictions { get; set; }
}