using Server.MiddleWare;
using Server.RegisterServices;
using Microsoft.Extensions.FileProviders;
using UnitSystem;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.AddServerServices();
UnitManager.RegisterByAssembly(typeof(SIUnitTypes).Assembly);

var app = builder.Build();

app.UseExceptionHandling(app.Environment);

app.UseForwarding(builder.Configuration);

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
//app.MapStaticAssets();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Initialize(builder.Configuration);


app.MapEndPoint();
app.UseEndpoints();

app.Run();
