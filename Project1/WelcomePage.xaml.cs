namespace Project1;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();
	}
	private async void SkipToMainPageClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new MainPage());
	}
}