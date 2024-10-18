using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;

public static class ContextBuilderInstallerExtensions
{
    public static WebApplicationBuilder AddCompositionRoot(
        this WebApplicationBuilder builder)
    {
        IConfiguration configuration = builder.BuildConfiguration();

        builder
            .BuildContext<DomainContextBuilder>(configuration)
            .BuildContext<IOContextBuilder>(configuration)
            .BuildContext<ServiceContextBuilder>(configuration)
            .BuildContext<BusinessContextBuilder>(configuration)
            .BuildContext<WebApiContextBuilder>(configuration);

        return builder;
    }

    private static WebApplicationBuilder BuildContext<T>(
        this WebApplicationBuilder builder,
        IConfiguration configuration)
        where T : IContextBuilderInstaller, new()
    {
        T installer = new();

        if (installer is IContextBuilderConfigBinder configurator)
        {
            configurator.BindConfig(builder, configuration);
        }

        installer.Install(builder);

        return builder;
    }

    private static IConfiguration BuildConfiguration(
        this WebApplicationBuilder builder)
    {
        return builder.Configuration
            .AddJsonFile(
                "appsettings.json",
                false,
                true)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                true,
                true)
            .AddEnvironmentVariables()
            .Build();
    }
}
