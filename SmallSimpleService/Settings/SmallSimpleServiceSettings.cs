namespace SmallSimpleService.Settings;

/// <summary>
/// Generic settings for the small simple service.
/// </summary>
public class SmallSimpleServiceSettings
{
    /// <summary>
    /// Number of updated before an account will be requested for deletion.
    /// </summary>
    public int DeleteAccountAfterUpdates { get; set; } = 10;

    /// <summary>
    /// Sleep period between process rounds, in milliseconds.
    /// </summary>
    public int SleepPeriodInMilliseconds { get; set; } = 1000;
}