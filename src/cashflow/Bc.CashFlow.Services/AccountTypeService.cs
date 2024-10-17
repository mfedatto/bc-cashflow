using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.DbContext;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.Services;

public class AccountTypeService : IAccountTypeService
{
	private readonly ILogger<AccountTypeService> _logger;
	private readonly IUnitOfWork _uow;

	public AccountTypeService(
		ILogger<AccountTypeService> logger,
		IUnitOfWork uow)
	{
		_logger = logger;
		_uow = uow;
	}

	public async Task<IEnumerable<IAccountType>> GetAccountTypes(
		string? accountTypeName,
		decimal? baseFeeFrom,
		decimal? baseFeeTo,
		int? paymentDueDaysFrom,
		int? paymentDueDaysTo,
		CancellationToken cancellationToken)
	{
		return await _uow.AccountTypeRepository.GetAccountTypes(
			accountTypeName,
			baseFeeFrom,
			baseFeeTo,
			paymentDueDaysFrom,
			paymentDueDaysTo,
			cancellationToken);
	}
}
