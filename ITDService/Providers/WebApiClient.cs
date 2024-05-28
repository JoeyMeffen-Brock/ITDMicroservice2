using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BrockSolutions.ITDService.Providers
{
    public class WebApiClient : IWebApiClient
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _clientFactory;

        public WebApiClient
        (
            ILogger<WebApiClient> logger,
            IHttpClientFactory httpClientFactory
        )
        {
            _logger = logger;
            _clientFactory = httpClientFactory;
        }

        public async Task<string> ExampleCallApiGetRequest()
        {
            var client = _clientFactory.CreateClient("webapiclient");

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{client.BaseAddress}api/exampleApi/getSomething"),
                Method = HttpMethod.Get,
            };

            HttpResponseMessage response = await client.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Web.API request failed with response '{StatusCode}'", response.StatusCode);

                // TODO: Implement according to desired application behaviour.
                // - Could throw an exception or
                // - Return a "defaulted" version of the expected response object
                return response.StatusCode.ToString();
            }

            _logger.LogInformation("Web.API request successful");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<ExampleResponseDto> ExampleCallApiPostRequest()
        {
            var client = _clientFactory.CreateClient("webapiclient");

            var content = new ExampleRequest
            {
                Fruits = new[] { "Strawberry", "Banana", "Apple" },
                NumberOfFruits = 3
            };

            string jsonContent = JsonSerializer.Serialize(content);

            var httpRequest = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{client.BaseAddress}api/exampleApi/postSomething"),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(httpRequest);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Web.API request failed with response '{StatusCode}'", response.StatusCode);

                // TODO: Implement according to desired application behaviour.
                // - Could throw an exception or
                // - Return a "defaulted" version of the expected response object
                throw new NotImplementedException();
            }

            _logger.LogInformation("Web.API request successful");
            var responseContent = await response.Content.ReadFromJsonAsync(typeof(ExampleResponseDto));
            if (responseContent == null)
            {
                _logger.LogWarning("Web.API responded with null");

                // TODO: Implement according to desired application behaviour.
                // - Could throw an exception or
                // - Return a "defaulted" version of the expected response object
                throw new NotImplementedException();
            }
            return (ExampleResponseDto)responseContent;
        }
    }

    public class ExampleRequest
    {
        public IEnumerable<string> Fruits { get; set; }
        public int NumberOfFruits { get; set; }
    }

    public class ExampleResponseDto
    {
        public IEnumerable<string> Colours { get; set; }
        public int NumberOfColours { get; set; }
    }
}
