using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class SpeedLimitServices
    {
        private const string apiKey = "fpyvIoRnt9iTfzLK9fW2b5n-fAQ5b6xYA7L_cyuRNBQ"; // Replace with your actual API key

        public async Task<DeviceLocation> GetSpeedLimitAndLocationAsync(double latitude, double longitude)
        {
            string url = $"https://smap.hereapi.com/v8/maps/attributes?layers=SPEED_LIMITS_FC4,SPEED_LIMITS_FC2&prox={latitude},{longitude},50&apiKey={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var speedData = JsonSerializer.Deserialize<SpeedLimitResponse>(jsonResponse);

                        var speedLimit = speedData?.Layers?.FirstOrDefault()?.Attributes?.FirstOrDefault()?.FromRefSpeedLimit;
                        return new DeviceLocation(latitude, longitude, speedLimit ?? 0);
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
                        return new DeviceLocation(latitude, longitude, 0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return new DeviceLocation(latitude, longitude, 0);
                }
            }
        }

        private class SpeedLimitResponse
        {
            public List<Layer> Layers { get; set; }
        }

        private class Layer
        {
            public List<Attribute> Attributes { get; set; }
        }

        private class Attribute
        {
            public int? FromRefSpeedLimit { get; set; }
        }
    }
}
