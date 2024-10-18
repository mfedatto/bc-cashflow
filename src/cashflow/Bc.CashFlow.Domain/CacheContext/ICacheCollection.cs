namespace Bc.CashFlow.Domain.CacheContext;

public interface ICacheCollection<TContext>
	where TContext : class
{
	string KeyPrefix { get; }

	Task SetVale(
		string key,
		TContext value,
		CancellationToken cancellationToken);

	Task<TContext?> GetValue(
		string key,
		CancellationToken cancellationToken);
}
