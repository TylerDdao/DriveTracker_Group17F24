using Project1.Data;
using Project1.Models;
using System;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;

namespace Project1
{
    public partial class SettingsPage : ContentPage
    {
        private readonly AzureSQLAccess _azureSQLAccess;
        private readonly Account accountInstance;

        public SettingsPage(string email)
        {
            InitializeComponent();
            _azureSQLAccess = new AzureSQLAccess();
            SetFieldsByAccountEmail(email).ConfigureAwait(false);
            
        }

        public async Task SetFieldsByAccountEmail(string email)
        {
            var driverInstance = await _azureSQLAccess.GetDriverByEmailAsync(email);

            if (driverInstance != null)
            {
                // Remove spaces from the fields
                FirstNameEntry.Text = driverInstance.GetName().Replace(" ", "");
                LastNameEntry.Text = driverInstance.getLastName().Replace(" ", "");
                AddressEntry.Text = driverInstance.GetAddress().Replace(" ", "");
                PostalCodeEntry.Text = driverInstance.GetPostalCode().Replace(" ", "");
                LicenseNumberEntry.Text = driverInstance.GetDriverLicenseNumber().Replace(" ", "");
                EmailEntry.Text = email.Replace(" ", "");
                EmailEntry.IsReadOnly = true;  // Make email field view-only
            }
            else
            {
                await DisplayAlert("Error", "Driver not found.", "OK");
            }
        }




        private async void OnUpdateClicked(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                // Create new instances with updated values
                var driverInstance = new Driver(
                    FirstNameEntry.Text,
                    LastNameEntry.Text,
                    EmailEntry.Text,
                    AddressEntry.Text,
                    PostalCodeEntry.Text,
                    LicenseNumberEntry.Text,
                    0 // Assuming OverallScore is managed separately
                );

              

                // Update the driver information in the database
                await _azureSQLAccess.UpdateDriverAsync(driverInstance, accountInstance);

                await DisplayAlert("Success", "Driver information updated successfully.", "OK");
                await Navigation.PopAsync(); // Return to the previous page
            }
        }

        private bool ValidateInputs()
        {
            // Variables
            string email = EmailEntry.Text;
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string address = AddressEntry.Text;
            string postalCode = PostalCodeEntry.Text;
            string licenseNumber = LicenseNumberEntry.Text;
            

            // Status of requirements
            bool isValid = true;

            // Clear error labels
            FirstNameErrorLabel.IsVisible = false;
            LastNameErrorLabel.IsVisible = false;
            EmailErrorLabel.IsVisible = false;
            AddressErrorLabel.IsVisible = false;
            PostalCodeErrorLabel.IsVisible = false;
            LicenseNumberErrorLabel.IsVisible = false;
            

            // Validate First Name
            if (string.IsNullOrWhiteSpace(firstName))
            {
                FirstNameErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Last Name
            if (string.IsNullOrWhiteSpace(lastName))
            {
                LastNameErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Address
            if (string.IsNullOrWhiteSpace(address))
            {
                AddressErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate Postal Code (Canadian Format)
            if (string.IsNullOrWhiteSpace(postalCode) || !Regex.IsMatch(postalCode, @"^[A-Za-z]\\d[A-Za-z] \\d[A-Za-z]\\d$"))
            {
                PostalCodeErrorLabel.Text = string.IsNullOrWhiteSpace(postalCode) ? "Postal Code is required." : "Invalid postal code format.";
                PostalCodeErrorLabel.IsVisible = true;
                isValid = false;
            }

            // Validate License Number
            if (string.IsNullOrWhiteSpace(licenseNumber))
            {
                LicenseNumberErrorLabel.IsVisible = true;
                isValid = false;
            }


            return isValid;
        }

        private async void OnButtonHomeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage(EmailEntry.Text));
        }

        private async void OnButtonTripClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripHistoryPage(EmailEntry.Text));
        }

        private async void OnButtonSettingsClicked(object sender, EventArgs e)
        {
            // Notify the user that they are already on the settings page
            await DisplayAlert("Attention", "Currently on the Settings Page.", "OK");
        }
    }
}
