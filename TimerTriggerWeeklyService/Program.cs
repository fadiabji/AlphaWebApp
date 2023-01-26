using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimerTriggerWeeklyService.Data;
using TimerTriggerWeeklyService.Services;


//config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
//= Newtonsoft.Json.ReferenceLoopHandling.Ignore;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder => builder.AddJsonFile("local.Settings.json", optional: true, true))
    .ConfigureServices(s => {
        s.AddDbContext<FuncDbContext>();
        s.AddScoped<IWeeklyService, WeeklyService>();
    })
    .Build();


host.Run();


