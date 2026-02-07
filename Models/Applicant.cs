/// <summary>
/// Represents a person who applies for and holds licenses in the system.
/// </summary>
public class Applicant
{
    /// <summary>Gets or sets the unique identifier for the applicant.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the applicant's first name.</summary>
    public required string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the applicant's last name.</summary>
    public required string LastName { get; set; } = string.Empty;

    /// <summary>Gets the date the applicant joined the system.</summary>
    public required DateOnly DateJoined { get; init; }

    /// <summary>Gets the applicant's date of birth.</summary>
    public required DateOnly DateOfBirth { get; init; }

    /// <summary>Gets or sets the collection of applications submitted by this applicant.</summary>
    public List<Application> Applications { get; set; } = [];

    /// <summary>Gets or sets the collection of licenses held by this applicant.</summary>
    public List<License> Licenses { get; set; } = [];

    /// <summary>Gets or sets the applicant's mailing address.</summary>
    public required string Address { get; set; }

    /// <summary>Gets or sets the applicant's email address.</summary>
    public required string Email { get; set; }

    /// <summary>Gets or sets the applicant's phone number, if provided.</summary>
    public string? PhoneNumber { get; set; }
}
