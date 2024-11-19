using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class SpeedLimitServices
    {
        public async Task<string> GetSpeedLimitAsync(double latitude, double longitude)
        {
            string apiKey = "AIzaSyAH6k9NDKMnz-GOzYMcZ5Gc_A1C7M31h-Q";
            string url = $"https://roads.googleapis.com/v1/speedLimits?path={latitude},{longitude}&key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Get response from the API
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        // Debug: Log the raw JSON response
                        Console.WriteLine("API Response: " + jsonResponse);

                        // Parse the JSON response to extract speed limit data
                        var speedData = JsonSerializer.Deserialize<SpeedLimitResponse>(jsonResponse);

                        // Check if speed data is available and return it
                        var speedLimit = speedData?.SpeedLimits?.FirstOrDefault()?.speedLimit;
                        if (speedLimit.HasValue)
                        {
                            return $"{speedLimit.Value} km/h";
                        }
                        else
                        {
                            return "Speed limit not found";
                        }
                    }
                    else
                    {
                        // Log error response from the API
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
                        return "Error retrieving speed limit";
                    }
                }
                catch (Exception ex)
                {
                    // Log exception if occurs
                    Console.WriteLine($"Exception: {ex.Message}");
                    return "Exception occurred while fetching speed limit";
                }
            }
        }

        // Define a model to parse the JSON response
        public class SpeedLimitResponse
        {
            public List<SpeedLimit> SpeedLimits { get; set; }
        }

        public class SpeedLimit
        {
            public int speedLimit { get; set; } // Speed limit in km/h or mph depending on locale
        }
    }
}