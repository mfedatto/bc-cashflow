﻿using System.Data.Common;
using Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;
using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.CacheContext;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Bc.CashFlow.Domain.User;
using Bc.CashFlow.IO.CacheContext;
using Bc.CashFlow.IO.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

// ReSharper disable once InconsistentNaming
public class IOContextBuilder : IContextBuilderInstaller
{
	public void Install(
		WebApplicationBuilder builder,
		IConfiguration configuration = null)
	{
		builder
			.BuildContext<DbContextBuilder>(configuration)
			.BuildContext<CacheContextBuilder>(configuration);
	}
}
