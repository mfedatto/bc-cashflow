namespace Bc.CashFlow.Domain.AccountType;

public class AccountTypeFactory
{
	public IAccountType Create(
		int accountTypeId,
		string accountTypeName,
		decimal baseFee,
		int paymentDueDays)
	{
		return new AccountTypeVo(
			Id: accountTypeId,
			Name: accountTypeName,
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
