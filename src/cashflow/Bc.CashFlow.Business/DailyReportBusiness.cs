using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.DailyReport;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Business;

public class DailyReportBusiness : IDailyReportBusiness
{
	private readonly IAccountService _accountService;
	private readonly IDailyReportService _dailyReportService;

	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<DailyReportBusiness> _logger;
	private readonly ITransactionService _transactionService;

	public DailyReportBusiness(
		ILogger<DailyReportBusiness> logger,
		ITransactionService transactionService,
		IAccountService accountService,
		IDailyReportService dailyReportService)
	{
		_logger = logger;
		_transactionService = transactionService;
		_accountService = accountService;
		_dailyReportService = dailyReportService;
	}

	public async Task ConsolidateDailyReport(
		DateTime referenceDay,
		CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		IEnumerable<Identity<int>> accountIdsList =
			await _accountService.GetAccountsId(
				cancellationToken);

		Dictionary<int, decimal> accountsDebitBalance = new();
		Dictionary<int, decimal> accountsCreditBalance = new();
		Dictionary<int, decimal> accountsFeeBalance = new();
		decimal totalDebitBalance = 0;
		decimal totalCreditBalance = 0;
		decimal totalFeeBalance = 0;

		foreach (Identity<int> accountIdentityId in accountIdsList)
		{
			IEnumerable<ITransaction> transactionsList =
				await _transactionService.GetTransactionsOnProjectedRepaymentDate(
					accountIdentityId.Value,
					referenceDay,
					cancellationToken);

			accountsDebitBalance.Add(accountIdentityId.Value, 0);
			accountsCreditBalance.Add(accountIdentityId.Value, 0);
			accountsFeeBalance.Add(accountIdentityId.Value, 0);

			foreach (ITransaction transaction in transactionsList)
			{
				if (transaction.TransactionType is TransactionType.Debit)
				{
					accountsDebitBalance[accountIdentityId.Value] += transaction.Amount;
					totalDebitBalance += transaction.Amount;
				}
				else
				{
					accountsCreditBalance[accountIdentityId.Value] += transaction.Amount;
					totalCreditBalance += transaction.Amount;
				}

				accountsFeeBalance[accountIdentityId.Value] += transaction.TransactionFee ?? 0;
				totalFeeBalance += transaction.TransactionFee ?? 0;
			}

			decimal accountBalance = accountsCreditBalance[accountIdentityId.Value]
			                         - accountsDebitBalance[accountIdentityId.Value]
			                         - accountsFeeBalance[accountIdentityId.Value];

			Identity<int> accountDailyReport =
				await _dailyReportService.CreateDailyReport(
					accountIdentityId.Value,
					referenceDay,
					accountsDebitBalance[accountIdentityId.Value],
					accountsCreditBalance[accountIdentityId.Value],
					accountsFeeBalance[accountIdentityId.Value],
					accountBalance,
					cancellationToken);

			if (accountDailyReport is null)
			{
				throw new DailyReportCreationReturnedNullIdentityException();
			}
		}

		decimal balanceBeforeFee = totalCreditBalance - totalDebitBalance;
		decimal finalBalance = balanceBeforeFee - totalFeeBalance;

		Identity<int> totalDailyReport =
			await _dailyReportService.CreateDailyReport(
				null,
				referenceDay,
				totalDebitBalance,
				totalCreditBalance,
				balanceBeforeFee,
				finalBalance,
				cancellationToken);

		if (totalDailyReport is null)
		{
			throw new DailyReportCreationReturnedNullIdentityException();
		}
	}

	public async Task<IEnumerable<IDailyReport>> GetDailyReports(
		DateTime? referenceDateSince,
		DateTime? referenceDateUntil,
		CancellationToken cancellationToken)
	{
		IEnumerable<Identity<int>> dailyReportsIdsList =
			await GetDailyReportsId(
				referenceDateSince,
				referenceDateUntil,
				cancellationToken);
		IEnumerable<IDailyReport> dailyReportsList =
			await GetDailyReports(
				dailyReportsIdsList,
				cancellationToken);

		return dailyReportsList;
	}

	private async Task<IEnumerable<Identity<int>>> GetDailyReportsId(
		DateTime? referenceDateSince,
		DateTime? referenceDateUntil,
		CancellationToken cancellationToken)
	{
		return await _dailyReportService.GetDailyReportsId(
			referenceDateSince,
			referenceDateUntil,
			cancellationToken);
	}

	private async Task<IEnumerable<IDailyReport>> GetDailyReports(
		IEnumerable<Identity<int>> dailyReportsIdsList,
		CancellationToken cancellationToken)
	{
		List<IDailyReport> result = [];

		foreach (Identity<int> dailyReportIdentity in dailyReportsIdsList)
		{
			IDailyReport dailyReport = await _dailyReportService.GetDailyReport(
				dailyReportIdentity.Value,
				cancellationToken);

			result.Add(dailyReport);
		}

		return result;
	}
}