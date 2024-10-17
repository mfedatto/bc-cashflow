using System.Diagnostics.CodeAnalysis;

namespace Bc.CashFlow.Domain.User;

[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
public class UserFactory
{
	public IUser Create(
		int userId,
		string username,
		string passwordSalt,
		string passwordHash,
		DateTime createdAt)
	{
		if (userId <= 0) throw new InvalidUserDataException("user ID must be grater than 0");
		if (string.IsNullOrWhiteSpace(username)) throw new InvalidUserDataException("username cannot be null nor white space");
		if (string.IsNullOrWhiteSpace(passwordSalt)) throw new InvalidUserDataException("password salt cannot be null nor white space");
		if (string.IsNullOrWhiteSpace(passwordHash)) throw new InvalidUserDataException("password hash cannot be null nor white space");
		if (createdAt == DateTime.MinValue) throw new InvalidUserDataException("create at cannot be MinDate");

		return new UserVo(
			Id: userId,
			Username: username,
			PasswordSalt: passwordSalt,
			PasswordHash: passwordHash,
			CreatedAt: createdAt
		);
	}
}

file record UserVo(
	int Id,
	string Username,
	string PasswordSalt,
	string PasswordHash,
	DateTime CreatedAt
) : IUser;
