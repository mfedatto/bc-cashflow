namespace Bc.CashFlow.Domain.Transaction;

public class TransactionFactory
{
	// ReSharper disable once MemberCanBeMadeStatic.Global
	public ITransaction Create(
		int id,
		int userId,
		int accountId,
		TransactionType transactionType,
		decimal amount,
		string? description,
		DateTime transactionDate,
		decimal? transactionFee,
		DateTime? projectedRepaymentDate)
	{
		return new TransactionVo(
			id,
			userId,
			accountId,
			transactionType,
			amount,
			description,
			transactionDate,
			transactionFee,
			projectedRepaymentDate);
	}
}

file record TransactionVo(
	int Id,
	int UserId,
	int AccountId,
	TransactionType TransactionType,
	decimal Amount,
	string? Description,
	DateTime TransactionDate,
	decimal? TransactionFee,
	DateTime? ProjectedRepaymentDate
) : ITransaction;
