using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.DbContext;

namespace Bc.CashFlow.Domain.Transaction;

public interface ITransactionBusiness
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

	Task<IEnumerable<IAccount>> GetAccounts(
		CancellationToken cancellationToken);

	Task<Identity<int>> CreateTransaction(
		int? userId,
		int accountId,
		TransactionType transactionType,
		decimal amount,
		string? description,
		DateTime transactionDate,
		CancellationToken cancellationToken);
}
