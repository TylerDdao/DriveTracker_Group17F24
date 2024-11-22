using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Project1.Data;
using Project1.Models;

namespace Project1
{
    public partial class AccountPage : ContentPage
    {
        private readonly AzureSQLAccess _azureSQLAccess;

        public AccountPage()
        {
            InitializeComponent();
            _azureSQLAccess = new AzureSQLAccess();

            // Add event handler for email entry text changed
            EmailEntry.TextChanged += OnEmailEntryTextChanged;
        }

        // Event handler for email entry text changed
        private async void OnEmailEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string email = e.NewTextValue;
            if (await _azureSQLAccess.IsDuplicateEmailAsync(email))
            {
                EmailErrorLabel.Text = "There's an account associated with that email";
                EmailErrorLabel.IsVisible = true;
            }
            else
            {
                EmailErrorLabel.IsVisible = false;
            }
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            // Variables.
            string email = EmailEntry.Text;
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string address = AddressEntry.Text;
            string postalCode = PostalCodeEntry.Text;
            string licenseNumber = LicenseNumberEntry.Text;
            string password = PasswordEntry.Text;
            int overallScore = 0;

            // Status of requirements.
            bool isValid = true;

            // Clear error labels.
            FirstNameErrorLabel.IsVisible = false;
            LastNameErrorLabel.IsVisible = false;
            AddressErrorLabel.IsVisible = false;
            PostalCodeErrorLabel.IsVisible = false;
            LicenseNumberErrorLabel.IsVisible = false;
            PasswordErrorLabel.IsVisible = false;

            // Check if email error label is visible
            if (EmailErrorLabel.IsVisible)
            {
                isValid = false;
            }
            else
            {
                // Validate Email.
                if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                {
                    EmailErrorLabel.Text = string.IsNullOrWhiteSpace(email) ? "Email is required." : "Invalid email address.";
                    EmailErrorLabel.IsVisible = true;
                    isValid = false;
                }
            }

            // Validate First Name.
            if (string.IsNullOrWhiteSpace(firstName))
            {
                FirstNameErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Last Name.
            if (string.IsNullOrWhiteSpace(lastName))
            {
                LastNameErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Address.
            if (string.IsNullOrWhiteSpace(address))
            {
                AddressErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Postal Code (Canadian Format).
            if (string.IsNullOrWhiteSpace(postalCode) || !Regex.IsMatch(postalCode, @"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$"))
            {
                PostalCodeErrorLabel.Text = string.IsNullOrWhiteSpace(postalCode) ? "Postal Code is required." : "Invalid postal code format.";
                PostalCodeErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate License Number.
            if (string.IsNullOrWhiteSpace(licenseNumber))
            {
                LicenseNumberErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Password.
            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordErrorLabel.IsVisible = true;
                isValid = false;
            }

            // If not valid, show an error message and return.
            if (!isValid)
            {
                await DisplayAlert("Validation Error", "Please fill in all required fields.", "OK");
                return;
            }

            // If valid, create a new account and driver.
            var newDriver = new Driver(firstName, lastName, email, address, postalCode, licenseNumber, overallScore);
            var newAccount = new Account(email, password);

            await _azureSQLAccess.InsertDriverAsync(newDriver, newAccount);
            await DisplayAlert("Success", "Account created successfully!", "OK");

            // Navigate to MainPage and pass the driver instance.
            var mainPage = new MainPage();
            mainPage.SetDriverInstance(newDriver);
            await Application.Current.MainPage.Navigation.PushAsync(mainPage);
        }
    }
}
