﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

public class WebApiContextBuilder : IContextBuilderInstaller, IContextBuilderAppConfigurator
{
	public void Install(
		WebApplicationBuilder builder,
		IConfiguration configuration = null)
	{
		builder.Services.AddControllersWithViews();
	}

	public WebApplication Configure(WebApplication app)
	{
		if (!app.Environment.IsDevelopment())
		{
		}

		app.UseStaticFiles();
		app.UseRouting();
		app.UseAuthorization();
		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");

		return app;
	}
}
