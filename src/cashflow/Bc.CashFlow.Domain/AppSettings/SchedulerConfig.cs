namespace Bc.CashFlow.Domain.AppSettings;

public class SchedulerConfig : IConfig
{
	public string Section => "Scheduler";

	public HangfireConfig? Hangfire { get; set; }

	public class HangfireConfig
	{
		public string? ConnectionString { get; set; }
	}
}
