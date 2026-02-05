public interface IApplicationService
{
    Task<Application?> SubmitApplication(ApplicationSubmitRequest applicationRequest);
    Task<Application?> GetApplication(int applicationId);
    Task<License?> ApproveApplication(int applicationId);
    Task<bool> DenyApplication(int applicationId);
    Task<bool> UpdateDeliveryAddress(int applicationId, string newAddress);
    Task<List<Application>> GetApplicationsByStatus(ApplicationStatus status);
    Task<List<Application>> GetApplicationsByApplicant(int applicantId);
    Task<List<Application>> GetApplicationsByLicenseType(int licenseTypeId);
}