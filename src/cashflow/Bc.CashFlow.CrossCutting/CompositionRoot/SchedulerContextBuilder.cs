using Bc.CashFlow.Domain.AppSettings;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

public class SchedulerContextBuilder : IContextBuilderInstaller, IContextBuilderAppConfigurator
{
	public void Install(
		WebApplicationBuilder builder,
		IConfiguration? configuration = null)
	{
		builder.Services.AddHangfire(
			sc =>
			{
				SchedulerConfig? config = builder.Services.BuildServiceProvider()
					.GetService<SchedulerConfig>();

				sc.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
					.UseSimpleAssemblyNameTypeSerializer()
					.UseRecommendedSerializerSettings()
					.UseSqlServerStorage(config!.Hangfire!.ConnectionString);
			});
		builder.Services.AddHangfireServer();
		builder.Services.AddMvc();
	}

	public WebApplication Configure(WebApplication app)
	{
		app.UseHangfireDashboard();

		return app;
	}
}
