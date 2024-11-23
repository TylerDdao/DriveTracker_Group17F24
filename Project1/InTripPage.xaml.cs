using Project1.Models;
using Project1.Services;
namespace InTrip
{
    public partial class InTripPage : ContentPage
    {

        private LocationServices _locationServices;
        private SpeedLimitServices speedLimitServices;
        public InTripPage()
        {
            InitializeComponent();
            _locationServices = new LocationServices();
            _locationServices.LocationChanged += OnLocationChanged;
            speedLimitServices = new SpeedLimitServices();
            _locationServices.OnStartListening();
        }



        private void OnLocationChanged(object sender, DeviceLocation location)
        {
            LatitudeText.Text = $"Latitude:{location.latitude}";
            LongitudeText.Text = $"Longitude:{location.longitude}";
            CurrentSpeedLabel.Text = $"{location.speed} km/h";
            var speedLimit = speedLimitServices.GetSpeedLimitAsync(location.latitude, location.longitude);
            speedLimit.ContinueWith(task =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SpeedLimitLabel.Text = task.Result + " km/h";
                });
            });
        }
        
        private async void OnEndTripButtonClicked(object sender, EventArgs e)
        {
            _locationServices.OnStopListening();
            await DisplayAlert("Trip ended", "Location stoped", "Ok");
        }
        private async void HomeClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}