namespace Bc.CashFlow.Domain.User;

// ReSharper disable once InconsistentNaming
public interface IUserRepository
{
	Task<IEnumerable<IUser>> GetUsers(
		CancellationToken cancellationToken,
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null);
}
