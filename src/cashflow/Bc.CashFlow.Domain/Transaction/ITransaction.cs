namespace Bc.CashFlow.Domain.Transaction;

public interface ITransaction
{
	int Id { get; }
	int UserId { get; }
	int AccountId { get; }
	TransactionType TransactionType { get; }
	decimal Amount { get; }
	string? Description { get; }
	DateTime TransactionDate { get; }
	decimal? TransactionFee { get; }
	DateTime? ProjectedRepaymentDate { get; }
}