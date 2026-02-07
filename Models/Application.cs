/// <summary>
/// Represents a license application submitted by an applicant.
/// </summary>
public class Application
{
    /// <summary>Gets or sets the unique identifier for the application.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the foreign key identifier of the applicant who submitted this application.</summary>
    public int ApplicantId { get; set; } // For Dapper

    /// <summary>Gets or sets the navigation property to the associated applicant.</summary>
    public Applicant? Applicant { get; set; }

    /// <summary>Gets or sets the foreign key identifier of the requested license type.</summary>
    public int LicenseTypeId { get; set; } // For Dapper

    /// <summary>Gets or sets the navigation property to the requested license type.</summary>
    public LicenseType? LicenseType { get; set; }

    /// <summary>Gets the date the application was submitted.</summary>
    public required DateOnly SubmissionDate { get; init; }

    /// <summary>Gets or sets the address where the license should be delivered.</summary>
    public required string DeliveryAddress { get; set; }

    /// <summary>Gets or sets the current approval status of the application.</summary>
    public ApplicationStatus ApprovedStatus { get; set; } = ApplicationStatus.UnderReview;

    /// <summary>Gets or sets the date the application was approved or denied, if a decision has been made.</summary>
    public DateOnly? ApprovedDate { get; set; }

    /// <summary>Gets or sets the foreign key identifier of the issued license, if approved.</summary>
    public string? LicenseId { get; set; } // For Dapper

    /// <summary>Gets or sets the navigation property to the issued license.</summary>
    public License? License { get; set; }

    /// <summary>Gets the fee associated with this application, derived from the license type.</summary>
    public decimal? Fee { get; init; }
}
