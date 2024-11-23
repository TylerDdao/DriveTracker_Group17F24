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
        // Get current location
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        public event EventHandler<DeviceLocation> LocationChanged;

        public async Task GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(30));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                {
                    // Create DeviceLocation object with the latitude and longitude
                    var deviceLocation = new DeviceLocation(location.Latitude, location.Longitude, location.Speed ?? 0.0);

                    // Trigger the LocationChanged event
                    LocationChanged?.Invoke(this, deviceLocation);
                }
            }
            catch (Exception ex)
            {
                // Handle error here
                Console.WriteLine($"Error getting location: {ex.Message}");
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

        // Listen for location updates
        public async Task OnStartListening()
        {
            try
            {
                // Check and request location permission
                var locationPermission = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (locationPermission != PermissionStatus.Granted)
                {
                    locationPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (locationPermission != PermissionStatus.Granted)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Permission Denied",
                            "Location permission is required to get accurate updates.",
                            "OK"
                        );
                    });
                    return;
                }

                // Fetch initial location to avoid default fallback
                var initialLocation = await Geolocation.GetLocationAsync(
                    new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(30))
                );
                Geolocation.LocationChanged += Geolocation_LocationChanged;

                var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(30));
                var success = await Geolocation.StartListeningForegroundAsync(request);

                if (!success)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Error",
                            "Could not start listening for location updates.",
                            "OK"
                        );
                    });
                }
            }
            catch (Exception ex)
            {
                // Unable to start listening for location changes
                Console.WriteLine($"Error: {ex}");
            }
        }

        void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
        {
            // Process e.Location to get the new location
            var deviceLocation = new DeviceLocation(e.Location.Latitude, e.Location.Longitude, e.Location.Speed ?? 0.0);

            // Trigger the LocationChanged event when location changes
            LocationChanged?.Invoke(this, deviceLocation);
        }

        // Stop listening for location updates
        public void OnStopListening()
        {
            try
            {
                Geolocation.LocationChanged -= Geolocation_LocationChanged;
                Geolocation.StopListeningForeground();
                _isCheckingLocation = false;
            }
            catch (Exception ex)
            {
                // Unable to stop listening for location changes
                Console.WriteLine($"Error stopping location listening: {ex.Message}");
            }
        }
    }
}