using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Refit;
using SharedModels;
using SmallSimpleService.RefitClients;

namespace SmallSimpleService.Services;

/// <summary>
/// Service acting like a wrapper around the <see cref="ISmallSimpleApiClient"/> interface.
/// </summary>
public class SmallSimpleApiService
{
    private readonly ILogger _logger;
    private readonly ISmallSimpleApiClient _smallSimpleApiClient;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="smallSimpleApiClient">Small Simple API client interface.</param>
    public SmallSimpleApiService(ILogger logger, ISmallSimpleApiClient smallSimpleApiClient)
    {
        _logger = logger;
        _smallSimpleApiClient = smallSimpleApiClient;
    }

    /// <summary>
    /// Get an overview of all account identifiers.
    /// </summary>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Set of account identifiers.</returns>
    /// <exception cref="ApiException">Thrown when the request wasn't successful.</exception>
    public async Task<Guid[]> GetAccountsAsync(CancellationToken stoppingToken)
    {
        try
        {
            return await _smallSimpleApiClient.GetAccountsAsync(stoppingToken);
        }
        catch (ApiException exception)
        {
            _logger.LogError(
                exception,
                "Could not get overview of accounts - HttpStatusCode: {HttpStatusCode}, ResponseContent: {ResponseContent}",
                exception.StatusCode, exception.Content
            );
            return Array.Empty<Guid>();
        }
    }

    /// <summary>
    /// Get an account linked to an identifier.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    /// <exception cref="ApiException">Thrown when the request wasn't successful.</exception>
    public async Task<ApiAccount> GetAccountAsync(Guid id, CancellationToken stoppingToken)
    {
        try
        {
            return await _smallSimpleApiClient.GetAccountAsync(id, stoppingToken);
        }
        catch (ApiException exception)
        {
            _logger.LogError(
                exception,
                "Could not get account linked to id '{Id}' - HttpStatusCode: {HttpStatusCode}, ResponseContent: {ResponseContent}",
                id, exception.StatusCode, exception.Content
            );
            return null;
        }
    }

    /// <summary>
    /// Create a new account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    /// <exception cref="ApiException">Thrown when the request wasn't successful.</exception>
    public async Task<ApiAccount> CreateAccountAsync(Guid id, CancellationToken stoppingToken)
    {
        try
        {
            return await _smallSimpleApiClient.CreateAccountAsync(id, stoppingToken);
        }
        catch (ApiException exception)
        {
            _logger.LogError(
                exception,
                "Could not create account with id '{Id}' - HttpStatusCode: {HttpStatusCode}, ResponseContent: {ResponseContent}",
                id, exception.StatusCode, exception.Content
            );
            return null;
        }
    }

    /// <summary>
    /// Update an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    /// <exception cref="ApiException">Thrown when the request wasn't successful.</exception>
    public async Task<ApiAccount> UpdateAccountAsync(Guid id, CancellationToken stoppingToken)
    {
        try
        {
            return await _smallSimpleApiClient.UpdateAccountAsync(id, stoppingToken);
        }
        catch (ApiException exception)
        {
            _logger.LogError(
                exception,
                "Could not update account linked to id '{Id}' - HttpStatusCode: {HttpStatusCode}, ResponseContent: {ResponseContent}",
                id, exception.StatusCode, exception.Content
            );
            return null;
        }
    }

    /// <summary>
    /// Delete an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>True when the account is deleted, else false.</returns>
    /// <exception cref="ApiException">Thrown when the request wasn't successful.</exception>
    public async Task<bool> DeleteAccountAsync(Guid id, CancellationToken stoppingToken)
    {
        try
        {
            await _smallSimpleApiClient.DeleteAccountAsync(id, stoppingToken);
            return true;
        }
        catch (ApiException exception)
        {
            _logger.LogError(
                exception,
                "Could not delete account linked to id '{Id}' - HttpStatusCode: {HttpStatusCode}, ResponseContent: {ResponseContent}",
                id, exception.StatusCode, exception.Content
            );
            return false;
        }
    }
}