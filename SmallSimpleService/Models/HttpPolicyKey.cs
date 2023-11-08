namespace SmallSimpleService.Models;

/// <summary>
/// Possible key to indicate the HTTP policy.
/// </summary>
public enum HttpPolicyKey
{
    /// <summary>
    /// Name of the HTTP retry policy.
    /// </summary>
    Retry,
    
    /// <summary>
    /// Name of the HTTP circuit breaker policy.
    /// </summary>
    CircuitBreaker
}