namespace Bc.CashFlow.Domain.AccountType;

public interface IAccountTypeService
{
	Task<IEnumerable<IAccountType>> GetAccountTypes(
		string? accountTypeName,
		decimal? baseFeeFrom,
		decimal? baseFeeTo,
		int? paymentDueDaysFrom,
		int? paymentDueDaysTo,
		CancellationToken cancellationToken);
}
