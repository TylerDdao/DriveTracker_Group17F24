﻿using Project1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class LocationServices
    {
        
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
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
            }
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
                    new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(5))
                );
                Geolocation.LocationChanged += Geolocation_LocationChanged;
                var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
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
