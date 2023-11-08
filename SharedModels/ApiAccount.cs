using System;

namespace SharedModels;

/// <summary>
/// Details of an account.
/// </summary>
public class ApiAccount
{
    /// <summary>
    /// Identifier of the account.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Counter to keep track of how many times the account is updated.
    /// </summary>
    public int Counter { get; set; }
}