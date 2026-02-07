/// <summary>
/// Service implementation that manages the license application lifecycle,
/// including submission, approval, denial, and querying.
/// </summary>
public sealed class ApplicationService : IApplicationService
{
    private readonly IRepository<Application, int> _applicationRepository;
    private readonly IRepository<Applicant, int> _applicantRepository;
    private readonly IRepository<LicenseType, int> _licenseTypeRepository;
    private readonly IRepository<License, string> _licenseRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationService"/> class.
    /// </summary>
    /// <param name="applicationRepository">The application repository.</param>
    /// <param name="licenseTypeRepository">The license type repository.</param>
    /// <param name="applicantRepository">The applicant repository.</param>
    /// <param name="licenseRepository">The license repository.</param>
    public ApplicationService(
        IRepository<Application, int> applicationRepository,
        IRepository<LicenseType, int> licenseTypeRepository,
        IRepository<Applicant, int> applicantRepository,
        IRepository<License, string> licenseRepository
    )
    {
        _applicationRepository = applicationRepository;
        _licenseTypeRepository = licenseTypeRepository;
        _applicantRepository = applicantRepository;
        _licenseRepository = licenseRepository;
    }

    /// <inheritdoc />
    public async Task<Application?> SubmitApplication(ApplicationSubmitRequest applicationRequest)
    {
        if (string.IsNullOrWhiteSpace(applicationRequest.DeliveryAddress))
            return null;

        var applicant = await _applicantRepository.GetEntityById(applicationRequest.ApplicantId);
        if (applicant == null)
            return null;

        var licenseType = await _licenseTypeRepository.GetEntityById(
            applicationRequest.LicenseTypeId
        );
        if (licenseType == null)
            return null;

        var application = new Application
        {
            ApplicantId = applicant.Id,
            Applicant = applicant,
            LicenseTypeId = licenseType.Id,
            LicenseType = licenseType,
            SubmissionDate = DateOnly.FromDateTime(DateTime.Now),
            DeliveryAddress = applicationRequest.DeliveryAddress,
            Fee = licenseType.Fee,
        };

        return await _applicationRepository.AddEntity(application);
    }

    /// <inheritdoc />
    public async Task<License?> ApproveApplication(int applicationId)
    {
        // Get and validate application
        var application = await _applicationRepository.GetEntityById(applicationId);
        if (application == null)
            return null;
        if (application.ApprovedStatus != ApplicationStatus.UnderReview)
            return null;

        // Get related entities
        var applicant = application.Applicant;
        if (applicant == null)
            return null;

        var licenseType = application.LicenseType;
        if (licenseType == null)
            return null;

        // Calculate dates and license id
        var licenseId = LicenseIdGenerator.GenerateId();

        var today = DateOnly.FromDateTime(DateTime.Now);

        DateOnly? expirationDate =
            licenseType.ExpirationTime != null
                ? today.AddMonths((int)licenseType.ExpirationTime)
                : null;

        // Create license
        var license = new License
        {
            Id = licenseId,
            LicenseTypeId = licenseType.Id,
            LicenseType = licenseType,
            ApplicantId = application.ApplicantId,
            Applicant = applicant,
            FirstName = applicant.FirstName,
            LastName = applicant.LastName,
            Address = applicant.Address,
            DateOfBirth = applicant.DateOfBirth,
            IssueDate = today,
            ExpirationDate = expirationDate,
            Status = LicenseStatus.Valid,
        };

        // Save license
        license = await _licenseRepository.AddEntity(license);
        if (license == null)
            return null;

        // Update application
        application.LicenseId = license.Id;
        application.License = license;
        application.ApprovedStatus = ApplicationStatus.Approved;
        application.ApprovedDate = today;

        // Save application
        if (!await _applicationRepository.UpdateEntity(application))
            return null;

        return license;
    }

    /// <inheritdoc />
    public async Task<bool> DenyApplication(int applicationId)
    {
        var application = await _applicationRepository.GetEntityById(applicationId);
        if (application == null)
            return false;
        if (application.ApprovedStatus != ApplicationStatus.UnderReview)
            return false;

        application.ApprovedStatus = ApplicationStatus.Denied;
        application.ApprovedDate = DateOnly.FromDateTime(DateTime.Now);

        return await _applicationRepository.UpdateEntity(application);
    }

    /// <inheritdoc />
    public async Task<Application?> GetApplication(int applicationId)
    {
        return await _applicationRepository.GetEntityById(applicationId);
    }

    /// <inheritdoc />
    public async Task<List<Application>> GetApplicationsByApplicant(int applicantId)
    {
        var allApplications = await _applicationRepository.GetAllEntities();
        return [.. allApplications.Where(a => a.ApplicantId == applicantId)];
    }

    /// <inheritdoc />
    public async Task<List<Application>> GetApplicationsByLicenseType(int licenseTypeId)
    {
        var allApplications = await _applicationRepository.GetAllEntities();
        return [.. allApplications.Where(a => a.LicenseTypeId == licenseTypeId)];
    }

    /// <inheritdoc />
    public async Task<List<Application>> GetApplicationsByStatus(ApplicationStatus status)
    {
        var allApplications = await _applicationRepository.GetAllEntities();
        return [.. allApplications.Where(a => a.ApprovedStatus == status)];
    }

    /// <inheritdoc />
    public async Task<bool> UpdateDeliveryAddress(int applicationId, string newAddress)
    {
        if (string.IsNullOrWhiteSpace(newAddress))
            return false;

        var application = await _applicationRepository.GetEntityById(applicationId);
        if (application == null)
            return false;
        if (application.ApprovedStatus != ApplicationStatus.UnderReview)
            return false;

        application.DeliveryAddress = newAddress;

        return await _applicationRepository.UpdateEntity(application);
    }
}
