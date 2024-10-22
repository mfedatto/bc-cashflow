namespace Bc.CashFlow.Domain.Transaction;

public class TransactionNotFoundException : Exception
{
	public TransactionNotFoundException() : base("Transaction not found.")
	{
	}
}
