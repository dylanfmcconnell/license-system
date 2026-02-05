public class Applicant
{
    public int Id { get; set; }
    public required string FirstName { get; set; } = string.Empty;
    public required string LastName { get; set; } = string.Empty;
    public required DateOnly DateJoined { get; init; }
    public required DateOnly DateOfBirth { get; init; }
    public List<Application> Applications { get; set; } = [];
    public List<License> Licenses { get; set; } = [];
    public string? Address { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
}