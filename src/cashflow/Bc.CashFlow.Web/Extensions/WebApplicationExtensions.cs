using Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;

namespace Bc.CashFlow.Web.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplication Configure(this WebApplication app)
	{
		return ((WebApplication)app.UseExceptionHandler("/Home/Error"))
			.ConfigureApp();
	}
}
