namespace Bc.CashFlow.Domain.AccountType;

public class AccountTypeNotFoundException : Exception
{
	public AccountTypeNotFoundException() : base("Account type not found.")
	{
	}
}
