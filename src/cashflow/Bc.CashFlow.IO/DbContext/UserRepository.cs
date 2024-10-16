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
		string? username,
		DateTime? createdSince,
		DateTime? createdUntil,
		CancellationToken cancellationToken)
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

	public async Task<IUser?> GetSingleUser(
		int userId,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@UserId", userId, DbType.Int32);

		return (await _dbConnection.QueryAsync<UserDto>(
				"usp_SelectUser",
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction))
			.SingleOrDefault();
	}
}

file record UserDto(
	int UserId,
	string Username,
	string PasswordSalt,
	string PasswordHash,
	DateTime CreatedAt) : IUser;
