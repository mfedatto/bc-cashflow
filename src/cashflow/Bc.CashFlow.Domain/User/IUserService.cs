namespace Bc.CashFlow.Domain.User;

public interface IUserService
{
	Task<IEnumerable<IUser>> GetUsers(
		CancellationToken cancellationToken,
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null);
}
