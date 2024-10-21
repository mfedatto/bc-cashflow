using Bc.CashFlow.Domain.DbContext;

namespace Bc.CashFlow.Domain.Transaction;

public interface ITransactionRepository
{
	Task<IEnumerable<ITransaction>> GetTransactions(
		int? userId,
		int? accountId,
		TransactionType? transactionType,
		decimal? amountFrom,
		decimal? amountTo,
		DateTime? transactionDateSince,
		DateTime? transactionDateUntil,
		DateTime? projectedRepaymentDateSince,
		DateTime? projectedRepaymentDateUntil,
		CancellationToken cancellationToken);

	Task<IEnumerable<ITransaction>> GetTransactionsByProjectedRepaymentDate(
		int? accountId,
		DateTime projectedRepaymentDate,
		CancellationToken cancellationToken);

	Task<Identity<int>?> CreateTransaction(
		int? userId,
		int accountId,
		TransactionType transactionType,
		decimal amount,
		string? description,
		DateTime transactionDate,
		decimal? transactionFee,
		DateTime? projectedRepaymentDate,
		CancellationToken cancellationToken);
}
