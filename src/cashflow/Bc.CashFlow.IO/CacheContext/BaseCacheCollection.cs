using Bc.CashFlow.Domain.CacheContext;
using Bc.CashFlow.Domain.User;
using StackExchange.Redis;

namespace Bc.CashFlow.IO.CacheContext;

public abstract class BaseCacheCollection<TValue> : ICacheCollection<TValue>
	where TValue : class
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
		TValue value,
		CancellationToken cancellationToken)
	{
		await _db.SetValue(
			KeyPrefix,
			key,
			value,
			cancellationToken);
	}

	public async Task<TValue?> GetVale(
		string key,
		CancellationToken cancellationToken)
	{
		return await _db.GetValue<TValue>(
			KeyPrefix,
			key,
			cancellationToken);
	}
}
