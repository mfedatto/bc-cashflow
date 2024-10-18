using Bc.CashFlow.Domain.AccountType;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Business;

public class AccountTypeBusiness : IAccountTypeBusiness
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<AccountTypeBusiness> _logger;
	private readonly IAccountTypeService _accountTypeService;

	public AccountTypeBusiness(
		ILogger<AccountTypeBusiness> logger,
		IAccountTypeService accountTypeService)
	{
		_logger = logger;
		_accountTypeService = accountTypeService;
	}

	public async Task<IEnumerable<IAccountType>> GetAccountTypes(
		string? name,
		decimal? baseFeeFrom,
		decimal? baseFeeTo,
		int? paymentDueDaysFrom,
		int? paymentDueDaysTo,
		CancellationToken cancellationToken)
	{
		return await _accountTypeService.GetAccountTypes(
			name,
			baseFeeFrom,
			baseFeeTo,
			paymentDueDaysFrom,
			paymentDueDaysTo,
			cancellationToken);
	}

	public async Task<IAccountType?> GetAccountType(
		int id,
		CancellationToken cancellationToken)
	{
		return await _accountTypeService.GetAccountType(
			id,
			cancellationToken);
	}
}
