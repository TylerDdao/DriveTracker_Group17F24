using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    internal class Account
    {
        string email;
        string password;
        string dateCreated;

        // Non-parameterized constructor
        public Account()
        {
            email = "default@example.com";
            password = "defaultPassword";
            dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
        }

        // Parameterized constructor
        public Account(string email, string password)
        {
            this.email = email;
            this.password = password;
            dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
        }
        
        // Set date to dateCreated according to the date on the phone
        public void SetDateCreatedToToday()
        {
            dateCreated = DateTime.Now.ToString("yyyy-MM-dd");
        }

        //Setter for password
        public void SetPassword(string password)
        {
            this.password = password;
        }

        //Check If Password is valid (hase between 6 and 12 characters)
        public bool IsPasswordValid(string password)
        {
            bool password_acceptance = false;
            if (password.Length>=6 && password.Length <= 12)
            {
                password_acceptance = true;
            }
            return password_acceptance; 
        }


        // Set account password from user's input
        // If user password is not valid, then return false and do not set the password to input
        public bool SetAccountPassword(string password)
        {
            if (IsPasswordValid(password))
            {
                this.SetPassword(password);
                return true;
            }
            return false;
        }

        //Setter for email
        public void SetEmail(string email)
        {
            this.email = email;
        }

        // Getter for email
        public string GetEmail()
        {
            return email;
        }

        // Getter for password
        public string GetPassword()
        {
            return password;
        }

        // Getter for dateCreated
        public string GetDateCreated()
        {
            return dateCreated;
        }

    }



}
