﻿using Catel.Reflection;
using Project1.Models;
using System;
using System.Threading.Tasks;


namespace Project1.Services
{
    public class LocationServices
    {
        public event EventHandler<DeviceLocation> LocationChanged;

        public async Task StartListening()
        {
            try
            {
                Geolocation.LocationChanged += OnLocationChanged;
                var request = new GeolocationListeningRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));
                var success = await Geolocation.StartListeningForegroundAsync(request);
                Console.WriteLine(success ? "Started listening for location updates" : "Couldn't start listening");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to start listening for location changes: {ex.Message}");
            }
        }

        public void StopListening()
        {
            try
            {
                Geolocation.LocationChanged -= OnLocationChanged;
                Geolocation.StopListeningForeground();
                Console.WriteLine("Stopped listening for location updates");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to stop listening for location changes: {ex.Message}");
            }
        }

        private void OnLocationChanged(object sender, GeolocationLocationChangedEventArgs e)
        {
            var deviceLocation = new DeviceLocation(e.Location.Latitude, e.Location.Longitude, e.Location.Speed ?? 0.0);
            LocationChanged?.Invoke(this, deviceLocation);
        }

        public async Task<Location> GetCurrentLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                return await Geolocation.GetLocationAsync(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get location: {ex.Message}");
                return null;
            }
        }
    }

    public class DeviceLocation
    {
        public double Latitude { get; }
        public double Longitude { get; }
        public double Speed { get; }

        public DeviceLocation(double latitude, double longitude, double speed)
        {
            Latitude = latitude;
            Longitude = longitude;
            Speed = speed;
        }
    }
}
