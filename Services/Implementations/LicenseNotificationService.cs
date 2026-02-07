/// <summary>
/// Service that checks for licenses approaching expiration and raises notification events.
/// </summary>
public sealed class LicenseNotificationService
{
    private readonly IRepository<License, string> _licenseRepository;

    /// <summary>
    /// Raised when a license is found to be expiring within the specified threshold.
    /// </summary>
    public event Action<License>? OnLicenseExpiringSoon;

    /// <summary>
    /// Initializes a new instance of the <see cref="LicenseNotificationService"/> class.
    /// </summary>
    /// <param name="licenseRepository">The license repository.</param>
    public LicenseNotificationService(IRepository<License, string> licenseRepository)
    {
        _licenseRepository = licenseRepository;
    }

    /// <summary>
    /// Checks all licenses and raises <see cref="OnLicenseExpiringSoon"/> for each license
    /// expiring within the specified number of days.
    /// </summary>
    /// <param name="daysThreshold">The number of days from today to check for upcoming expirations.</param>
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
