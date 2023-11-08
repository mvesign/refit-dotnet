using System;
using System.Collections.Generic;
using System.Linq;
using SharedModels;

namespace SmallSimpleApi.Services;

public class AccountsService
{
    private List<ApiAccount> _storedAccounts;

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
    
    public Guid[] GetAccountIds() =>
        _storedAccounts
            .Select(account => account.Id)
            .ToArray();

    public ApiAccount GetAccount(Guid id) =>
        _storedAccounts
            .FirstOrDefault(account => account.Id.Equals(id));

    public ApiAccount CreateAccount(Guid id)
    {
        var createdAccount = GenerateAccount(id);
        _storedAccounts.Add(createdAccount);
        return createdAccount;
    }

    public ApiAccount UpdateAccount(Guid id)
    {
        var storedAccount = GetAccount(id);
        if (storedAccount != null)
            storedAccount.Counter++;
        return storedAccount;
    }

    public void DeleteAccount(Guid id)
    {
        var storedAccount = GetAccount(id);
        _storedAccounts.Remove(storedAccount);
    }

    private ApiAccount GenerateAccount(Guid id) =>
        new()
        {
            Id = id,
            Counter = 0
        };
}