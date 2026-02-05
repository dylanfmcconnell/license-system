public sealed class ApplicationSubmitRequest
{
    public int ApplicantId { get; set; }
    public int LicenseTypeId { get; set; }
    public required string DeliveryAddress { get; set; }
}
