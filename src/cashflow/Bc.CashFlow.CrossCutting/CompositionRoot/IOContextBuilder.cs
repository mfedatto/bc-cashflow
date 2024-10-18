using System.Data.Common;
using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.CacheContext;
using Bc.CashFlow.Domain.DbContext;
using Bc.CashFlow.Domain.Transaction;
using Bc.CashFlow.Domain.User;
using Bc.CashFlow.IO.CacheContext;
using Bc.CashFlow.IO.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

// ReSharper disable once InconsistentNaming
public class IOContextBuilder : IContextBuilderInstaller
{
	public void Install(WebApplicationBuilder builder)
	{
		InstallDbContext(builder);
		InstallCacheContext(builder);
	}

	private static void InstallDbContext(WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		builder.Services.AddScoped<DbConnection>(
			sp =>
				sp.GetRequiredService<IUnitOfWork>().Connection);
		builder.Services.AddScoped<DbTransaction>(
			sp =>
				sp.GetRequiredService<IUnitOfWork>().Transaction!);
		builder.Services.AddScoped<IUserRepository, UserRepository>();
		builder.Services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
		builder.Services.AddScoped<IAccountRepository, AccountRepository>();
		builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
	}

	private static void InstallCacheContext(WebApplicationBuilder builder)
	{
		builder.Services.AddSingleton<ICacheConnection, CacheConnection>();
		builder.Services.AddSingleton<IConnectionMultiplexer>(
			sp =>
				sp.GetRequiredService<ICacheConnection>().Connect());
		builder.Services.AddScoped<IDatabase>(
			sp =>
				sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
		builder.Services.AddScoped<ICacheContext, CacheContext>();
		builder.Services.AddScoped<ICacheCollection<IUser>, UserCacheCollection>();
		builder.Services.AddScoped<ICacheCollection<IAccountType>, AccountTypeCacheCollection>();
		builder.Services.AddScoped<ICacheCollection<IAccount>, AccountCacheCollection>();
	}
}
