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

        public async Task<int?> GetSpeedLimitAsync(double latitude, double longitude)
        {
            // URL remains unchanged
            string url = $"https://smap.hereapi.com/v8/maps/attributes?layers=SPEED_LIMITS_FC4&in=tile:24275195&apiKey=VrjJPxekgdcQ5QK0YXk2PipVaMYGd_qkyeAhLQUe35I";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var speedData = JsonSerializer.Deserialize<SpeedLimitResponse>(jsonResponse);

                        // Log the full response to troubleshoot
                        Console.WriteLine("Full JSON Response: " + jsonResponse);

                        // Implement more detailed logging for each row
                        foreach (var tile in speedData.Tiles)
                        {
                            foreach (var row in tile.Rows)
                            {
                                Console.WriteLine($"LINK_ID: {row.LINK_ID}, FROM_REF_SPEED_LIMIT: {row.FROM_REF_SPEED_LIMIT}, TO_REF_SPEED_LIMIT: {row.TO_REF_SPEED_LIMIT}");
                                
                                if (!string.IsNullOrEmpty(row.FROM_REF_SPEED_LIMIT))
                                {
                                    return int.Parse(row.FROM_REF_SPEED_LIMIT);
                                }
                                if (!string.IsNullOrEmpty(row.TO_REF_SPEED_LIMIT))
                                {
                                    return int.Parse(row.TO_REF_SPEED_LIMIT);
                                }
                            }
                        }

                        return null; // No speed limit found
                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode}, {errorResponse}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return null;
                }
            }
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
            public string LINK_ID { get; set; }
            public string FROM_REF_SPEED_LIMIT { get; set; }
            public string TO_REF_SPEED_LIMIT { get; set; }
        }
    }
}
