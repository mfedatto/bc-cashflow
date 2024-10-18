namespace Bc.CashFlow.Domain.User;

public class AccountNotFoundException : Exception
{
	public AccountNotFoundException() : base("Account not found.")
	{
	}
}
