using Project1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Project1;
using SkiaSharp.Views.Maui.Controls;

namespace InTrip
{
    public partial class InTripPage : ContentPage
    {
        private readonly LocationServices _locationServices = new LocationServices();
        private readonly SpeedLimitServices _speedLimitServices = new SpeedLimitServices();
        private Trip currentTrip;

        public InTripPage()
        {
            InitializeComponent();
            currentTrip = new Trip();
            StartListeningForLocationUpdates();
        }

        private void StartListeningForLocationUpdates()
        {
            _locationServices.LocationChanged += async (sender, deviceLocation) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LatitudeText.Text = $"{deviceLocation.Latitude}";
                    LongitudeText.Text = $"{deviceLocation.Longitude}";
                    CurrentSpeedLabel.Text = $"{deviceLocation.Speed} km/h";
                });

                try
                {
                    var speedLimit = await _speedLimitServices.GetSpeedLimitAsync(deviceLocation.Latitude, deviceLocation.Longitude);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        SpeedLimitLabel.Text = speedLimit.HasValue ? $"{speedLimit.Value} km/h" : "No speed limit data available";
                    });

                    if (speedLimit.HasValue)
                    {
                        currentTrip.AddSpeedRecordIfExceedsLimit(deviceLocation.Speed, speedLimit.Value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching speed limit: {ex.Message}");
                }
            };

            _locationServices.StartListening();
        }

        private async void OnEndTripButtonClicked(object sender, EventArgs e)
        {
            try
            {
                _locationServices.StopListening();
                currentTrip.EndTrip(); // Record the end time of the trip
                currentTrip.CalculateScore(); // Calculate the trip score
                await DisplayAlert("Trip ended", "Location updates stopped.", "Ok");

                // Navigate to TripSummaryPage and pass the trip data
                await Navigation.PushAsync(new TripSummaryPage(currentTrip.Duration, currentTrip.Score, currentTrip.ExceedingSpeedRecords));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during navigation: {ex.Message}");
                await DisplayAlert("Error", "An error occurred while ending the trip.", "OK");
            }
        }
    }
}
