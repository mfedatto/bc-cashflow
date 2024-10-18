using Bc.CashFlow.Domain.Account;
using StackExchange.Redis;

namespace Bc.CashFlow.IO.CacheContext;

public class AccountCacheCollection : BaseCacheCollection<IAccount>
{
	public AccountCacheCollection(
		IDatabase db)
		: base("account", db)
	{
	}
}
