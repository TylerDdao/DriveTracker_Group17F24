using Project1.Services;
namespace InTrip
{
    public partial class InTripPage : ContentPage
    {

        private readonly LocationServices _locationServices = new LocationServices();
        private readonly SpeedLimitServices speedLimitServices = new SpeedLimitServices();
        public InTripPage()
        {
            InitializeComponent();
            StartListeningForLocationUpdates();

        }
        private async void StartListeningForLocationUpdates()
        {
            _locationServices.LocationChanged += (sender, deviceLocation) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    LatitudeText.Text = $"{deviceLocation.latitude}";
                    LongitudeText.Text = $"{deviceLocation.longitude}";
                });
                // Update the UI with the current speed
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CurrentSpeedLabel.Text = $"{deviceLocation.speed} km/h";
                });

                // Get the speed limit and update the UI
                var speedLimit = speedLimitServices.GetSpeedLimitAsync(deviceLocation.latitude, deviceLocation.longitude);
                speedLimit.ContinueWith(task =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        SpeedLimitLabel.Text = task.Result + " km/h";
                    });
                });
            };

                _locationServices.OnStartListening();

        }


       
        private async void OnEndTripButtonClicked(object sender, EventArgs e)
        {
            _locationServices.OnStopListening();
            await DisplayAlert("Trip ended", "Location stoped", "Ok");
        }
    }
}