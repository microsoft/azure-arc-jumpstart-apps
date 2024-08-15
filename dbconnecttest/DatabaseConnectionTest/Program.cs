using DatabaseConnectionTest;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.SuppressInsecureTLSWarning", true);
        AppContext.SetSwitch("Switch.Microsoft.Data.SqlClient.EnableRetryLogic", true);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();