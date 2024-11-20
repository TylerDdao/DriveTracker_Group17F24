//using Catel.Reflection;
using Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class LocationServices
    {
        public async Task<double> GetLat()
        {
            double lat = 0;
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();
            lat = location.Latitude;
            return lat;
        }
        public async Task<double> GetLog()
        {
            double log = 0;
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();
            log = location.Longitude;
            return log;
        }
        public async Task<string> GetCachedLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location != null)
                    return $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}, speed:{location.Speed}";
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            return "None";
        }
        //get current location 
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        public event EventHandler<DeviceLocation> LocationChanged;
        public async Task GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
                _cancelTokenSource.Cancel();
        }
        //Lister for location 
        public async Task OnStartListening()
        {
            try
            {
                Geolocation.LocationChanged += Geolocation_LocationChanged;
                var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
                var success = await Geolocation.StartListeningForegroundAsync(request);

                string status = success
                    ? "Started listening for foreground location updates"
                    : "Couldn't start listening";
            }
            catch (Exception ex)
            {
                // Unable to start listening for location changes
            }
        }

        void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
        {
            // Process e.Location to get the new location
            var deviceLocation = new DeviceLocation(e.Location.Latitude, e.Location.Longitude,e.Location.Speed??0.0);

            if (deviceLocation != null)
            {
                LocationChanged?.Invoke(this, deviceLocation);
            }


        }

        //stop listening to location 
        public void OnStopListening()
        {
            try
            {
                Geolocation.LocationChanged -= Geolocation_LocationChanged;
                Geolocation.StopListeningForeground();
                string status = "Stopped listening for foreground location updates";
                _isCheckingLocation = false;
            }
            catch (Exception ex)
            {
                // Unable to stop listening for location changes
            }
        }
    }
}
