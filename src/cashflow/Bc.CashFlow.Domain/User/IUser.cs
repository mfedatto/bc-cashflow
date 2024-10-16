namespace Bc.CashFlow.Domain.User;

public interface IUser
{
	int UserId { get; }
	string Username { get; }
	string PasswordSalt { get; }
	string PasswordHash { get; }
	DateTime CreatedAt { get; }
}
