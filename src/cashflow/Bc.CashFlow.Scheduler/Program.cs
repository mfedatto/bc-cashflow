using Bc.CashFlow.CrossCutting.CompositionRoot;
using Bc.CashFlow.CrossCutting.CompositionRoot.Extensions;
using Bc.CashFlow.Scheduler.Extensions;

WebApplication.CreateBuilder(args)
	.Setup()
	.Build()
	.Configure()
	.Run();
