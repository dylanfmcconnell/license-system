/// <summary>
/// Represents the possible states of an issued license.
/// </summary>
public enum LicenseStatus
{
    /// <summary>The license is currently active and valid.</summary>
    Valid,

    /// <summary>The license has passed its expiration date.</summary>
    Expired,

    /// <summary>The license has been temporarily suspended.</summary>
    Suspended,

    /// <summary>The license has been permanently revoked.</summary>
    Revoked,
}
