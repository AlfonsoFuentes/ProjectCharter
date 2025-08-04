using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazorWeb.Services;
using MudBlazorWeb.Services.Navigations;
using Shared.FinshingLines;
using Shared.FinshingLines.DPSimulation;
using UnitSystem;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder
.AddRootComponents()
.AddClientServices();
builder.Services.AddSingleton<NavigationBack>();
builder.Services.AddBlazorDownloadFile();
UnitManager.RegisterByAssembly(typeof(SIUnitTypes).Assembly);
// Registro de servicios
builder.Services.AddSingleton<NewSimulationEngine>();
//builder.Services.AddSingleton<ProductionScheduler>();
builder.Services.AddSingleton<ReadSimulationFromDatabase>();
builder.Services.AddSingleton<NewSimulationServices>();
var host = builder.Build();
host.Services.GetService<NavigationBack>();
await host.RunAsync();
