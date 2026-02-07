/// <summary>
/// Represents an issued license credential held by an applicant.
/// </summary>
public class License
{
    /// <summary>Gets the unique string identifier for the license (e.g., "AB1234567").</summary>
    public required string Id { get; init; }

    /// <summary>Gets or sets the foreign key identifier of the license type.</summary>
    public int LicenseTypeId { get; set; } // Dapper needs this

    /// <summary>Gets or sets the navigation property to the license type.</summary>
    public LicenseType? LicenseType { get; set; }

    /// <summary>Gets or sets the foreign key identifier of the applicant who holds this license.</summary>
    public int ApplicantId { get; set; } // Dapper needs this

    /// <summary>Gets or sets the navigation property to the license holder.</summary>
    public Applicant? Applicant { get; set; }

    /// <summary>Gets or sets the first name printed on the license.</summary>
    public required string FirstName { get; set; }

    /// <summary>Gets or sets the last name printed on the license.</summary>
    public required string LastName { get; set; }

    /// <summary>Gets or sets the address printed on the license.</summary>
    public required string Address { get; set; }

    /// <summary>Gets the date of birth printed on the license.</summary>
    public required DateOnly DateOfBirth { get; init; }

    /// <summary>Gets the date the license was issued.</summary>
    public required DateOnly IssueDate { get; init; }

    /// <summary>Gets the date the license expires, or <c>null</c> if it does not expire.</summary>
    public DateOnly? ExpirationDate { get; init; }

    /// <summary>Gets or sets the current status of the license.</summary>
    public LicenseStatus Status { get; set; } = LicenseStatus.Valid;
}
