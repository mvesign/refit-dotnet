using System;

namespace SmallSimpleService.Settings;

/// <summary>
/// Settings related to setup the HTTP policies.
/// </summary>
public class HttpPoliciesSettings
{
    /// <summary>
    /// Number of regular retries for the wait and retry policy.
    /// </summary>
    public int WaitAndRetryCount { get; set; } = 6;

    /// <summary>
    /// Number of handled exception before the circuit breaker policy will kick in.
    /// </summary>
    /// <remarks>
    /// Be aware that when this number is lower than <see cref="WaitAndRetryCount"/>, the circuit will opened sooner.
    /// Resulting in less retries than expected, when using the <see cref="WaitAndRetryCount"/> as well.
    /// </remarks>
    public int CircuitBreakerRetryCount { get; set; } = 7;

    /// <summary>
    /// Duration of the break before the circuit breaker policy will be resolved.
    /// </summary>
    public TimeSpan CircuitBreakerDuration { get; set; } = TimeSpan.FromSeconds(10);
}