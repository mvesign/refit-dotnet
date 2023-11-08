using System;
using System.Collections.Generic;
using System.Linq;
using SharedModels;

namespace SmallSimpleApi.Services;

/// <summary>
/// Service for managing the fake and setup API accounts.
/// </summary>
public class AccountsService
{
    private readonly List<ApiAccount> _storedAccounts;

    /// <summary>
    /// Ctor.
    /// </summary>
    public AccountsService()
    {
        _storedAccounts = Enumerable
            .Range(0, 10)
            .Select(_ => GenerateAccount(Guid.NewGuid()))
            .ToList();
    }

    /// <summary>
    /// Get an overview of all account identifiers.
    /// </summary>
    /// <returns>Set of account identifiers.</returns>
    public Guid[] GetAccountIds() =>
        _storedAccounts
            .Select(account => account.Id)
            .ToArray();

    /// <summary>
    /// Get an account linked to an identifier.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    public ApiAccount GetAccount(Guid id) =>
        _storedAccounts
            .FirstOrDefault(account => account.Id.Equals(id));

    /// <summary>
    /// Create a new account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    public ApiAccount CreateAccount(Guid id)
    {
        var storedAccount = GetAccount(id);
        if (storedAccount != null)
            return storedAccount;

        var createdAccount = GenerateAccount(id);
        _storedAccounts.Add(createdAccount);
        return createdAccount;
    }

    /// <summary>
    /// Update an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    public ApiAccount UpdateAccount(Guid id)
    {
        var storedAccount = GetAccount(id);
        if (storedAccount != null)
            storedAccount.Counter++;
        return storedAccount;
    }

    /// <summary>
    /// Delete an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>True when the account is deleted, else false.</returns>
    public void DeleteAccount(Guid id)
    {
        var storedAccount = GetAccount(id);
        _storedAccounts.Remove(storedAccount);
    }

    private static ApiAccount GenerateAccount(Guid id) =>
        new()
        {
            Id = id,
            Counter = 0
        };
}