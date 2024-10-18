using System.Data;
using System.Data.Common;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.IO.DbContext;

public class TransactionRepository : ITransactionRepository
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<TransactionRepository> _logger;
	private readonly DbConnection _dbConnection;
	private readonly DbTransaction _dbTransaction;
	private readonly TransactionFactory _factory;

	public TransactionRepository(
		ILogger<TransactionRepository> logger,
		DbConnection dbConnection,
		DbTransaction dbTransaction,
		TransactionFactory factory)
	{
		_logger = logger;
		_dbConnection = dbConnection;
		_dbTransaction = dbTransaction;
		_factory = factory;
	}

	public async Task<IEnumerable<ITransaction>> GetTransactions(
		int? userId,
		int? accountId,
		TransactionType? transactionType,
		decimal? amountFrom,
		decimal? amountTo,
		DateTime? transactionDateSince,
		DateTime? transactionDateUntil,
		DateTime? projectedRepaymentDateSince,
		DateTime? projectedRepaymentDateUntil,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@UserId", userId, DbType.Int32);
		parameters.Add("@AccountId", accountId, DbType.Int32);
		parameters.Add("@TransactionType", transactionType, DbType.Boolean);
		parameters.Add("@AmountFrom", amountFrom, DbType.Decimal);
		parameters.Add("@AmountTo", amountTo, DbType.Decimal);
		parameters.Add("@TransactionDateSince", transactionDateSince, DbType.DateTime);
		parameters.Add("@TransactionDateUntil", transactionDateUntil, DbType.DateTime);
		parameters.Add("@ProjectedRepaymentDateSince", projectedRepaymentDateSince, DbType.DateTime);
		parameters.Add("@ProjectedRepaymentDateUntil", projectedRepaymentDateUntil, DbType.DateTime);

		return (await _dbConnection.QueryAsync<TransactionDto>(
				"usp_SelectTransactions",
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction))
			.Select(
				row =>
					_factory.Create(
						row.TransactionId,
						row.UserId,
						row.AccountId,
						(TransactionType)row.TransactionType,
						row.Amount,
						row.Description,
						row.TransactionDate,
						row.TransactionFee,
						row.ProjectedRepaymentDate
					));
	}

	public async Task<Identity<int>?> CreateTransaction(
		int? userId,
		int accountId,
		TransactionType transactionType,
		decimal amount,
		string? description,
		DateTime transactionDate,
		decimal? transactionFee,
		DateTime? projectedRepaymentDate,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		DynamicParameters parameters = new();

		parameters.Add("@UserId", userId, DbType.Int32);
		parameters.Add("@AccountId", accountId, DbType.Int32);
		parameters.Add("@TransactionType", transactionType, DbType.Boolean);
		parameters.Add("@Amount", amount, DbType.Decimal);
		parameters.Add("@Description", description, DbType.String);
		parameters.Add("@TransactionDate", transactionDate, DbType.DateTime);
		parameters.Add("@TransactionFee", transactionFee, DbType.Decimal);
		parameters.Add("@ProjectedRepaymentDate", projectedRepaymentDate, DbType.DateTime);

		return (await _dbConnection.QueryAsync<TransactionIdDto>(
				"usp_InsertTransaction",
				parameters,
				commandType: CommandType.StoredProcedure,
				transaction: _dbTransaction))
			.Select(
				row =>
					new Identity<int>
					{
						Value = row.Id
					})
			.SingleOrDefault();
	}
}

file record TransactionIdDto
{
	public int Id { get; init; }
}

file record TransactionDto
{
	public int TransactionId { get; init; }
	public int UserId { get; init; }
	public int AccountId { get; init; }
	public int TransactionType { get; init; }
	public decimal Amount { get; init; }
	public string? Description { get; init; }
	public DateTime TransactionDate { get; init; }
	public decimal? TransactionFee { get; init; }
	public DateTime? ProjectedRepaymentDate { get; init; }
}
