public interface IReportingService
{
    Task<Dictionary<ApplicationStatus, int>> GetApplicationCountsByStatus();
    Task<Dictionary<string, decimal>> GetRevenueByLicenseType();
    Task<List<Applicant>> GetTopApplicantsByLicenseCount(int count);
    Task<List<License>> GetLicensesExpiringSoon(int days);
    Task<Dictionary<string, int>> GetLicenseCountsByCategory();
}
