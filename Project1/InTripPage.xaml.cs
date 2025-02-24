using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Project1;
using Project1.Models;
using Project1.Services;

namespace InTrip
{
    public partial class InTripPage : ContentPage
    {
        private readonly LocationServices _locationServices;
        private readonly SpeedLimitServices _speedLimitServices;
        private Driver _driver;
        private Trip currentTrip;

        public InTripPage(Driver driver)
        {
            InitializeComponent();
            _locationServices = new LocationServices();
            _speedLimitServices = new SpeedLimitServices();
            currentTrip = new Trip();
            _driver = driver;

            StartListeningForLocationUpdates();
            RefreshSpeedLimitAsync();
        }
        //Every time the location is updated it adds a record if voilation is made.
        private void StartListeningForLocationUpdates()
        {
            _locationServices.LocationChanged += async (sender, deviceLocation) =>
            {
                _speedLimitServices.SetCurrentLocation(deviceLocation);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LatitudeText.Text = $"{deviceLocation.Latitude}";
                    LongitudeText.Text = $"{deviceLocation.Longitude}";
                    double speedInKmh = deviceLocation.Speed;
                    CurrentSpeedLabel.Text = $"{speedInKmh} km/h";
                });

                var speedLimit = await _speedLimitServices.GetSpeedLimitAsync();
                
                

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (speedLimit != null)
                    {
                        currentTrip.AddSpeedRecordIfExceedsLimit(deviceLocation.Speed, speedLimit.Value);
                    }
                    SpeedLimitLabel.Text = $"{speedLimit} km/h"; // Always show speed limit
                });
            };

            _locationServices.StartListening();
        }

        private async Task RefreshSpeedLimitAsync()
        {
            while (true)
            {
                await Task.Delay(5000); // 5 seconds delay
                var speedLimit = await _speedLimitServices.GetSpeedLimitAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SpeedLimitLabel.Text = $"{speedLimit} km/h"; // Always show speed limit
                });
            }
        }

        private async void OnEndTripButtonClicked(object sender, EventArgs e)
        {
            try
            {
                _locationServices.StopListening();
                currentTrip.EndTrip();
                currentTrip.CalculateScore();
                await DisplayAlert("Trip ended", "Location updates stopped.", "OK");

                await Navigation.PushAsync(new TripSummaryPage(currentTrip.Duration, currentTrip.Score, currentTrip.ExceedingSpeedRecords, _driver));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during navigation: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while ending the trip.", "OK");
            }
        }
    }
}
