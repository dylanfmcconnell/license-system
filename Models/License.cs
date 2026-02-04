public class License
{
    public required string Id { get; init; }
    public required LicenseType LicenseType { get; init;}
    public required Applicant Applicant { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public required DateOnly DateOfBirth { get; init; }
    public required DateOnly IssueDate { get; init; }
    public DateOnly? ExpirationDate { get; init; }
    public LicenseStatus Status { get; set; } = LicenseStatus.Valid;
}