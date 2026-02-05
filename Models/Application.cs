public class Application
{
    public int Id { get; set; }
    public int ApplicantId { get; set; } // For Dapper
    public  Applicant? Applicant { get; set; }
    public int LicenseTypeId { get; set; } // For Dapper
    public  LicenseType? LicenseType { get; set; }
    public required DateOnly SubmissionDate { get; init; }
    public required string DeliveryAddress { get; set; }
    public ApplicationStatus ApprovedStatus { get; set; } = ApplicationStatus.UnderReview;
    public DateOnly? ApprovedDate { get; set; }
    public string? LicenseId { get; set; } // For Dapper
    public License? License { get; set; }
    public decimal? Fee { get; init; }
}