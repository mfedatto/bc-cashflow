using System.Data.Common;
using Bc.CashFlow.Domain.AppSettings;
using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.IO.DbContext;

// ReSharper disable once InconsistentNaming
public class UserRepository : IUserRepository
{
	private readonly ILogger<UserRepository> _logger;
	private readonly DbConnection _dbConnection;
	private readonly DbTransaction _dbTransaction;
	private readonly UserFactory _factory;

	public UserRepository(
		ILogger<UserRepository> logger,
		DbConnection dbConnection,
		DbTransaction dbTransaction,
		UserFactory factory)
	{
		_logger = logger;
		_dbConnection = dbConnection;
		_dbTransaction = dbTransaction;
		_factory = factory;
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
