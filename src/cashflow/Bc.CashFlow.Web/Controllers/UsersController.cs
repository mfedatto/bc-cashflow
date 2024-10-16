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
		CancellationToken cancellationToken,
		[FromQuery(Name = "username")] string? username,
		[FromQuery(Name = "created-since")] DateTime? createdSince = null,
		[FromQuery(Name = "created-until")] DateTime? createdUntil = null)
	{
		UserIndexModel model = new(
			await _service.GetUsers(
				cancellationToken,
				username,
				createdSince,
				createdUntil));

		return View(model);
	}
}
