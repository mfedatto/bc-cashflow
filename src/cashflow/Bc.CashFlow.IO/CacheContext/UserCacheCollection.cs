using Bc.CashFlow.Domain.User;
using StackExchange.Redis;

namespace Bc.CashFlow.IO.CacheContext;

public class UserCacheCollection : BaseCacheCollection<IUser>
{
	public UserCacheCollection(
		IDatabase db)
		: base("user", db)
	{
	}
}
