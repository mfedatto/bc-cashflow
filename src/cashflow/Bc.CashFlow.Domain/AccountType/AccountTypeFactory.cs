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
			AccountTypeId: accountTypeId,
			AccountTypeName: accountTypeName,
			BaseFee: baseFee,
			PaymentDueDays: paymentDueDays
		);
	}
}

file record AccountTypeVo(
	int AccountTypeId,
	string AccountTypeName,
	decimal BaseFee,
	int PaymentDueDays
) : IAccountType;
