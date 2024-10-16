using Microsoft.AspNetCore.Mvc;

namespace Bc.CashFlow.Web.Controllers;

public class AccountTypeController : Controller
{
	[HttpGet]
	public IActionResult Index()
	{
		return View();
	}
}
