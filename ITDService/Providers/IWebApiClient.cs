namespace BrockSolutions.ITDService.Providers
{
    public interface IWebApiClient
    {
        Task<string> ExampleCallApiGetRequest();
        Task<ExampleResponseDto> ExampleCallApiPostRequest();
    }
}