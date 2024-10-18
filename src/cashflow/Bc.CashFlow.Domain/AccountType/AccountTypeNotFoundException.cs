namespace Bc.CashFlow.Domain.User;

public class AccountTypeNotFoundException : Exception
{
	public AccountTypeNotFoundException() : base("Account type not found.")
	{
	}
}
