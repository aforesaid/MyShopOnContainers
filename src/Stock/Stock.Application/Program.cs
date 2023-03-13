using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Core;
using Stock.Infrastructure;
using Stock.Infrastructure.Database;

const string settings = "appsettings.json";

var host = new WebHostBuilder()
    .UseKestrel()
    .ConfigureAppConfiguration(x =>
        x.AddJsonFile(settings, optional: true)
        .AddEnvironmentVariables())
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