/// <summary>
/// Provides extension methods for the <see cref="License"/> class.
/// </summary>
public static class LicenseExtensions
{
    /// <summary>
    /// Determines whether a license is valid and will expire within the specified number of days.
    /// </summary>
    /// <param name="license">The license to check.</param>
    /// <param name="daysThreshold">The number of days from today to check for upcoming expiration.</param>
    /// <returns><c>true</c> if the license is valid and expires within the threshold; otherwise, <c>false</c>.</returns>
    public static bool IsExpiringSoon(this License license, int daysThreshold)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var cutoff = today.AddDays(daysThreshold);

        return (
            license.Status == LicenseStatus.Valid
            && license.ExpirationDate != null
            && license.ExpirationDate >= today
            && license.ExpirationDate <= cutoff
        );
    }
}
