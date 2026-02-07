/// <summary>
/// Represents the possible approval states of a license application.
/// </summary>
public enum ApplicationStatus
{
    /// <summary>The application is pending review.</summary>
    UnderReview,

    /// <summary>The application has been approved.</summary>
    Approved,

    /// <summary>The application has been denied.</summary>
    Denied,
}
