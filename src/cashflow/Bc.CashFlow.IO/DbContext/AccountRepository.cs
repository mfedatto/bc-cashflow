using System.Data;
using System.Data.Common;
using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.DbContext;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.IO.DbContext;

public class AccountRepository : IAccountRepository
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<AccountRepository> _logger;
	private readonly DbConnection _dbConnection;
	private readonly DbTransaction _dbTransaction;
	private readonly AccountFactory _factory;

	public AccountRepository(
		ILogger<AccountRepository> logger,
		DbConnection dbConnection,
		DbTransaction dbTransaction,
		AccountFactory factory)
	{
		_logger = logger;
		_dbConnection = dbConnection;
		_dbTransaction = dbTransaction;
		_factory = factory;
	}

	public async Task<IEnumerable<Identity<int>>> GetAccountsId(
		int? userId,
		int? accountTypeId,
		string? name,
		decimal? initialBalanceFrom,
		decimal? initialBalanceTo,
		decimal? currentBalanceFrom,
		decimal? currentBalanceTo,
		DateTime? balanceUpdatedAtSince,
		DateTime? balanceUpdatedAtUntil,
		DateTime? createdAtSince,
		DateTime? createdAtUntil,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@UserId", userId, DbType.Int32);
		parameters.Add("@AccountTypeId", accountTypeId, DbType.Int32);
		parameters.Add("@AccountName", name, DbType.String);
		parameters.Add("@InitialBalanceFrom", initialBalanceFrom, DbType.Decimal);
		parameters.Add("@InitialBalanceTo", initialBalanceTo, DbType.Decimal);
		parameters.Add("@CurrentBalanceFrom", currentBalanceFrom, DbType.Decimal);
		parameters.Add("@CurrentBalanceTo", currentBalanceTo, DbType.Decimal);
		parameters.Add("@BalanceUpdatedSince", balanceUpdatedAtSince, DbType.DateTime);
		parameters.Add("@BalanceUpdatedUntil", balanceUpdatedAtUntil, DbType.DateTime);
		parameters.Add("@CreatedSince", createdAtSince, DbType.DateTime);
		parameters.Add("@CreatedUntil", createdAtUntil, DbType.DateTime);

		return (await _dbConnection.QueryAsync<AccountIdDto>(
				"usp_SelectAccounts",
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction))
			.Select(
				row =>
					new Identity<int>
					{
						Value = row.AccountId
					});
	}

	public async Task<IAccount?> GetAccount(
		int id,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@AccountId", id, DbType.Int32);

		return (await _dbConnection.QueryAsync<AccountDto>(
				"usp_SelectAccount",
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction))
			.Select(
				row =>
					_factory.Create(
						row.AccountId,
						row.UserId,
						row.AccountTypeId,
						row.AccountName,
						row.InitialBalance,
						row.CurrentBalance,
						row.BalanceUpdatedAt,
						row.CreatedAt
					))
			.SingleOrDefault();
	}
}

file record AccountIdDto
{
	public required int AccountId { get; init; }
}

file record AccountDto
{
	public required int AccountId { get; init; }
	public required int UserId { get; init; }
	public required int AccountTypeId { get; init; }
	public required string AccountName { get; init; }
	public required decimal InitialBalance { get; init; }
	public required decimal CurrentBalance { get; init; }
	public required DateTime BalanceUpdatedAt { get; init; }
	public required DateTime CreatedAt { get; init; }
}
