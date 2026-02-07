public sealed class LicenseNotificationService
{
    private readonly IRepository<License, string> _licenseRepository;

    public event Action<License>? OnLicenseExpiringSoon;

    public LicenseNotificationService(IRepository<License, string> licenseRepository)
    {
        _licenseRepository = licenseRepository;
    }

    public async Task CheckExpiringLicenses(int daysThreshold)
    {
        var licenses = await _licenseRepository.GetAllEntities();
        var expiring = licenses.Where(l => l.IsExpiringSoon(daysThreshold));

        foreach (var license in expiring)
        {
            // Raise the event - notify all subscribers
            OnLicenseExpiringSoon?.Invoke(license);
        }
    }
}
