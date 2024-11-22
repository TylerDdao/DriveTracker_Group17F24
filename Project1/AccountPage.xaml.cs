using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Project1.Data;
using Project1.Models;
namespace Project1;

public partial class AccountPage : ContentPage
{
    private readonly AzureSQLAccess _azureSQLAccess;
    public AccountPage()
    {
        InitializeComponent();
        _azureSQLAccess = new AzureSQLAccess();
    }

    //Event Handler.
    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        //Variables.
        //Field texts.
        string email = EmailEntry.Text;
        string firstName = FirstNameEntry.Text;
        string lastName = LastNameEntry.Text;
        string address = AddressEntry.Text;
        string postalCode = PostalCodeEntry.Text;
        string licenseNumber = LicenseNumberEntry.Text;
        string password = PasswordEntry.Text;
        int overallScore = 0;
        //Status of requirements.
        bool isValid = true;
        //Error lable clear.
        EmailErrorLabel.IsVisible = false;
        FirstNameErrorLabel.IsVisible = false;
        LastNameErrorLabel.IsVisible = false;
        AddressErrorLabel.IsVisible = false;
        PostalCodeErrorLabel.IsVisible = false;
        LicenseNumberErrorLabel.IsVisible = false;
        PasswordErrorLabel.IsVisible = false;
        //Validate Email.
        //Condition.
        if (string.IsNullOrWhiteSpace(email))
        {
            //Email Field Status.
            EmailErrorLabel.IsVisible = true;
            isValid = false;
        }
        else
        {
            //Expression for Email Address.
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            //Condition.
            if (!Regex.IsMatch(email, emailPattern))
            {
                //Set text field.
                EmailErrorLabel.Text = "Invalid email address.";
                //Email field Status.
                EmailErrorLabel.IsVisible = true;
                isValid = false;
            }
        }
        //Validate First Name
        //Condition.
        if (string.IsNullOrWhiteSpace(firstName))
        {
            //First Name Field Status.
            FirstNameErrorLabel.IsVisible = true;
            isValid = false;
        }
        // Validate Last Name.
        //Condtion.
        if (string.IsNullOrWhiteSpace(lastName))
        {
            //Last Name Field Status.
            LastNameErrorLabel.IsVisible = true;
            isValid = false;
        }
        // Validate Address
        //Condition.
        if (string.IsNullOrWhiteSpace(address))
        {
            //Address Field Status.
            AddressErrorLabel.IsVisible = true;
            isValid = false;
        }
        // Validate Postal Code (Canadian Format)
        //Condition.
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            //Postal Code Field Status.
            PostalCodeErrorLabel.IsVisible = true;
            isValid = false;
        }
        else
        {
            //Expression for Postal Code.
            var postalCodePattern = @"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$";
            //Condition.
            if (!Regex.IsMatch(postalCode, postalCodePattern))
            {
                //Set text field.
                PostalCodeErrorLabel.Text = "Invalid postal code format.";
                //Postal code Field Status.
                PostalCodeErrorLabel.IsVisible = true;
                isValid = false;
            }
        }

        //Validate License Number.
        if (string.IsNullOrWhiteSpace(licenseNumber))
        {
            //License Number Field Status.
            LicenseNumberErrorLabel.IsVisible = true;
            isValid = false;
        }

        //Validate Password.
        if (string.IsNullOrWhiteSpace(password))
        {
            //Password Field Status.
            PasswordErrorLabel.IsVisible = true;
            isValid = false;
        }

        //Condition.
        if (!isValid)
        {
            //Prompt.
            await DisplayAlert("Validation Error", "Please fill in all required fields.", "OK");
            return;
        }
        else
        {
            //Prompt.
            await DisplayAlert("Success", "Account created successfully!", "OK");

            //*** DRIVER CLASS ***
            Driver newDriver = new Driver(firstName, lastName, email, address, postalCode, licenseNumber, overallScore);

            //*** Account CLASS ***
            Account newAccount = new Account(email, password);

            

            //*** DATABASE ***          
            await _azureSQLAccess.InsertDriverAsync(newDriver, newAccount);

            //*** NAVIGATE TO MAIN ***
            
            // Navigate to MainPage and pass the driver instance
            var mainPage = new MainPage(); 
            mainPage.SetDriverInstance(newDriver); 
            await Application.Current.MainPage.Navigation.PushAsync(mainPage); 
        }
    }
}




    