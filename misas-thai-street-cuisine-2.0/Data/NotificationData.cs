namespace misas_thai_street_cuisine_2._0.Data;

public static class NotificationData
{
    /// <summary>
    /// Set to true to show the notification banner on the home page.
    /// </summary>
    public static bool IsActive { get; } = true;

    /// <summary>
    /// The title displayed prominently in the banner.
    /// </summary>
    public static string Title { get; } = "We're on Hiatus";

    /// <summary>
    /// The body message displayed below the title.
    /// </summary>
    public static string Message { get; } = "We're taking a short break! Ordering and catering requests are temporarily disabled. Follow us on social media for updates on when we'll be back.";

    /// <summary>
    /// FontAwesome icon class (e.g. "fas fa-info-circle", "fas fa-exclamation-triangle").
    /// </summary>
    public static string IconClass { get; } = "fas fa-circle-pause";

    /// <summary>
    /// Whether the user can dismiss/close the banner.
    /// </summary>
    public static bool IsDismissable { get; } = false;
}
