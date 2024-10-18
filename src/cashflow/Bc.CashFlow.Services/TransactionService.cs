using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Services;

public class TransactionService : ITransactionService
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<TransactionService> _logger;
	private readonly IUnitOfWork _uow;

	public TransactionService(
		ILogger<TransactionService> logger,
		IUnitOfWork uow)
	{
		_logger = logger;
		_uow = uow;
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
		return await _uow.TransactionRepository.GetTransactions(
			userId,
			accountId,
			transactionType,
			amountFrom,
			amountTo,
			transactionDateSince,
			transactionDateUntil,
			projectedRepaymentDateSince,
			projectedRepaymentDateUntil,
			cancellationToken);
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
		return await _uow.TransactionRepository.CreateTransaction(
			userId,
			accountId,
			transactionType,
			amount,
			description,
			transactionDate,
			transactionFee,
			projectedRepaymentDate,
			cancellationToken);
	}
}
