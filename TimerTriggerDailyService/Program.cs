using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimerNewsApp.Data;
using TimerNewsApp.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder => builder.AddJsonFile("local.Settings.json", optional: true,true))
    .ConfigureServices(s => {
        s.AddDbContext<FuncDbContext>();
        s.AddScoped<IDailyService,DailyService>();
        })
    .Build();

host.Run();