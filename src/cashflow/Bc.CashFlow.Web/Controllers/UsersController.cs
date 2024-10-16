using Bc.CashFlow.Domain.User;
using Bc.CashFlow.Web.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Bc.CashFlow.Web.Controllers;

public class UsersController : Controller
{
	private readonly ILogger<UsersController> _logger;
	private readonly IUserService _service;

	public UsersController(
		ILogger<UsersController> logger,
		IUserService service)
	{
		_logger = logger;
		_service = service;
	}

	[HttpGet]
	public async Task<IActionResult> Index(
			[FromQuery(Name = "username")] string? username,
		[FromQuery(Name = "created-since")] DateTime? createdSince,
		[FromQuery(Name = "created-until")] DateTime? createdUntil,
		CancellationToken cancellationToken)
	{
		UserIndexViewModel viewModel = new(
			await _service.GetUsers(
				username,
				createdSince,
				createdUntil,
				cancellationToken));

		return View(viewModel);
	}
	
	[HttpGet]
	public async Task<IActionResult> Details(
		[FromRoute(Name = "id")] int userId,
		CancellationToken cancellationToken)
	{
		UserDetailsViewModel viewModel = new(
			await _service.GetSingleUser(
				userId,
				cancellationToken));

		return View(viewModel);
	}
}
