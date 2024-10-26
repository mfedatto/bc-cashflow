using Bc.CashFlow.Domain.Transaction;

namespace Bc.CashFlow.Web.Models.Transaction;

public class TransactionDetailsViewModel
{
	public TransactionDetailsViewModel(
		ITransaction transaction)
	{
		Transaction = transaction;
	}
	
	public ITransaction Transaction { get; }
}
