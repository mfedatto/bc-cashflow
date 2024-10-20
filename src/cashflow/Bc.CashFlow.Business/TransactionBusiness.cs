using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Business;

public class TransactionBusiness : ITransactionBusiness
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<TransactionBusiness> _logger;
	private readonly ITransactionService _transactionService;
	private readonly IAccountService _accountService;
	private readonly IAccountTypeService _accountTypeService;
	private readonly IUserService _userService;

	public TransactionBusiness(
		ILogger<TransactionBusiness> logger,
		ITransactionService transactionService,
		IAccountService accountService,
		IAccountTypeService accountTypeService,
		IUserService userService)
	{
		_logger = logger;
		_transactionService = transactionService;
		_accountService = accountService;
		_accountTypeService = accountTypeService;
		_userService = userService;
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
		IAccount account = await GetRequiredAccount(
			accountId,
			cancellationToken);
		IUser? user = userId is null
			? null
			: await GetRequiredUser(
				userId.Value,
				cancellationToken);
		Identity<int> result = await CreateRequiredTransaction(
			user,
			account,
			transactionType,
			amount,
			description,
			transactionDate,
			cancellationToken);

		await _transactionService.PublishNewTransactionToBalance(
			result.Value,
			cancellationToken);

		return result;
	}

	private async Task<IAccount> GetRequiredAccount(
		int id,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		IAccount? result = await _accountService.GetAccount(
			id,
			cancellationToken);

		if (result is null) throw new AccountNotFoundException();

		return result;
	}

	private async Task<IAccountType> GetRequiredAccountType(
		IAccount account,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		IAccountType? result = await _accountTypeService.GetAccountType(
			account.AccountTypeId,
			cancellationToken);

		if (result is null) throw new AccountTypeNotFoundException();

		return result;
	}

	private async Task<IUser> GetRequiredUser(
		int id,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		IUser? result = await _userService.GetSingleUser(
			id,
			cancellationToken);

		if (result is null) throw new UserNotFoundException();
		
		return result;
	}

	private async Task<Identity<int>> CreateRequiredTransaction(
		IUser? user,
		IAccount account,
		TransactionType transactionType,
		decimal amount,
		string? description,
		DateTime transactionDate,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		IAccountType accountType = await GetRequiredAccountType(
			account,
			cancellationToken);

		decimal amountReSigned = GetReSignedAmount(
				amount,
				transactionType);
		decimal transactionFee = GetTransactionFee(
			accountType,
			transactionType,
			amount);
		DateTime projectedRepaymentDate = GetProjectedRepaymentDate(
			accountType,
			transactionDate);

		Identity<int>? result = await _transactionService.CreateTransaction(
			user?.Id,
			account.Id,
			transactionType,
			amount,
			description,
			transactionDate,
			transactionFee,
			projectedRepaymentDate,
			cancellationToken);

		if (result is null) throw new TransactionCreationReturnedNullIdentityException();

		return result;
	}

	private static decimal GetReSignedAmount(
		decimal amount,
		TransactionType transactionType)
	{
		decimal amountMultiplier = transactionType
			switch
			{
				TransactionType.Debit => -1,
				_ => 1
			};
		decimal result = amount * amountMultiplier;
		
		return result;
	}

	private static decimal GetTransactionFee(
		IAccountType accountType,
		TransactionType transactionType,
		decimal amount)
	{
		if (transactionType is TransactionType.Debit) return 0;
		
		return amount * accountType.BaseFee;
	}

	private static DateTime GetProjectedRepaymentDate(
		IAccountType accountType,
		DateTime transactionDate)
	{
		return transactionDate.AddDays(accountType.PaymentDueDays);
	}
}
