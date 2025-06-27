using Serilog;
using Yungching.WebAPI;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting up");


    var builder = WebApplication.CreateBuilder(args);

    if (!builder.Environment.IsDevelopment())
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
        builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
    }

    builder.Host.UseSerilog((context, services, config) =>
    {
        config
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console();
    });


    var startup = new Startup(builder.Configuration);
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();

    startup.Configure(app, app.Environment);

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
