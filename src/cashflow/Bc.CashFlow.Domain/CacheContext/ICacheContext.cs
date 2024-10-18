using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.User;

namespace Bc.CashFlow.Domain.CacheContext;

public interface ICacheContext
{
	ICacheCollection<IUser> User { get; }
	ICacheCollection<IAccountType> AccountType { get; }
	ICacheCollection<IAccount> Account { get; }
}
