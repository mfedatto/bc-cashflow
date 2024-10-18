using System.Data;
using System.Data.Common;
using Bc.CashFlow.Domain.AccountType;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.IO.DbContext;

public class AccountTypeRepository : IAccountTypeRepository
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<AccountTypeRepository> _logger;
	private readonly DbConnection _dbConnection;
	private readonly DbTransaction _dbTransaction;
	private readonly AccountTypeFactory _factory;

	public AccountTypeRepository(
		ILogger<AccountTypeRepository> logger,
		DbConnection dbConnection,
		DbTransaction dbTransaction,
		AccountTypeFactory factory)
	{
		_logger = logger;
		_dbConnection = dbConnection;
		_dbTransaction = dbTransaction;
		_factory = factory;
	}

	public async Task<IEnumerable<IAccountType>> GetAccountTypes(
		string? name,
		decimal? baseFeeFrom,
		decimal? baseFeeTo,
		int? paymentDueDaysFrom,
		int? paymentDueDaysTo,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@AccountTypeName", name, DbType.String);
		parameters.Add("@BaseFeeFrom", baseFeeFrom, DbType.Decimal);
		parameters.Add("@BaseFeeTo", baseFeeTo, DbType.Decimal);
		parameters.Add("@PaymentDueDaysFrom", paymentDueDaysFrom, DbType.Int32);
		parameters.Add("@PaymentDueDaysTo", paymentDueDaysTo, DbType.Int32);

		return (await _dbConnection.QueryAsync<AccountTypeDto>(
				"usp_SelectAccountTypes",
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction))
			.Select(
				row =>
					_factory.Create(
						row.AccountTypeId,
						row.AccountTypeName,
						row.BaseFee,
						row.PaymentDueDays
					));
	}

	public async Task<IAccountType?> GetAccountType(
		int id,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@AccountTypeId", id, DbType.Int32);

		return (await _dbConnection.QueryAsync<AccountTypeDto>(
				"usp_SelectAccountType",
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction))
			.Select(
				row =>
					_factory.Create(
						row.AccountTypeId,
						row.AccountTypeName,
						row.BaseFee,
						row.PaymentDueDays
					))
			.SingleOrDefault();
	}
}

file record AccountTypeDto
{
	public required int AccountTypeId { get; init; }
	public required string AccountTypeName { get; init; }
	public required decimal BaseFee { get; init; }
	public required int PaymentDueDays { get; init; }
}
