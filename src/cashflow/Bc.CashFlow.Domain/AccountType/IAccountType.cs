namespace Bc.CashFlow.Domain.AccountType;

public interface IAccountType
{
	int AccountTypeId { get; }
	string AccountTypeName { get; }
	decimal BaseFee { get; }
	int PaymentDueDays { get; }
}
