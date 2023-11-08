using System.Collections.Generic;

namespace SmallSimpleService.Settings;

/// <summary>
/// Settings related to setup HTTP client utilities.
/// </summary>
public class HttpClientSettings
{
    /// <summary>
    /// Base URL of the API.
    /// </summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// Set of headers required when sending requests to the API.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; }
}