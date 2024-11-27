using Project1;

namespace Project1;

public partial class SettingsPage : ContentPage
{

    Driver driverInstance;
	public SettingsPage()
	{
		InitializeComponent();
	}

    public void DisplayInfo()
    {
        DriverName.Text = $"{driverInstance.GetName()}";
        emailEntry.Placeholder = $"{driverInstance.GetAccountEmail()}";
        driverLincense.Placeholder = $"{driverInstance.GetDriverLicenseNumber()}";
    }
    public void SetDriverInstance(Driver driver)
    {
        driverInstance = driver;
        DisplayInfo();
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