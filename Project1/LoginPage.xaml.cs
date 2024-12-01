using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;
using Project1.Data;
using Project1.Models;
//Group 17 - Drive Tracker.
namespace Project1
{
    public partial class LoginPage : ContentPage
    {
        private readonly AzureSQLAccess _azureSQLAccess;
        public LoginPage()
        {
            InitializeComponent();
            _azureSQLAccess = new AzureSQLAccess();
        }
        //Login Button ClickedEvent.
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            //Email password entry.
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;
            //Bool operation.
            bool isValid = true;

            // Clear error labels
            EmailErrorLabel.IsVisible = false;
            PasswordErrorLabel.IsVisible = false;

            // Validate Email
            //Condition.
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                EmailErrorLabel.Text = string.IsNullOrWhiteSpace(email) ? "Email is required." : "Invalid email address.";
                EmailErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(password))
            {
                //Validation.
                PasswordErrorLabel.Text = "Password is required.";
                PasswordErrorLabel.IsVisible = true;
                isValid = false;
            }

            // If validation fails, return
            if (!isValid)
            {
                return;
            }

            // Check if the driver credentials are valid
            bool isDriverValid = await _azureSQLAccess.IsDriverValidAsync(email, password);
            if (isDriverValid)
            {
                // Fetch the driver's data
                Driver driverData = await _azureSQLAccess.GetDriverByEmailAsync(email);
                //Condition.
                if (driverData != null)
                {
                    // Create a new Account instance
                    var newAccount = new Account(email, password);

                    // Create a new Driver instance with fetched data
                    var newDriver = new Driver(
                        driverData.GetName(),
                        driverData.getLastName(),
                        driverData.GetAccountEmail(),
                        driverData.GetAddress(),
                        driverData.GetPostalCode(),
                        driverData.GetDriverLicenseNumber(),
                        driverData.GetOverallScore()
                    );

                    // Set the driver's instance in the MainPage
                    var mainPage = new MainPage();
                    mainPage.SetDriverInstance(newDriver);

                    await DisplayAlert("Success", "Logged in successfully!", "OK");
                    // Navigate to the MainPage
                    await Navigation.PushAsync(mainPage);
                }
            }
            else
            {
                //Exception handling.
                await DisplayAlert("Error", "Invalid email or password.", "OK");              
            }
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {        
            // Navigate to the registration page
            await Navigation.PushAsync(new AccountPage());

        }
    }
}
