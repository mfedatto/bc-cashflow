using Bc.CashFlow.Domain.Transaction;
using Bc.CashFlow.Web.Models.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bc.CashFlow.Web.Controllers;

public class TransactionsController : Controller
{
	private readonly ITransactionBusiness _business;

	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<TransactionsController> _logger;

	public TransactionsController(
		ILogger<TransactionsController> logger,
		ITransactionBusiness business)
	{
		_logger = logger;
		_business = business;
	}

	[HttpGet]
	public async Task<IActionResult> Index(
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
		IEnumerable<ITransaction> transactionsList = await _business.GetTransactions(
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
		TransactionIndexViewModel viewModel = new(transactionsList);

		return View(viewModel);
	}

	[HttpGet]
	public async Task<IActionResult> Create(
		CancellationToken cancellationToken)
	{
		TransactionCreateViewModel viewModel = new()
		{
			AccountsList = await GetAccountsSelectList(cancellationToken)
		};

		return View(viewModel);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(
		TransactionCreateViewModel viewModel,
		CancellationToken cancellationToken)
	{
		if (ModelState.IsValid)
		{
			_ = await _business.CreateTransaction(
				viewModel.UserId,
				viewModel.AccountId,
				viewModel.TransactionType,
				viewModel.Amount,
				viewModel.Description,
				DateTime.Now,
				cancellationToken);

			return RedirectToAction(nameof(Index));
		}

		viewModel.AccountsList = await GetAccountsSelectList(cancellationToken);

		return View(viewModel);
	}

	private async Task<IEnumerable<SelectListItem>> GetAccountsSelectList(
		CancellationToken cancellationToken)
	{
		return (await _business.GetAccounts(cancellationToken))
			.Select(
				account => new SelectListItem(
					account.Name,
					account.Id.ToString())
			);
	}
}