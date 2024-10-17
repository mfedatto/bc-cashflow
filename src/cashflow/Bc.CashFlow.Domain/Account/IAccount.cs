namespace Bc.CashFlow.Domain.Account;

public interface IAccount
{
	int Id { get; }
	int UserId { get; }
	int AccountTypeId { get; }
	string Name { get; }
	decimal InitialBalance { get; }
	decimal CurrentBalance { get; }
	DateTime BalanceUpdatedAt { get; }
	DateTime CreatedAt { get; }
}
