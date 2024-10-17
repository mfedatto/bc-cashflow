using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.User;
using Bc.CashFlow.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

public class ServiceContextBuilder : IContextBuilderInstaller
{
	public void Install(WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<IUserService, UserService>();
		builder.Services.AddScoped<IAccountTypeService, AccountTypeService>();
	}
}
