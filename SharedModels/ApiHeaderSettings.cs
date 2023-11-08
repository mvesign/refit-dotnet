namespace SharedModels;

/// <summary>
/// Pre-defined settings to setup and check API security headers.
/// </summary>
public static class ApiHeaderSettings
{
    /// <summary>
    /// Default allowed key of the required API header. 
    /// </summary>
    public const string ApiHeaderKey = "X-SECURITY-HEADER";

    /// <summary>
    /// Default allowed value of the required API header. 
    /// </summary>
    public const string ApiHeaderValue = "SOME-SECRET-PASSWORD";
}