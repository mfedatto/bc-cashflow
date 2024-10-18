namespace Bc.CashFlow.Domain.AppSettings;

public class CacheConfig : IConfig
{
	public string Section => "Cache";
	
	public string? Configuration { get; set; }
	public bool IgnoreUnknown { get; set; }
}
