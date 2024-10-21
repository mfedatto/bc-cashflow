using System.Diagnostics.CodeAnalysis;

namespace Bc.CashFlow.Domain.AppSettings;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class CacheConfig : IConfig
{
	public string Section => "Cache";
	
	public string? Configuration { get; set; }
	public bool IgnoreUnknown { get; set; }
}
