using Microsoft.Extensions.Configuration;

namespace BlazorContainerizedApp.Data
{
    public class WeatherForecastService
    {
        protected HttpClient _apiClient;
        protected IConfiguration _configuration;

        public WeatherForecastService(
            HttpClient apiClient,
            IConfiguration configuration)
        {
            _apiClient = apiClient;
            _configuration = configuration;
        }

        public async Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            var _url = _configuration.GetValue<string>("ServiceURL");
            var _message = new HttpRequestMessage(new HttpMethod("GET"), $"{_url}/WeatherForecast");
            var _resultMessage = await _apiClient.SendAsync(_message);
            if (_resultMessage.IsSuccessStatusCode)
            {
                var _result = await _resultMessage.Content.ReadFromJsonAsync<WeatherForecast[]>();
                return _result ?? new WeatherForecast[0];
            }
            else
            {
                return new WeatherForecast[0];
            }
        }
    }
}