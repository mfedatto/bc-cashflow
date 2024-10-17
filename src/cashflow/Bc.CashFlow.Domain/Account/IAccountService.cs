namespace Bc.CashFlow.Domain.Account;

public interface IAccountService
{
	Task<IEnumerable<IAccount>> GetAccounts(
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
		CancellationToken cancellationToken);
}
