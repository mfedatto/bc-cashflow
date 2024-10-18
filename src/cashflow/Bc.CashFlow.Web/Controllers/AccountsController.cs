using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Web.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace Bc.CashFlow.Web.Controllers;

public class AccountsController : Controller
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<AccountsController> _logger;
	private readonly IAccountBusiness _business;

	public AccountsController(
		ILogger<AccountsController> logger,
		IAccountBusiness business)
	{
		_logger = logger;
		_business = business;
	}

	[HttpGet]
	public async Task<IActionResult> Index(
		[FromQuery(Name = "user-id")] int? userId,
		[FromQuery(Name = "account-type-id")] int? accountTypeId,
		[FromQuery(Name = "name")] string? name,
		[FromQuery(Name = "initial-balance-from")] decimal? initialBalanceFrom,
		[FromQuery(Name = "initial-balance-to")] decimal? initialBalanceTo,
		[FromQuery(Name = "current-balance-from")] decimal? currentBalanceFrom,
		[FromQuery(Name = "current-balance-to")] decimal? currentBalanceTo,
		[FromQuery(Name = "balance-updated-at-since")] DateTime? balanceUpdatedAtSince,
		[FromQuery(Name = "balance-updated-at-until")] DateTime? balanceUpdatedAtUntil,
		[FromQuery(Name = "created-at-since")] DateTime? createdAtSince,
		[FromQuery(Name = "created-at-until")] DateTime? createdAtUntil,
		CancellationToken cancellationToken)
	{
		IEnumerable<IAccount> accounts = await _business.GetAccounts(
			userId,
			accountTypeId,
			name,
			initialBalanceFrom,
			initialBalanceTo,
			currentBalanceFrom,
			currentBalanceTo,
			balanceUpdatedAtSince,
			balanceUpdatedAtUntil,
			createdAtSince,
			createdAtUntil,
			cancellationToken);
		AccountIndexViewModel viewModel = new(accounts);
		
		return View(viewModel);
	}
}
