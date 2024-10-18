using System.Text;
using Bc.CashFlow.Domain.AppSettings;
using Bc.CashFlow.Domain.QueueContext;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Bc.CashFlow.IO.QueueContext;

public class QueuePublisher : IQueuePublisher
{
	// ReSharper disable once NotAccessedField.Local
	private readonly ILogger<QueuePublisher> _logger;
	private readonly IConnection _connection;
	private readonly QueueConfig _config;

	public QueuePublisher(
		ILogger<QueuePublisher> logger,
		IConnection connection,
		QueueConfig config)
	{
		_logger = logger;
		_connection = connection;
		_config = config;
	}
	
	public async Task PublishMessage(
		string message,
		string queue)
	{
		await Task.Run(() =>
		{
			// ReSharper disable once ConvertToUsingDeclaration
			using (IModel? channel = _connection.CreateModel())
			{
				channel.QueueDeclare(
					queue,
					false,
					false,
					false,
					null);

				byte[] body = Encoding.UTF8.GetBytes(message);
				IBasicProperties basicProperties = channel.CreateBasicProperties();
				
				basicProperties.Persistent = true;

				channel.BasicPublish(
					_config.Exchange,
					queue,
					basicProperties,
					body);
			}
		});
	}
}
