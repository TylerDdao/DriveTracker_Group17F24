using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    public class Driver
    {
        string accountEmail;
        string lastName;
        string firstName;
        string address;
        string postalCode;
        string driverLicenseNumber;
        int overallScore;

        // Non-parameterized constructor
        public Driver()
        {
            accountEmail = "default@example.com";
            lastName = "Default lastname";
            firstName = "Default Name";
            address = "Default Address";
            postalCode = "1A1A1A";
            driverLicenseNumber = "0";
            overallScore = 0;
        }

        // Parameterized constructor
        public Driver(string firstName, string lastName, string accountEmail, string address, string postalCode, string driverLicenseNumber, int overallScore)
        {
            this.accountEmail = accountEmail;
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.postalCode = postalCode;
            this.driverLicenseNumber = driverLicenseNumber;
            this.overallScore = overallScore;
        }

        // Setter for accountEmail
        public void SetAccountEmail(string accountEmail)
        {
            this.accountEmail = accountEmail;
        }

        // Getter for accountEmail
        public string GetAccountEmail()
        {
            return accountEmail;
        }
        public string getLastName()
        {

            return this.lastName;
        }

        public void setLastName(string Lastname)
        {
            this.lastName = Lastname;
        }

        // Setter for name
        public void SetName(string name)
        {
            this.firstName = name;
        }

        // Getter for name
        public string GetName()
        {
            return firstName;
        }

        // Setter for address
        public void SetAddress(string address)
        {
            this.address = address;
        }

        // Getter for address
        public string GetAddress()
        {
            return address;
        }

        // Setter for postalCode
        public void SetPostalCode(string postalCode)
        {
            this.postalCode = postalCode;
        }

        // Getter for postalCode
        public string GetPostalCode()
        {
            return postalCode;
        }

        // Setter for driverLicenseNumber
        public void SetDriverLicenseNumber(string driverLicenseNumber)
        {
            this.driverLicenseNumber = driverLicenseNumber;
        }

        // Getter for driverLicenseNumber
        public string GetDriverLicenseNumber()
        {
            return driverLicenseNumber;
        }

        // Setter for overallScore
        public void SetOverallScore(int overallScore)
        {
            this.overallScore = overallScore;
        }

        // Getter for overallScore
        public int GetOverallScore()
        {
            return overallScore;
        }
        // Check if postal code format is suitable (AIAIAI A=Capital letter, I=digit from 0-9)
        public bool CheckPostalCode(string postalCode)
        {
            if (postalCode.Length != 6)
            {
                return false;
            }

            for (int i = 0; i < postalCode.Length; i++)
            {
                if (i % 2 == 0) // Expecting a capital letter at even indices
                {
                    if (!char.IsUpper(postalCode[i]) || char.IsDigit(postalCode[i]))
                    {
                        return false;
                    }
                }
                else // Expecting a digit at odd indices
                {
                    if (!char.IsDigit(postalCode[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // validate and set postal code
        public bool CheckAndSetPostalCode(string postalCode)
        {
            if (CheckPostalCode(postalCode))
            {
                SetPostalCode(postalCode);
                return true;
            }
            return false;
        }
        // Function to check if name is between 2 and 30 characters long
        public bool CheckName(string name)
        {
            bool nameAcceptance = false;
            if (name.Length >= 2 && name.Length <= 30)
            {
                nameAcceptance = true;
            }
            return nameAcceptance;
        }
        public bool CheckAddress(string address)
        {
            return address.Length >= 1 && address.Length < 50;
        }
        // Function to check and set name
        public bool CheckAndSetName(string name)
        {
            if (CheckName(name))
            {
                SetName(name);
                return true;
            }
            return false;
        }

        // Function to check and set address
        public bool CheckAndSetAddress(string address)
        {
            if (CheckAddress(address))
            {
                SetAddress(address);
                return true;
            }
            return false;
        }
    }
}
