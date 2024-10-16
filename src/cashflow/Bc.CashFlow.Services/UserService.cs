using Bc.CashFlow.Domain.User;

namespace Bc.CashFlow.Services;

public class UserService : IUserService
{
	public async Task<IEnumerable<IUser>> GetUsers(
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null)
	{
		throw new NotImplementedException();
	}
}
