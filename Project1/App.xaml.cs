namespace Project1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

 
            MainPage = new NavigationPage(new WelcomePage());

            //Set AccountPage as the startup page.
            //MainPage = new NavigationPage(new AccountPage());
            //MainPage = new NavigationPage(new MainPage());
 
        }
    }
}
