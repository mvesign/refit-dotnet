using System;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SmallSimpleApi.Services;

namespace SmallSimpleApi.Controllers;

/// <summary>
/// API controller to manage accounts.
/// </summary>
[ApiController]
[Route("v1.0/accounts")]
public class AccountsController : ControllerBase
{
    private readonly AccountsService _accountsService;

    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="accountsService">Accounts service.</param>
    public AccountsController(AccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    /// <summary>
    /// Get an overview of all account identifiers.
    /// </summary>
    /// <returns>Set of account identifiers.</returns>
    [HttpGet]
    [Produces(typeof(Guid[]))]
    public IActionResult GetAccountIdsAsync() =>
        Ok(
            _accountsService.GetAccountIds()
        );

    /// <summary>
    /// Get an account linked to an identifier.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    [HttpGet("id")]
    [Produces(typeof(ApiAccount))]
    public IActionResult GetAccountAsync(Guid id) =>
        Ok(
            _accountsService.GetAccount(id)
        );

    /// <summary>
    /// Create a new account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    [HttpPost("id")]
    [Produces(typeof(ApiAccount))]
    public IActionResult CreateAccountAsync(Guid id) =>
        Ok(
            _accountsService.CreateAccount(id)
        );

    /// <summary>
    /// Update an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    /// <returns>Account of type <see cref="ApiAccount"/>.</returns>
    [HttpPut("id")]
    [Produces(typeof(ApiAccount))]
    public IActionResult UpdateAccountAsync(Guid id) =>
        Ok(
            _accountsService.UpdateAccount(id)
        );

    /// <summary>
    /// Delete an existing account.
    /// </summary>
    /// <param name="id">Identifier of an account.</param>
    [HttpDelete("id")]
    [Produces(typeof(ApiAccount))]
    public IActionResult DeleteAccountAsync(Guid id)
    {
        _accountsService.DeleteAccount(id);
        return NoContent();
    }
}