using System.Data.Common;
using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.User;

namespace Bc.CashFlow.Domain.DbContext;

public interface IUnitOfWork : IDisposable
{
	DbConnection Connection { get; }
	DbTransaction? Transaction { get; }
	IUserRepository UserRepository { get; }
	IAccountTypeRepository AccountTypeRepository { get; }
	IAccountRepository AccountRepository { get; }

	Task BeginTransactionAsync();
	Task CommitAsync();
	Task RollbackAsync();
}
