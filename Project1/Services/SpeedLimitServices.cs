using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class SpeedLimitServices
    {
        private const string apiKey = "VrjJPxekgdcQ5QK0YXk2PipVaMYGd_qkyeAhLQUe35I"; // Replace with your actual API key
        private DeviceLocation _currentLocation;

        public void SetCurrentLocation(DeviceLocation location)
        {
            _currentLocation = location;
        }

        public async Task<int?> GetSpeedLimitAsync()
        {
            if (_currentLocation == null)
            {
                Console.WriteLine("Current location is not set.");
                return null;
            }

            int? speedLimit = null;

            string url = $"https://routematching.hereapi.com/v8/match/routelinks?apikey={apiKey}&waypoint0={_currentLocation.Latitude},{_currentLocation.Longitude}&waypoint1={_currentLocation.Latitude},{_currentLocation.Longitude}&mode=fastest;car&routeMatch=1&attributes=SPEED_LIMITS_FCN(*)";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        var routeResponse = JsonSerializer.Deserialize<RouteResponse>(jsonResponse);

                        if (routeResponse?.response?.route != null && routeResponse.response.route.Count > 0)
                        {
                            var route = routeResponse.response.route[0];

                            foreach (var leg in route.leg)
                            {
                                foreach (var link in leg.link)
                                {
                                    if (link.attributes != null && link.attributes.SPEED_LIMITS_FCN != null)
                                    {
                                        var speedLimitInfo = link.attributes.SPEED_LIMITS_FCN[0];
                                        if (!string.IsNullOrEmpty(speedLimitInfo.FROM_REF_SPEED_LIMIT))
                                        {
                                            speedLimit = int.Parse(speedLimitInfo.FROM_REF_SPEED_LIMIT);
                                            break;
                                        }
                                    }
                                    if (speedLimit.HasValue)
                                    {
                                        break;
                                    }
                                }
                                if (speedLimit.HasValue)
                                {
                                    break;
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

            return speedLimit;
        }

        private class RouteResponse
        {
            public Response response { get; set; }
        }

        private class Response
        {
            public List<Route> route { get; set; }
        }

        private class Route
        {
            public List<Leg> leg { get; set; }
        }

        private class Leg
        {
            public List<Link> link { get; set; }
        }

        private class Link
        {
            public Attributes attributes { get; set; }
        }

        private class Attributes
        {
            public List<SpeedLimitInfo> SPEED_LIMITS_FCN { get; set; }
        }

        private class SpeedLimitInfo
        {
            public string FROM_REF_SPEED_LIMIT { get; set; }
        }
    }
}
