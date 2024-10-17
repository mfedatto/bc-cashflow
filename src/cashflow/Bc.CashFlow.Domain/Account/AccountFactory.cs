namespace Bc.CashFlow.Domain.Account;

public class AccountFactory
{
	public IAccount Create(
		int id,
		int userId,
		int accountTypeId,
		string name,
		decimal initialBalance,
		decimal currentBalance,
		DateTime balanceUpdatedAt,
		DateTime createdAt)
	{
		return new AccountVo(
			Id: id,
			UserId: userId,
			AccountTypeId: accountTypeId,
			Name: name,
			InitialBalance: initialBalance,
			CurrentBalance: currentBalance,
			BalanceUpdatedAt: balanceUpdatedAt,
			CreatedAt: createdAt);
	}
}

file record AccountVo(
	int Id,
	int UserId,
	int AccountTypeId,
	string Name,
	decimal InitialBalance,
	decimal CurrentBalance,
	DateTime BalanceUpdatedAt,
	DateTime CreatedAt
) : IAccount;
