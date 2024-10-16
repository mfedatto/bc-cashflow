namespace Bc.CashFlow.Domain.User;

public interface IUserService
{
	Task<IEnumerable<IUser>> GetUsers(
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null);
}
