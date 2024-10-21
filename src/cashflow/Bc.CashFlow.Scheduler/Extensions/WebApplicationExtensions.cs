using System.Linq.Expressions;
using Bc.CashFlow.CrossCutting.CompositionRoot;
using Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;
using Bc.CashFlow.Scheduler.Jobs;
using Hangfire;

namespace Bc.CashFlow.Scheduler.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplicationBuilder Setup(
		this WebApplicationBuilder builder)
	{
		builder.AddCompositionRoot<SchedulerContextBuilder>()
			.Services.AddSingleton<DailyReportJob>();

		return builder;
	}

	public static WebApplication Configure(
		this WebApplication app)
	{
		return ((WebApplication)app
				.UseHangfireDashboard())
			.ConfigureApp<SchedulerContextBuilder>()
			.EnqueueJob<DailyReportJob>();
	}

	private static WebApplication EnqueueJob<TJob>(
		this WebApplication app)
		where TJob : IJob
	{
		return app.EnqueueJob(
			() =>
				app.Services.GetRequiredService<TJob>().Run());
	}

	private static WebApplication EnqueueJob(
		this WebApplication app,
		Expression<Action> methodCall)
	{
		IBackgroundJobClient backgroundJobs = app.Services
			.GetRequiredService<IBackgroundJobClient>();

		backgroundJobs.Enqueue(methodCall);

		return app;
	}
}
