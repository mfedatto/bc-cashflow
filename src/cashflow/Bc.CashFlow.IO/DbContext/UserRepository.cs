using System.Data;
using System.Data.Common;
using Bc.CashFlow.Domain.User;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.IO.DbContext;

// ReSharper disable once InconsistentNaming
public class UserRepository : IUserRepository
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<UserRepository> _logger;
	private readonly DbConnection _dbConnection;
	private readonly DbTransaction _dbTransaction;

	public UserRepository(
		ILogger<UserRepository> logger,
		DbConnection dbConnection,
		DbTransaction dbTransaction)
	{
		_logger = logger;
		_dbConnection = dbConnection;
		_dbTransaction = dbTransaction;
	}

	public async Task<IEnumerable<IUser>> GetUsers(
		CancellationToken cancellationToken,
		string? username = null,
		DateTime? createdSince = null,
		DateTime? createdUntil = null)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		DynamicParameters parameters = new();
		
		parameters.Add("@Username", username, DbType.String);
		parameters.Add("@CreatedSince", createdSince, DbType.DateTime);
		parameters.Add("@CreatedUntil", createdUntil, DbType.DateTime);

		return await _dbConnection.QueryAsync<UserDto>(
			"usp_SelectUsers",
			parameters,
			commandType: CommandType.StoredProcedure,
			transaction: _dbTransaction);
	}
}

file record UserDto(
	int UserId,
	string Username,
	string PasswordSalt,
	string PasswordHash,
	DateTime CreatedAt) : IUser;
