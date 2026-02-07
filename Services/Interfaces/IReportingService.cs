/// <summary>
/// Defines operations for generating reports on applications, licenses, and revenue.
/// </summary>
public interface IReportingService
{
    /// <summary>
    /// Gets the count of applications grouped by their approval status.
    /// </summary>
    /// <returns>A dictionary mapping each <see cref="ApplicationStatus"/> to its count.</returns>
    Task<Dictionary<ApplicationStatus, int>> GetApplicationCountsByStatus();

    /// <summary>
    /// Gets the total revenue generated from approved applications, grouped by license type name.
    /// </summary>
    /// <returns>A dictionary mapping license type names to their total revenue.</returns>
    Task<Dictionary<string, decimal>> GetRevenueByLicenseType();

    /// <summary>
    /// Gets the applicants with the most licenses, ordered by license count descending.
    /// </summary>
    /// <param name="count">The maximum number of applicants to return.</param>
    /// <returns>A list of top applicants.</returns>
    Task<List<Applicant>> GetTopApplicantsByLicenseCount(int count);

    /// <summary>
    /// Gets all valid licenses that will expire within the specified number of days.
    /// </summary>
    /// <param name="days">The number of days from today to check for expiration.</param>
    /// <returns>A list of licenses expiring within the threshold.</returns>
    Task<List<License>> GetLicensesExpiringSoon(int days);

    /// <summary>
    /// Gets the count of issued licenses grouped by their category name.
    /// </summary>
    /// <returns>A dictionary mapping category names to their license count.</returns>
    Task<Dictionary<string, int>> GetLicenseCountsByCategory();
}
