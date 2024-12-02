using Project1;
namespace Project1;

public partial class WelcomePage : ContentPage
{
    public WelcomePage()
    {
        InitializeComponent();
    }
        private async void SkipToLoginPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

}