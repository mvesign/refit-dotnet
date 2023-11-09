using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedModels;
using SmallSimpleService.Services;
using SmallSimpleService.Settings;

namespace SmallSimpleService;

/// <summary>
/// Default worker service.
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IOptionsMonitor<SmallSimpleServiceSettings> _smallSimpleServiceSettings;
    private readonly SmallSimpleApiService _smallSimpleApiService;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="smallSimpleServiceSettings">Small simple service settings of type <see cref="SmallSimpleServiceSettings"/>.</param>
    /// <param name="smallSimpleApiService">Small Simple API service of type <see cref="SmallSimpleApiService"/>.</param>
    public Worker(
        ILogger<Worker> logger, IOptionsMonitor<SmallSimpleServiceSettings> smallSimpleServiceSettings,
        SmallSimpleApiService smallSimpleApiService
    )
    {
        _logger = logger;
        _smallSimpleServiceSettings = smallSimpleServiceSettings;
        _smallSimpleApiService = smallSimpleApiService;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // First let us try to get all account identifiers from the API.
            var accountIds = await _smallSimpleApiService.GetAccountIdsAsync(stoppingToken);
            if (accountIds.Length > 0)
                foreach (var accountId in accountIds)
                    // Than we process each account identifier with an appropriate and pre-defined action. 
                    await ProcessAccountIdAsync(accountId, stoppingToken);

            // And after all that, we wait for several seconds so we can do all over again.
            Thread.Sleep(_smallSimpleServiceSettings.CurrentValue.SleepPeriodInMilliseconds);
        }
    }

    private async Task ProcessAccountIdAsync(Guid accountId, CancellationToken stoppingToken)
    {
        // Per account we try to get the actual details of that account.
        var account = await _smallSimpleApiService.GetAccountAsync(accountId, stoppingToken);

        // But if no account is found, we try to create a new one.
        if (account == null)
            await FollowCreateFlowAsync(accountId, stoppingToken);
        // Otherwise we will update the existing one by incrementing the update counter.
        // And only if the account isn't updated for to many times.
        else if (account.Counter < _smallSimpleServiceSettings.CurrentValue.DeleteAccountAfterUpdates)
            await FollowUpdateFlowAsync(account, stoppingToken);
        // But if the account is updated for to many times, we delete it.
        else
            await FollowDeleteFlowAsync(account, stoppingToken);
    }

    private async Task FollowCreateFlowAsync(Guid accountId, CancellationToken stoppingToken)
    {
        var createdAccount = await _smallSimpleApiService.CreateAccountAsync(accountId, stoppingToken);
        if (createdAccount == null)
            _logger.LogWarning("Account '{Id}' not created", accountId);
        else
            _logger.LogInformation("Account '{Id}' created - Counter: {Counter}", accountId, createdAccount.Counter);
    }

    private async Task FollowUpdateFlowAsync(ApiAccount account, CancellationToken stoppingToken)
    {
        var updatedAccount = await _smallSimpleApiService.UpdateAccountAsync(account.Id, stoppingToken);
        if (updatedAccount != null && updatedAccount.Counter == account.Counter + 1)
            _logger.LogInformation("Account '{Id}' updated - Counter: {Counter}", updatedAccount.Id, updatedAccount.Counter);
        else
            _logger.LogWarning("Account '{Id}' not updated", account.Id);
    }

    private async Task FollowDeleteFlowAsync(ApiAccount account, CancellationToken stoppingToken)
    {
        var isDeleted = await _smallSimpleApiService.DeleteAccountAsync(account.Id, stoppingToken);
        var deletedAccount = await _smallSimpleApiService.GetAccountAsync(account.Id, stoppingToken);
        if (isDeleted && (deletedAccount == null || deletedAccount.Counter < account.Counter))
            _logger.LogInformation("Account '{Id}' deleted", account.Id);
        else
            _logger.LogWarning("Account '{Id}' not deleted", account.Id);
    }
}