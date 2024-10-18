using Bc.CashFlow.Domain.Transaction;

namespace Bc.CashFlow.Domain.QueueContext;

public interface IQueueContext
{
	Task PublishNewTransactionToBalance(
		int id,
		CancellationToken cancellationToken);
}
