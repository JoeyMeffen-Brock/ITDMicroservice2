using BrockSolutions.ITDService.Options;
using BrockSolutions.ITDService.Options.Validation;
using BrockSolutions.ITDService.Providers;
using IdentityModel.Client;

namespace BrockSolutions.ITDService
{
    public class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            BindConfigurationParameterClasses(services, configuration);

            services.AddSingleton<JokeService>();
            services.AddHostedService<ITDServiceBackgroundService>();

            ConfigureHttpClient(services, configuration);

            services.AddTransient<IWebApiClient, WebApiClient>();
        }

        private static void ConfigureHttpClient(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();

            services.AddAccessTokenManagement(options =>
            {
                var authority = configuration.GetSection("ITDService:AuthenticationAuthority").Get<string>().Trim('/');
                options.Client.Clients.Add("webapiclient", new ClientCredentialsTokenRequest
                {
                    // TODO: bonus points to anyone that can get this working by accessing the discovery cache instead of hard coding the token endpoint
                    // making the request to the discovery endpoint breaks the AddClientAccessTokenHttpClient below
                    Address = $"{authority}/connect/token",
                    ClientId = "itdservice",
                    ClientSecret = "SmartSuite",
                    Scope = "webapi",
                });
            });

            services.AddClientAccessTokenHttpClient(clientName: "webapiclient", tokenClientName: "webapiclient", configureClient: client =>
            {
                client.BaseAddress = new Uri($"{configuration.GetSection("ITDService:WebApiAddress").Get<string>().TrimEnd('/')}/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }

        private static void BindConfigurationParameterClasses(IServiceCollection services, IConfiguration configuration)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            services
                .AddOptions<ITDServiceParameters>()
                .Bind(configuration.GetSection(ITDServiceParameters.CONFIGURATION_KEY))
                .Validate(x =>
                    OptionsValidator<ITDServiceParameters>.Validate(logger, x),
                    "Invalid ITDServiceParameters configuration parameters"
                );
        }
    }
}
