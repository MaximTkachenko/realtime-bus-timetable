using BusTimetable.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using Serilog;
using Serilog.Events;


// todo read from config
// todo use build.props
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
    .MinimumLevel.Override("Orleans", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );
builder.Host.UseSerilog();

builder.Services.AddCors();

builder.Services.AddSingleton<IMetadataService, MetadataService>();
builder.Services.AddSingleton<MetadataV2>();
builder.Services.AddSingleton<ITimetableCache, TimetableCache>();

builder.Services.AddControllers();
builder.Host.UseOrleans(siloBuilder =>
{
    siloBuilder
        .UseDashboard(options => {
            options.Host = "*";
            options.Port = 5088;
            options.HostSelf = true;
            options.CounterUpdateIntervalMs = 2000;
        })
        .UseLocalhostClustering();
});

WebApplication app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(_ => true));

app.UseRouting();
app.MapControllers();
app.UseWebSockets();

await app.RunAsync();