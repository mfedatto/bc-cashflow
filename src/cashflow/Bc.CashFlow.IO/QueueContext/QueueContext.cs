using System.Text.Json;
using Bc.CashFlow.Domain.AppSettings;
using Bc.CashFlow.Domain.QueueContext;
using Microsoft.Extensions.Logging;

namespace Bc.CashFlow.IO.QueueContext;

public class QueueContext : IQueueContext
{
	private readonly QueueConfig _config;

	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<QueueContext> _logger;
	private readonly IQueuePublisher _q;

	public QueueContext(
		ILogger<QueueContext> logger,
		IQueuePublisher q,
		QueueConfig config)
	{
		_logger = logger;
		_q = q;
		_config = config;
	}

	public async Task PublishNewTransactionToBalance(
		int id,
		CancellationToken cancellationToken)
	{
		string jsonValue = JsonSerializer.Serialize(
			new
			{
				TransactionId = id
			});

		await _q.PublishMessage(
			jsonValue,
			_config.NewTransactionToBalanceQueue!,
			cancellationToken);
	}
}