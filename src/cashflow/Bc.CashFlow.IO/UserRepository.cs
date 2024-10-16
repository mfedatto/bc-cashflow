using Bc.CashFlow.Domain.AppSettings;
using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.IO;

// ReSharper disable once InconsistentNaming
public class UserRepository : IUserRepository
{
	private readonly ILogger<UserRepository> _logger;
	private readonly DatabaseConfig _databaseConfig;

	public UserRepository(
		ILogger<UserRepository> logger,
		DatabaseConfig databaseConfig)
	{
		_logger = logger;
		_databaseConfig = databaseConfig;
	}
	
	public async Task<IEnumerable<IUser>> GetUsers(
		CancellationToken cancellationToken,
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null)
	{
		throw new NotImplementedException();
	}
}
