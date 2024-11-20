using Project1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace InTrip
{
    public partial class InTripPage : ContentPage
    {
        private readonly LocationServices _locationServices = new LocationServices();
        private readonly SpeedLimitServices _speedLimitServices = new SpeedLimitServices();

        public InTripPage()
        {
            InitializeComponent();
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

                // Get the speed limit and update the UI
                try
                {
                    var speedLimit = await _speedLimitServices.GetSpeedLimitAsync(deviceLocation.Latitude, deviceLocation.Longitude);
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        SpeedLimitLabel.Text = speedLimit.HasValue ? $"{speedLimit.Value} km/h" : "No speed limit data available";
                    });
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
            _locationServices.StopListening();
            await DisplayAlert("Trip ended", "Location updates stopped.", "Ok");
        }
    }
}
