﻿using Bc.CashFlow.Domain.QueueContext;
using Bc.CashFlow.IO.CacheContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

// ReSharper disable once InconsistentNaming
public class IOQueueContextBuilder : IContextBuilderInstaller
{
	public void Install(
		WebApplicationBuilder builder,
		IConfiguration? configuration = null)
	{
		builder.Services.AddScoped<IQueueConnectionFactory, QueueConnectionFactory>();
		builder.Services.AddScoped<IConnection>(
			sp =>
				sp.GetRequiredService<IQueueConnectionFactory>().CreateConnection());
	}
}