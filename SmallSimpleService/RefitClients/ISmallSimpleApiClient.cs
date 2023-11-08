using System;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using SharedModels;

namespace SmallSimpleService.RefitClients;

/// <summary>
/// Refit client definition for the Small Simple API.
/// </summary>
public interface ISmallSimpleApiClient
{
    /// <summary>
    /// Get an overview of all account identifiers.
    /// </summary>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Set of account identifiers.</returns>
    [Get("/accounts")]
    Task<Guid[]> GetAccountsAsync(CancellationToken stoppingToken);

    /// <summary>
    /// Get an account linked to an identifier.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    [Get("/accounts/{id}")]
    Task<ApiAccount> GetAccountAsync(Guid id, CancellationToken stoppingToken);

    /// <summary>
    /// Create a new account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    [Post("/accounts/{id}")]
    Task<ApiAccount> CreateAccountAsync(Guid id, CancellationToken stoppingToken);

    /// <summary>
    /// Update an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    [Put("/accounts/{id}")]
    Task<ApiAccount> UpdateAccountAsync(Guid id, CancellationToken stoppingToken);

    /// <summary>
    /// Delete an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <param name="stoppingToken">Token to stop the request.</param>
    [Delete("/accounts/{id}")]
    Task DeleteAccountAsync(Guid id, CancellationToken stoppingToken);
}