using Bc.CashFlow.Domain.DailyReport;

namespace Bc.CashFlow.Scheduler.Jobs;

public class DailyReportJob : IJob
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<DailyReportJob> _logger;
	private readonly IDailyReportBusiness _business;

	public DailyReportJob(
		ILogger<DailyReportJob> logger,
		IDailyReportBusiness business)
	{
		_logger = logger;
		_business = business;
	}
	
	public void Run()
	{
		_business.ConsolidateDailyReport(new DateTime(2024, 10, 18), CancellationToken.None);
	}
}
