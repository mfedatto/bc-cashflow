namespace Bc.CashFlow.Domain.User;

public class UserNotFoundException : Exception
{
	public UserNotFoundException() : base("User not found.")
	{
	}
}