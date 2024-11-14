using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class AzureMapsService
{
    private readonly string _subscriptionKey;
    private readonly HttpClient _httpClient;

    public AzureMapsService(string subscriptionKey)
    {
        _subscriptionKey = subscriptionKey;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GetMapImageAsync(double latitude, double longitude, int zoomLevel = 10, int width = 512, int height = 512)
    {
        var requestUri = $"https://atlas.microsoft.com/map/static/png?subscription-key={_subscriptionKey}" +
                         $"&api-version=1.0&center={longitude},{latitude}&zoom={zoomLevel}&width={width}&height={height}";

        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        return response.RequestMessage.RequestUri.ToString(); // Return URL for image
    }

    public async Task<dynamic> GetReverseGeocodeAsync(double latitude, double longitude)
    {
        var requestUri = $"https://atlas.microsoft.com/search/address/reverse/json?subscription-key={_subscriptionKey}" +
                         $"&api-version=1.0&query={latitude},{longitude}";

        var response = await _httpClient.GetStringAsync(requestUri);
        return JsonConvert.DeserializeObject(response);
    }
}
