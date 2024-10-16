using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Services;

public class UserService : IUserService
{
	private readonly ILogger<UserService> _logger;
	private readonly IUserRepository _repository;

	public UserService(
		ILogger<UserService> logger,
		IUserRepository repository)
	{
		_logger = logger;
		_repository = repository;
	}
	
	public async Task<IEnumerable<IUser>> GetUsers(
		CancellationToken cancellationToken,
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null)
	{
		return await _repository.GetUsers(
			cancellationToken,
			username,
			createdSince,
			createdUntil);
	}
}
