using Bc.CashFlow.Domain.DbContext;
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
		string? username,
		DateTime? createdSince,
		DateTime? createdUntil,
		CancellationToken cancellationToken)
	{
		return await _uow.UserRepository.GetUsers(
			username,
			createdSince,
			createdUntil,
			cancellationToken);
	}
	
	public async Task<IUser> GetSingleUser(
		int userId,
		CancellationToken cancellationToken)
	{
		IUser? result = await _uow.UserRepository.GetSingleUser(
			userId,
			cancellationToken);

		if (result is null) throw new UserNotFoundException();
		
		return result;
	}
}
