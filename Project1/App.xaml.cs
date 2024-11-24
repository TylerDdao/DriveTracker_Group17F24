namespace Project1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //Set AccountPage as the startup page.
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
