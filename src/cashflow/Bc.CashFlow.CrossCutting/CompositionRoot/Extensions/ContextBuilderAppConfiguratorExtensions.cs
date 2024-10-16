using Microsoft.AspNetCore.Builder;

namespace Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;

public static class ContextBuilderAppConfiguratorExtensions
{
    public static WebApplication ConfigureApp(this WebApplication app)
    {
        return app.Configure<WebApiContextBuilder>()
            //// .Configure<TelemetryContextBuilder>()
            ;
    }

    private static WebApplication Configure<T>(this WebApplication app)
        where T : IContextBuilderAppConfigurator, new()
    {
        return new T().Configure(app);
    }
}
