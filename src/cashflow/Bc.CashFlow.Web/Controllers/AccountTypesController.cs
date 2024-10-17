using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Web.Models.AccountType;
using Microsoft.AspNetCore.Mvc;

namespace Bc.CashFlow.Web.Controllers;

public class AccountTypesController : Controller
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<UsersController> _logger;
	private readonly IAccountTypeService _service;

	public AccountTypesController(
		ILogger<UsersController> logger,
		IAccountTypeService service)
	{
		_logger = logger;
		_service = service;
	}

	[HttpGet]
	public async Task<IActionResult> Index(
		string? name,
		decimal? baseFeeFrom,
		decimal? baseFeeTo,
		int? paymentDueDaysFrom,
		int? paymentDueDaysTo,
		CancellationToken cancellationToken)
	{
		AccountTypeIndexViewModel viewModel = new(
			await _service.GetAccountTypes(
				name,
				baseFeeFrom,
				baseFeeTo,
				paymentDueDaysFrom,
				paymentDueDaysTo,
				cancellationToken));

		return View(viewModel);
	}
}
