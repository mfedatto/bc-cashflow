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

	public AccountTypeRepository(
		ILogger<AccountTypeRepository> logger,
		DbConnection dbConnection,
		DbTransaction dbTransaction)
	{
		_logger = logger;
		_dbConnection = dbConnection;
		_dbTransaction = dbTransaction;
	}

	public async Task<IEnumerable<IAccountType>> GetAccountTypes(
		string? accountTypeName,
		decimal? baseFeeFrom,
		decimal? baseFeeTo,
		int? paymentDueDaysFrom,
		int? paymentDueDaysTo,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@AccountTypeName", accountTypeName, DbType.String);
		parameters.Add("@BaseFeeFrom", baseFeeFrom, DbType.Decimal);
		parameters.Add("@BaseFeeTo", baseFeeTo, DbType.Decimal);
		parameters.Add("@PaymentDueDaysFrom", paymentDueDaysFrom, DbType.Int32);
		parameters.Add("@PaymentDueDaysTo", paymentDueDaysTo, DbType.Int32);

		return await _dbConnection.QueryAsync<AccountTypeDto>(
			"usp_SelectAccountTypes",
			parameters,
			commandType: CommandType.StoredProcedure,
			transaction: _dbTransaction);
	}
}

file record AccountTypeDto(
	int AccountTypeId,
	string AccountTypeName,
	decimal BaseFee,
	int PaymentDueDays
) : IAccountType;
