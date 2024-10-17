namespace Bc.CashFlow.Domain.Transaction;

public class TransactionFactory
{
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
			Id: id,
			UserId: userId,
			AccountId: accountId,
			TransactionType: transactionType,
			Amount: amount,
			Description: description,
			TransactionDate: transactionDate,
			TransactionFee: transactionFee,
			ProjectedRepaymentDate: projectedRepaymentDate);
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
