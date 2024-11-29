using Project1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Project1.Models;
using Project1;

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
                    // Call to GetSpeedLimitAsync method
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
