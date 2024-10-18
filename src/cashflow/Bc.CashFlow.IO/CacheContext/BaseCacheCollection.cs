using Bc.CashFlow.Domain.CacheContext;
using Bc.CashFlow.Domain.User;
using StackExchange.Redis;

namespace Bc.CashFlow.IO.CacheContext;

public abstract class BaseCacheCollection<TInterface, TConcrete> : ICacheCollection<TInterface>
	where TInterface : class
	where TConcrete : class, TInterface
{
	public string KeyPrefix { get; init; }

	private readonly IDatabase _db;

	protected BaseCacheCollection(
		string keyPrefix,
		IDatabase db)
	{
		KeyPrefix = keyPrefix;
		_db = db;
	}

	public async Task SetVale(
		string key,
		TInterface value,
		CancellationToken cancellationToken)
	{
		await _db.SetValue(
			KeyPrefix,
			key,
			value,
			cancellationToken);
	}

	public async Task<TInterface?> GetValue(
		string key,
		CancellationToken cancellationToken)
	{
		return await _db.GetValue<TConcrete>(
			KeyPrefix,
			key,
			cancellationToken);
	}
}
