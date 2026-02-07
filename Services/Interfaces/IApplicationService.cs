/// <summary>
/// Defines operations for managing the license application lifecycle.
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Submits a new license application.
    /// </summary>
    /// <param name="applicationRequest">The application submission details.</param>
    /// <returns>The created application, or <c>null</c> if submission failed.</returns>
    Task<Application?> SubmitApplication(ApplicationSubmitRequest applicationRequest);

    /// <summary>
    /// Retrieves an application by its identifier.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application.</param>
    /// <returns>The application if found; otherwise, <c>null</c>.</returns>
    Task<Application?> GetApplication(int applicationId);

    /// <summary>
    /// Approves an application and issues the corresponding license.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application to approve.</param>
    /// <returns>The newly issued license, or <c>null</c> if approval failed.</returns>
    Task<License?> ApproveApplication(int applicationId);

    /// <summary>
    /// Denies an application that is currently under review.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application to deny.</param>
    /// <returns><c>true</c> if the application was successfully denied; otherwise, <c>false</c>.</returns>
    Task<bool> DenyApplication(int applicationId);

    /// <summary>
    /// Updates the delivery address for an application that is still under review.
    /// </summary>
    /// <param name="applicationId">The unique identifier of the application.</param>
    /// <param name="newAddress">The new delivery address.</param>
    /// <returns><c>true</c> if the address was successfully updated; otherwise, <c>false</c>.</returns>
    Task<bool> UpdateDeliveryAddress(int applicationId, string newAddress);

    /// <summary>
    /// Retrieves all applications with the specified approval status.
    /// </summary>
    /// <param name="status">The approval status to filter by.</param>
    /// <returns>A list of matching applications.</returns>
    Task<List<Application>> GetApplicationsByStatus(ApplicationStatus status);

    /// <summary>
    /// Retrieves all applications submitted by a specific applicant.
    /// </summary>
    /// <param name="applicantId">The unique identifier of the applicant.</param>
    /// <returns>A list of the applicant's applications.</returns>
    Task<List<Application>> GetApplicationsByApplicant(int applicantId);

    /// <summary>
    /// Retrieves all applications for a specific license type.
    /// </summary>
    /// <param name="licenseTypeId">The unique identifier of the license type.</param>
    /// <returns>A list of matching applications.</returns>
    Task<List<Application>> GetApplicationsByLicenseType(int licenseTypeId);
}
