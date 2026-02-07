/// <summary>
/// Data transfer object for submitting a new license application.
/// </summary>
public sealed class ApplicationSubmitRequest
{
    /// <summary>Gets or sets the identifier of the applicant submitting the application.</summary>
    public int ApplicantId { get; set; }

    /// <summary>Gets or sets the identifier of the desired license type.</summary>
    public int LicenseTypeId { get; set; }

    /// <summary>Gets or sets the address where the license should be delivered.</summary>
    public required string DeliveryAddress { get; set; }
}
