using Bc.CashFlow.Domain.AccountType;
using StackExchange.Redis;

namespace Bc.CashFlow.IO.CacheContext;

public class AccountTypeCacheCollection : BaseCacheCollection<IAccountType>
{
	public AccountTypeCacheCollection(
		IDatabase db)
		: base("user", db)
	{
	}
}
