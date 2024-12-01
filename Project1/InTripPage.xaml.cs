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

        private void StartListeningForLocationUpdates()
        {
            _locationServices.LocationChanged += async (sender, deviceLocation) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LatitudeText.Text = $"{deviceLocation.Latitude}";
                    LongitudeText.Text = $"{deviceLocation.Longitude}";
                    double speedInKmh = deviceLocation.Speed; // Convert speed to km/h * 3.6
                    CurrentSpeedLabel.Text = $"{speedInKmh} km/h";
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
                    SpeedLimitLabel.Text = speedLimit.HasValue ? $"{speedLimit.Value} km/h" : "No speed limit data available";
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

    public class SpeedLimitResponse
    {
        public List<Tile> Tiles { get; set; }
    }

    public class Tile
    {
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        public string FROM_REF_SPEED_LIMIT { get; set; }
        public string TO_REF_SPEED_LIMIT { get; set; }
    }
}
