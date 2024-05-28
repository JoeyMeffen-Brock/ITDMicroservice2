using System.Data.SqlClient;
using System.Reflection;
using BrockSolutions.Configuration;
using BrockSolutions.Configuration.AzureAppConfig;
using BrockSolutions.ITDService;
using BrockSolutions.ITDService.Options;
using BrockSolutions.Service.NLog.Logging;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;

try
{
    // This ConfigurationBuilder is used to bootstrap the configuration process and allow early access to the configuration.
    // A separate ConfigurationBuilder is created below for use by the application once it is running.
    // This duplication is necessary because the DeafultBuilder does not expose the ConfigurationBuilder like the WebApplicationBuilder does.
    var configBuilder = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddBrockConfiguration(ITDServiceParameters.CONFIGURATION_KEY);

    var configuration = configBuilder.Build();

    var hostBuilder = Host.CreateDefaultBuilder(args);    
    hostBuilder.ConfigureNLog(configuration.GetSection($"{ITDServiceParameters.CONFIGURATION_KEY}:Logging").Get<NLogConfiguration>());

    if (configuration.GetValue($"{ITDServiceParameters.CONFIGURATION_KEY}:IsWindowsService", false))
    {
        hostBuilder.UseWindowsService(options =>
        {
            options.ServiceName = "ITDService";
        });
    }

    var host = hostBuilder
        .ConfigureAppConfiguration((context, builder) =>
        {
            // The configuration builder must be re-created here to ensure it is registered properly with the host.
            builder.SetBasePath(AppContext.BaseDirectory)
                .AddBrockConfiguration(ITDServiceParameters.CONFIGURATION_KEY);
        })
        .ConfigureServices((context, services) =>
        {
            ServiceConfiguration.ConfigureServices(services, context.Configuration);
            if (configBuilder.IsAzureAppConfigEnabled())
            {
                services.AddAzureAppConfiguration();
            }
            else
            {
                services.AddSingleton<IConfigurationRefresherProvider, EmptyConfigurationRefresherProvider>();
            }
        })
        .Build();

    LogAndValidateCriticalConfigurationValues(host.Services);

    var logger = LogManager.GetCurrentClassLogger();
    var assembly = Assembly.GetExecutingAssembly().GetName();
    logger.Info("Starting Service Host for {ApplicationName} version {ApplicationVersion}", assembly.Name, assembly.Version);

    await host.RunAsync();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    var logger = NLog.LogManager.GetCurrentClassLogger();
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

void LogAndValidateCriticalConfigurationValues(IServiceProvider services)
{
    var logger = NLog.LogManager.GetCurrentClassLogger();

    // Trigger validation of critical bound values on startup by getting them from the service provider. Then log them.
    var workerServiceConfigParams = services.GetRequiredService<IOptionsMonitor<ITDServiceParameters>>().CurrentValue;
    logger.Debug("Validating required parameter ExampleConfigurationValue: {ExampleConfigurationValue}", workerServiceConfigParams.ExampleConfigurationValue);
    var connectionStrings = workerServiceConfigParams.ConnectionString;
    logger.Debug("Validating required parameter ConnectionStrings: {ConnectionStringNames}", string.Join(',', connectionStrings.Select(x => x.Key)));

    if (connectionStrings != null)
    {
        foreach (var connectionString in connectionStrings)
        {
            logger.Info("ConnectionString {ConnectionString_Key} = {ConnectionString_Value}",
                         connectionString.Key, CleanupDBConnectionLogs(connectionString.Value));
        }
    }
    else
    {
        logger.Warn("No ConnectionStrings found in configuration.");
    }
}

string CleanupDBConnectionLogs(string connectionString)
{
    var logger = NLog.LogManager.GetCurrentClassLogger();
    SqlConnectionStringBuilder builder = null;
    try
    {
        builder = new SqlConnectionStringBuilder(connectionString);
    }
    catch (Exception e)
    {
        logger.Error(e);
        return null;
    }
    builder.UserID = "*****";
    builder.Password = "*****";
    return builder.ConnectionString;
}
