namespace Bc.CashFlow.Domain.AccountType;

public class AccountTypeFactory
{
	// ReSharper disable once MemberCanBeMadeStatic.Global
	public IAccountType Create(
		int id,
		string name,
		decimal baseFee,
		int paymentDueDays)
	{
		return new AccountTypeVo(
			Id: id,
			Name: name,
			BaseFee: baseFee,
			PaymentDueDays: paymentDueDays
		);
	}
}

file record AccountTypeVo(
	int Id,
	string Name,
	decimal BaseFee,
	int PaymentDueDays
) : IAccountType;
