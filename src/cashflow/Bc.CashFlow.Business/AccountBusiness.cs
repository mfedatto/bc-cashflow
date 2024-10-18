using Bc.CashFlow.Domain.Account;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Business;

public class AccountBusiness : IAccountBusiness
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<AccountBusiness> _logger;
	private readonly IAccountService _accountService;

	public AccountBusiness(
		ILogger<AccountBusiness> logger,
		IAccountService accountService)
	{
		_logger = logger;
		_accountService = accountService;
	}

	public async Task<IEnumerable<IAccount>> GetAccounts(
		CancellationToken cancellationToken)
	{
		return await _accountService.GetAccounts(
			cancellationToken);
	}

	public async Task<IEnumerable<IAccount>> GetAccounts(
		int? userId,
		int? accountTypeId,
		string? name,
		decimal? initialBalanceFrom,
		decimal? initialBalanceTo,
		decimal? currentBalanceFrom,
		decimal? currentBalanceTo,
		DateTime? balanceUpdatedAtSince,
		DateTime? balanceUpdatedAtUntil,
		DateTime? createdAtSince,
		DateTime? createdAtUntil,
		CancellationToken cancellationToken)
	{
		return await _accountService.GetAccounts(
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
	}

	public async Task<IAccount?> GetAccount(
		int id,
		CancellationToken cancellationToken)
	{
		return await _accountService.GetAccount(
			id,
			cancellationToken);
	}
}
