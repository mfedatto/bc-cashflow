using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Business;

public class TransactionBusiness : ITransactionBusiness
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<TransactionBusiness> _logger;
	private readonly ITransactionService _transactionService;
	private readonly IAccountService _accountService;

	public TransactionBusiness(
		ILogger<TransactionBusiness> logger,
		ITransactionService transactionService,
		IAccountService accountService)
	{
		_logger = logger;
		_transactionService = transactionService;
		_accountService = accountService;
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
		return await _transactionService.GetTransactions(
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

	public async Task<IEnumerable<IAccount>> GetAccounts(
		CancellationToken cancellationToken)
	{
		return await _accountService.GetAccounts(
			cancellationToken);
	}

	public async Task<Identity<int>> CreateTransaction(
		int? userId,
		int accountId,
		TransactionType transactionType,
		decimal amount,
		string? description,
		DateTime transactionDate,
		CancellationToken cancellationToken)
	{
		return (await _transactionService.CreateTransaction(
			userId,
			accountId,
			transactionType,
			amount,
			description,
			transactionDate,
			cancellationToken))!;
	}
}
