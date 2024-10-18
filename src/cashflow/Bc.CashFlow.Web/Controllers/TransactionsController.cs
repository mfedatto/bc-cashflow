using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Bc.CashFlow.Web.Models.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bc.CashFlow.Web.Controllers;

public class TransactionsController : Controller
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<TransactionsController> _logger;
	private readonly ITransactionService _service;
	private readonly IAccountService _accountService;

	public TransactionsController(
		ILogger<TransactionsController> logger,
		ITransactionService service,
		IAccountService accountService)
	{
		_logger = logger;
		_service = service;
		_accountService = accountService;
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
		IEnumerable<ITransaction> transactionsList = await _service.GetTransactions(
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
			Identity<int>? transactionId = await _service.CreateTransaction(
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
		return (await _accountService.GetAccounts(cancellationToken))
			.Select(
				account => new SelectListItem(
					account.Name,
					account.Id.ToString())
			);
	}
}
