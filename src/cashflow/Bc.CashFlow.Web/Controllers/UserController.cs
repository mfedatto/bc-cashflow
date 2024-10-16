using Bc.CashFlow.Domain.User;
using Bc.CashFlow.Web.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Bc.CashFlow.Web.Controllers;

public class UserController : Controller
{
	private readonly ILogger<UserController> _logger;
	private readonly IUserService _service;

	public UserController(
		ILogger<UserController> logger,
		IUserService service)
	{
		_logger = logger;
		_service = service;
	}

	[HttpGet]
	public async Task<IActionResult> Index()
	{
		UserIndexModel model = new(
			await _service.GetUsers());

		return View(model);
	}
}
