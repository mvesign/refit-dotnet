using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using SharedModels;
using SmallSimpleApi.Settings;

namespace SmallSimpleApi.Services;

/// <summary>
/// Service for managing the fake and setup API accounts.
/// </summary>
public class AccountsService
{
    private readonly List<ApiAccount> _storedAccounts = new();

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="accountSettings">Account settings of type <see cref="AccountSettings"/>.</param>
    public AccountsService(IOptions<AccountSettings> accountSettings)
    {
        Enumerable
            .Range(0, accountSettings.Value.NumberOfAccounts)
            .ToList()
            .ForEach(_ => GenerateAccount(Guid.NewGuid()));
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
    /// If no account is found, a new one will be created.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    public ApiAccount GetAccount(Guid id)
    {
        var storedAccount = _storedAccounts.FirstOrDefault(account => account.Id.Equals(id));
        if (storedAccount != null)
            return storedAccount;

        return GenerateAccount(id);
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

    private ApiAccount GenerateAccount(Guid id)
    {
        var createdAccount = new ApiAccount
        {
            Id = id,
            Counter = 0
        };
        _storedAccounts.Add(createdAccount);
        return createdAccount;
    }
}