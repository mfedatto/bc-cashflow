using Bc.CashFlow.Domain.MainDbContext;
using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Services;

public class UserService : IUserService
{
	private readonly ILogger<UserService> _logger;
	private readonly IUnitOfWork _uow;

	public UserService(
		ILogger<UserService> logger,
		IUnitOfWork uow)
	{
		_logger = logger;
		_uow = uow;
	}
	
	public async Task<IEnumerable<IUser>> GetUsers(
		CancellationToken cancellationToken,
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null)
	{
		return await _uow.UserRepository.GetUsers(
			cancellationToken,
			username,
			createdSince,
			createdUntil);
	}
}
