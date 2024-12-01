using Project1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Project1;
using SkiaSharp.Views.Maui.Controls;
using Project1.Models;

namespace Project1;

public partial class InTripPage : ContentPage
{
    private readonly LocationServices _locationServices = new LocationServices();
    private readonly SpeedLimitServices _speedLimitServices = new SpeedLimitServices();
    private Trip currentTrip;
    private Driver _driver; // Add this field to store the driver information

    public InTripPage(Driver driver) // Modify the constructor to take the Driver object
    {
        InitializeComponent();
        currentTrip = new Trip();
        _driver = driver; // Store the driver information
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

            // Navigate to TripSummaryPage and pass the trip data along with the driver information
            await Navigation.PushAsync(new TripSummaryPage(currentTrip.Duration, currentTrip.Score, currentTrip.ExceedingSpeedRecords, _driver));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during navigation: {ex.Message}");
            await DisplayAlert("Error", "An error occurred while ending the trip.", "OK");
        }
    }
    private async void OnHomeButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
