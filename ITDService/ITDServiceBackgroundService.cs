using BrockSolutions.ITDService.Options;
using BrockSolutions.ITDService.Providers;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;


namespace BrockSolutions.ITDService
{
    public sealed class ITDServiceBackgroundService : BackgroundService
    {
        private readonly JokeService _jokeService;
        private readonly ILogger<ITDServiceBackgroundService> _logger;
        private readonly IWebApiClient _webApiClient;
        private readonly IOptionsMonitor<ITDServiceParameters> _parameters;
        private readonly IEnumerable<IConfigurationRefresher> _refreshers;

        public ITDServiceBackgroundService
        (
            JokeService jokeService,
            ILogger<ITDServiceBackgroundService> logger,
            IWebApiClient webApiClient,
            IConfigurationRefresherProvider refreshProvider,
            IOptionsMonitor<ITDServiceParameters> parameters
        )
        {
            _jokeService = jokeService;
            _logger = logger;
            _webApiClient = webApiClient;
            _parameters = parameters;
            _refreshers = refreshProvider.Refreshers;
        }

        public async Task RefreshConfiguration()
        {
            foreach (var refresher in _refreshers)
            {
                _ = await refresher.TryRefreshAsync();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delayTime = TimeSpan.FromMinutes(0.25);

            // example showing how to call an api using configured httpclient
            // uncomment, run identity server and webapi to see in action
            //var response = await _webApiClient.ExampleCallApiGetRequest();
            //_logger.LogTrace("HTTP request response: {response}", response);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Reload the configuration from YAML & Azure App Configuration if it has changed. 
                await RefreshConfiguration();

                var exampleConfigValue = _parameters.CurrentValue.ExampleConfigurationValue;
                _logger.LogError("Current Config Value: {ExampleConfigValue}", exampleConfigValue);
                _logger.LogTrace("Retrieving a random joke...");
                Joke joke = _jokeService.GetJoke();

                _logger.LogWarning("Telling Joke #{Joke_Id}: {Setup} {Punchline}", joke.Id, joke.Setup, joke.Punchline);

                _logger.LogDebug("Waiting {Delay} seconds for next joke.", delayTime.TotalSeconds);
                await Task.Delay(TimeSpan.FromMinutes(0.25), stoppingToken);
            }
        }
    }

    public class JokeService
    {
        public Joke GetJoke()
        {
            Joke joke = _jokes.ElementAt(
                Random.Shared.Next(_jokes.Count));

            return joke;
        }

        // Programming jokes borrowed from:
        // https://github.com/eklavyadev/karljoke/blob/main/source/jokes.json
        readonly HashSet<Joke> _jokes = new()
        {
            new Joke(1, "What's the best thing about a Boolean?", "Even if you're wrong, you're only off by a bit."),
            new Joke(2, "What's the object-oriented way to become wealthy?", "Inheritance"),
            new Joke(3, "Why did the programmer quit their job?", "Because they didn't get arrays."),
            new Joke(4, "Why do programmers always mix up Halloween and Christmas?", "Because Oct 31 == Dec 25"),
            new Joke(5, "How many programmers does it take to change a lightbulb?", "None that's a hardware problem"),
            new Joke(6, "If you put a million monkeys at a million keyboards, one of them will eventually write a Java program", "the rest of them will write Perl"),
            new Joke(7, "['hip', 'hip']", "(hip hip array)"),
            new Joke(8, "To understand what recursion is...", "You must first understand what recursion is"),
            new Joke(9, "There are 10 types of people in this world...", "Those who understand binary and those who don't"),
            new Joke(10, "Which song would an exception sing?", "Can't catch me - Avicii"),
            new Joke(11, "Why do Java programmers wear glasses?", "Because they don't C#"),
            new Joke(12, "How do you check if a webpage is HTML5?", "Try it out on Internet Explorer"),
            new Joke(13, "A user interface is like a joke.", "If you have to explain it then it is not that good."),
            new Joke(14, "I was gonna tell you a joke about UDP...", "...but you might not get it."),
            new Joke(15, "The punchline often arrives before the set-up.", "Do you know the problem with UDP jokes?"),
            new Joke(16, "Why do C# and Java developers keep breaking their keyboards?", "Because they use a strongly typed language."),
            new Joke(17, "Knock-knock.", "A race condition. Who is there?"),
            new Joke(18, "What's the best part about TCP jokes?", "I get to keep telling them until you get them."),
            new Joke(19, "A programmer puts two glasses on their bedside table before going to sleep.", "A full one, in case they gets thirsty, and an empty one, in case they don’t."),
            new Joke(20, "There are 10 kinds of people in this world.", "Those who understand binary, those who don't, and those who weren't expecting a base 3 joke."),
            new Joke(21, "What did the router say to the doctor?", "It hurts when IP."),
            new Joke(22, "An IPv6 packet is walking out of the house.", "He goes nowhere."),
            new Joke(23, "3 SQL statements walk into a NoSQL bar. Soon, they walk out", "They couldn't find a table.")
        };
    }

    public record Joke(int Id, string Setup, string Punchline);
}
