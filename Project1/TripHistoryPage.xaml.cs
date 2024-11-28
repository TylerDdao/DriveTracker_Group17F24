namespace Project1
{
    public partial class TripHistoryPage : ContentPage
    {
        private readonly string _driverEmail;

        public TripHistoryPage(string driverEmail)
        {
            InitializeComponent();
            _driverEmail = driverEmail;
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            // Navigate to MainPage with the driver email
            await Navigation.PushAsync(new MainPage(_driverEmail));
        }

        private async void OnButtonTripClicked(object sender, EventArgs e)
        {
            // Prompt user to show that they are already on the page
            await DisplayAlert("Attention", "Currently on the Trip Page.", "OK");
        }

        private async void OnButtonSettingsClicked(object sender, EventArgs e)
        {
            // Prompt user to show that they are already on the page
            await Navigation.PushAsync(new SettingsPage(_driverEmail));
        }
    }
}
