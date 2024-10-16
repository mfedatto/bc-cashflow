using Microsoft.AspNetCore.Builder;

namespace Bc.CashFlow.CrossCutting.CompositionRoot;

public interface IContextBuilderInstaller
{
    void Install(
        WebApplicationBuilder builder);
}
