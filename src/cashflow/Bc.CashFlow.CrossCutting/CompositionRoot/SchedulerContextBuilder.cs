using Bc.CashFlow.Domain.AppSettings;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

public class SchedulerContextBuilder : IContextBuilderInstaller, IContextBuilderAppConfigurator
{
	public WebApplication Configure(WebApplication app)
	{
		app.UseHangfireDashboard(
			"/hangfire",
			new()
			{
				Authorization = [new AllowAllConnectionsFilter()],
				IgnoreAntiforgeryToken = true
			});

		return app;
	}

	public void Install(
		WebApplicationBuilder builder,
		IConfiguration? configuration = null)
	{
		builder.Services.AddDataProtection()
			.SetApplicationName("cashflow-scheduler")
			.PersistKeysToFileSystem(new("/root/.aspnet/DataProtection-Keys"));
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

	private class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			return true;
		}
	}
}
