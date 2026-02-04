public class Application
{
    public int Id { get; set; }
    public required Applicant Applicant { get; init; }
    public required LicenseType LicenseType { get; init; }
    public required DateOnly SubmissionDate { get; init; }
    public required string DeliveryAddress { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.UnderReview;
    public DateOnly? ApprovedDate { get; set; }
    public License? License { get; set; }
    public decimal? Fee { get; init; }
}