using Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;
using Bc.CashFlow.Web.Extensions;

WebApplication.CreateBuilder(args)
    .AddCompositionRoot()
    .Build()
    .Configure()
    .Run();
