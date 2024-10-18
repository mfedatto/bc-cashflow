using Bc.CashFlow.CrossCutting.CompositionRoot;
using Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;
using Bc.CashFlow.Web.Extensions;

WebApplication.CreateBuilder(args)
    .AddCompositionRoot<WebApiContextBuilder>()
    .Build()
    .Configure()
    .Run();
