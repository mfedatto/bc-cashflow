using Bc.CashFlow.Domain.AppSettings;
using Bc.CashFlow.Domain.QueueContext;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Bc.CashFlow.IO.CacheContext;

public class QueueConnectionFactory : IQueueConnectionFactory
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<CacheConnection> _logger;
	private readonly QueueConfig _config;

	public QueueConnectionFactory(
		ILogger<CacheConnection> logger,
		QueueConfig config)
	{
		_logger = logger;
		_config = config;
	}

	public IConnection CreateConnection()
	{
		if (_config.HostName is null) throw new NullQueueHostNameException();
		
		ConnectionFactory connectionFactory = new()
		{
			HostName = _config.HostName
		};

		return connectionFactory.CreateConnection();
	}
}
