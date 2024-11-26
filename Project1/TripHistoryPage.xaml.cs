namespace Project1;

public partial class TripHistoryPage : ContentPage
{
    public TripHistoryPage()
    {
        InitializeComponent();
    }
    private async void OnButtonClicked(object sender, EventArgs e)
    {
        //Prompt user to show that they are already on the page.
        await Navigation.PushAsync(new MainPage());
    }
    private async void OnButtonTripClicked(object sender, EventArgs e)
    {
        // Navigate to InTripPage with the driver instance
        await DisplayAlert("Attention", "Currently on the Trip Page.", "OK");
    }
    private async void OnButtonSettingsClicked(object sender, EventArgs e)
    {
        //Prompt user to show that they are already on the page.
        await Navigation.PushAsync(new SettingsPage());
    }
}