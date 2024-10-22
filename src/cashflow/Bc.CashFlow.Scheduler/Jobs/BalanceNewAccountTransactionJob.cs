using Bc.CashFlow.Domain.Transaction;

namespace Bc.CashFlow.Scheduler.Jobs;

public class BalanceNewAccountTransactionJob : IJob
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<BalanceNewAccountTransactionJob> _logger;
	private readonly IServiceProvider _serviceProvider;

	public BalanceNewAccountTransactionJob(
		ILogger<BalanceNewAccountTransactionJob> logger,
		IServiceProvider serviceProvider)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	public void Run()
	{
		// ReSharper disable once ConvertToUsingDeclaration
		using (IServiceScope scope = _serviceProvider.CreateScope())
		{
			ITransactionBusiness transactionBusiness = scope.ServiceProvider.GetRequiredService<ITransactionBusiness>();

			transactionBusiness.UpdateAccountBalance(
					-1,
					CancellationToken.None)
				.GetAwaiter()
				.GetResult();
		}
	}
}
