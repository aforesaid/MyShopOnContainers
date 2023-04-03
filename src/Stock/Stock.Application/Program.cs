using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Serilog;
using Stock.Core;
using Stock.Infrastructure;
using Stock.Infrastructure.Database;

const string settings = "appsettings.json";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var host = new WebHostBuilder()
    .UseKestrel()
    .ConfigureAppConfiguration(x =>
        x.AddJsonFile(settings, optional: true)
        .AddEnvironmentVariables())
    .ConfigureLogging((host, loggingBuilder) =>
    {  
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(host.Configuration)
            .Destructure.AsScalar<JObject>()
            .Destructure.AsScalar<JArray>()
            .Enrich.FromLogContext()
            .CreateLogger();
        
        loggingBuilder.AddSerilog(logger, dispose: true);
    })
    .ConfigureServices((host, serviceCollection) =>
    {
        serviceCollection.AddCoreServices(host.Configuration);
        serviceCollection.AddInfrastructure(host.Configuration);
    })
    .Configure(configuration =>
    {
        MigrateDbContext<StockDbContext>(configuration.ApplicationServices);
    })
    .Build();

await host.RunAsync();

static void MigrateDbContext<T>(IServiceProvider serviceProvider)
    where T: DbContext
{
    var dbContent = serviceProvider.CreateScope()
        .ServiceProvider
        .GetRequiredService<T>();
    dbContent.Database.Migrate();
}