namespace Bc.CashFlow.Domain.Account;

public class AccountNotFoundException : Exception
{
	public AccountNotFoundException() : base("Account not found.")
	{
	}
}
