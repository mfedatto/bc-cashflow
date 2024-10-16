using System.Data.Common;
using Bc.CashFlow.Domain.User;

namespace Bc.CashFlow.Domain.MainDbContext;

public interface IUnitOfWork : IDisposable
{
	DbConnection Connection { get; }
	DbTransaction? Transaction { get; }
	IUserRepository UserRepository { get; }

	Task BeginTransactionAsync();
	Task CommitAsync();
	Task RollbackAsync();
}
