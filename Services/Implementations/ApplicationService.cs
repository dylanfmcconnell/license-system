public sealed class ApplicationService: IApplicationService
{
    private readonly IRepository<Application, int> _applicationRepository;
    private readonly IRepository<Applicant, int> _applicantRepository;
    private readonly IRepository<LicenseType, int> _licenseTypeRepository;
    private readonly IRepository<License, string> _licenseRepository;

    public ApplicationService(
        IRepository<Application, int> applicationRepository, 
        IRepository<LicenseType, int> licenseTypeRepository, 
        IRepository<Applicant, int> applicantRepository, 
        IRepository<License, string> licenseRepository) 
    {
        _applicationRepository = applicationRepository;
        _licenseTypeRepository = licenseTypeRepository;
        _applicantRepository = applicantRepository;
        _licenseRepository = licenseRepository;
    }

    public async Task<Application?> SubmitApplication(ApplicationSubmitRequest applicationRequest)
    {
        if (string.IsNullOrWhiteSpace(applicationRequest.DeliveryAddress)) return null;

        var applicant = await _applicantRepository.GetEntityById(applicationRequest.ApplicantId);
        if (applicant == null) return null;

        var licenseType = await _licenseTypeRepository.GetEntityById(applicationRequest.LicenseTypeId);
        if (licenseType == null) return null;

        var application = new Application
        {
            ApplicantId = applicant.Id,
            Applicant = applicant,
            LicenseTypeId = licenseType.Id,
            LicenseType = licenseType,
            SubmissionDate = DateOnly.FromDateTime(DateTime.Now),
            DeliveryAddress = applicationRequest.DeliveryAddress,
            Fee = licenseType.Fee
        };

        return await _applicationRepository.AddEntity(application);
    }

    public Task<License?> ApproveApplication(int applicationId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DenyApplication(int applicationId)
    {
        throw new NotImplementedException();
    }

    public Task<Application?> GetApplication(int applicationId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Application>> GetApplicationsByApplicant(int applicantId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Application>> GetApplicationsByLicenseType(int licenseTypeId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Application>> GetApplicationsByStatus(ApplicationStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateDeliveryAddress(int applicationId, string newAddress)
    {
        throw new NotImplementedException();
    }
}