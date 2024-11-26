namespace Project1;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
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
        //Prompt user to show that they are already on the page.
        await Navigation.PushAsync(new TripHistoryPage());
    }
    private async void OnButtonSettingsClicked(object sender, EventArgs e)
    {
        
        // Navigate to InTripPage with the driver instance
        await DisplayAlert("Attention", "Currently on the Settings Page.", "OK");
    }
}