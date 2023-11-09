using System;
using System.Text.Json.Serialization;

namespace SharedModels;

/// <summary>
/// Details of an account.
/// </summary>
public class ApiAccount
{
    /// <summary>
    /// Identifier of the account.
    /// </summary>
    [JsonPropertyName("Id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Counter to keep track of how many times the account is updated.
    /// </summary>
    [JsonPropertyName("Counter")]
    public int Counter { get; set; }
}