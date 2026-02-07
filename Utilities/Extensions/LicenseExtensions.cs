public static class LicenseExtensions
{
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
