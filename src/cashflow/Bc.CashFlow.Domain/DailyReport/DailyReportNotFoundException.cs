namespace Bc.CashFlow.Domain.DailyReport;

public class DailyReportNotFoundException : Exception
{
	public DailyReportNotFoundException() : base("The requested daily report was not found.")
	{
	}
}
