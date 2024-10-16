namespace Bc.CashFlow.Domain.User;

public class InvalidUserDataException : Exception
{
	public InvalidUserDataException() : base("Invalid uer data.")
	{
	}
	
	public InvalidUserDataException(
		string details) : base($"Invalid uer data: {details}.")
	{
	}
}
