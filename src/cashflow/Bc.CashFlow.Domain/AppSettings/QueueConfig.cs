namespace Bc.CashFlow.Domain.AppSettings;

public class QueueConfig : IConfig
{
	public string Section => "Queue";
	
	public string? HostName { get; set; }
}
