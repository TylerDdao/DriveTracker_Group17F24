using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class SpeedLimitServices
    {
        private const string apiKey = "VrjJPxekgdcQ5QK0YXk2PipVaMYGd_qkyeAhLQUe35I"; // Replace with your actual API key

        public async Task<int?> GetSpeedLimitAsync()
        {
            var tileIds = new List<string> { "24283388"}; // Add more as needed
            int? speedLimit = null;

            foreach (var tileId in tileIds)
            {
                string url = $"https://smap.hereapi.com/v8/maps/attributes?layers=SPEED_LIMITS_FC4&in=tile:{tileId}&apiKey={apiKey}";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            var speedData = JsonSerializer.Deserialize<SpeedLimitResponse>(jsonResponse);

                            foreach (var tile in speedData.Tiles)
                            {
                                foreach (var row in tile.Rows)
                                {
                                    if (!string.IsNullOrEmpty(row.FROM_REF_SPEED_LIMIT))
                                    {
                                        speedLimit = int.Parse(row.FROM_REF_SPEED_LIMIT);
                                    }
                                    if (!string.IsNullOrEmpty(row.TO_REF_SPEED_LIMIT))
                                    {
                                        speedLimit = int.Parse(row.TO_REF_SPEED_LIMIT);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                    }
                }

                if (speedLimit.HasValue)
                {
                    break;
                }
            }
            return speedLimit;
        }

        private class SpeedLimitResponse
        {
            public List<Tile> Tiles { get; set; }
        }

        private class Tile
        {
            public List<Row> Rows { get; set; }
        }

        private class Row
        {
            public string FROM_REF_SPEED_LIMIT { get; set; }
            public string TO_REF_SPEED_LIMIT { get; set; }
        }
    }
}
