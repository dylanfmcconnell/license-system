public sealed class ReportingService : IReportingService
{
    private readonly IRepository<Application, int> _applicationRepository;
    private readonly IRepository<License, string> _licenseRepository;
    private readonly IRepository<LicenseType, int> _licenseTypeRepository;
    private readonly IRepository<LicenseCategory, int> _licenseCategoryRepository;
    private readonly IRepository<Applicant, int> _applicantRepository;

    public ReportingService(
        IRepository<Application, int> applicationRepository,
        IRepository<License, string> licenseRepository,
        IRepository<LicenseType, int> licenseTypeRepository,
        IRepository<LicenseCategory, int> licenseCategoryRepository,
        IRepository<Applicant, int> applicantRepository
    )
    {
        _applicationRepository = applicationRepository;
        _licenseRepository = licenseRepository;
        _licenseTypeRepository = licenseTypeRepository;
        _licenseCategoryRepository = licenseCategoryRepository;
        _applicantRepository = applicantRepository;
    }

    public async Task<Dictionary<ApplicationStatus, int>> GetApplicationCountsByStatus()
    {
        var applications = await _applicationRepository.GetAllEntities();

        return applications.GroupBy(a => a.ApprovedStatus).ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetLicenseCountsByCategory()
    {
        var licenses = await _licenseRepository.GetAllEntities();
        var licenseTypes = (await _licenseTypeRepository.GetAllEntities()).ToDictionary(lt =>
            lt.Id
        );
        var categories = (await _licenseCategoryRepository.GetAllEntities()).ToDictionary(c =>
            c.Id
        );

        return licenses
            .GroupBy(l =>
            {
                var licenseType = licenseTypes[l.LicenseTypeId];
                var category = categories[licenseType.CategoryId];
                return category.Name;
            })
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<List<License>> GetLicensesExpiringSoon(int days)
    {
        var licenses = await _licenseRepository.GetAllEntities();
        var today = DateOnly.FromDateTime(DateTime.Now);
        var cutoff = today.AddDays(days);

        return
        [
            .. licenses.Where(l =>
                l.Status == LicenseStatus.Valid
                && l.ExpirationDate != null
                && l.ExpirationDate >= today
                && l.ExpirationDate <= cutoff
            ),
        ];
    }

    public async Task<Dictionary<string, decimal>> GetRevenueByLicenseType()
    {
        var applications = await _applicationRepository.GetAllEntities();

        return applications
            .Where(a => a.ApprovedStatus == ApplicationStatus.Approved)
            .GroupBy(a => a.LicenseType!.Name)
            .ToDictionary(g => g.Key, g => g.Sum(a => a.Fee ?? 0m));
    }

    public async Task<List<Applicant>> GetTopApplicantsByLicenseCount(int count)
    {
        var applicants = await _applicantRepository.GetAllEntities();

        return [.. applicants.OrderByDescending(a => a.Licenses.Count).Take(count)];
    }
}
