using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Project1.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Project1;
using SkiaSharp.Views.Maui.Controls;
using System.Data;

namespace Project1.Data
{
    public class AzureSQLAccess
    {
        private readonly string connectionString;
        

        public AzureSQLAccess()
        {
            // Connection string with SQL authentication
            connectionString = "Server=tcp:dts.database.windows.net,1433;Initial Catalog=DB_DriveTracker;Persist Security Info=False;User ID=dts-admin;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public async Task InsertDriverAsync(Driver driver, Account account)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "INSERT INTO Drivers (FirstName, LastName, Email, Password, Address, PostalCode, LicenseNumber, OverallScore) VALUES (@FirstName, @LastName, @Email, @Password, @Address, @PostalCode, @LicenseNumber, @OverallScore)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@FirstName", driver.GetName());
                cmd.Parameters.AddWithValue("@LastName", driver.getLastName());
                cmd.Parameters.AddWithValue("@Password", account.GetPassword());
                cmd.Parameters.AddWithValue("@Email", account.GetEmail());
                cmd.Parameters.AddWithValue("@Address", driver.GetAddress());
                cmd.Parameters.AddWithValue("@PostalCode", driver.GetPostalCode());
                cmd.Parameters.AddWithValue("@LicenseNumber", driver.GetDriverLicenseNumber());
                cmd.Parameters.AddWithValue("@OverallScore", driver.GetOverallScore());

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateDriverOverallScoreAsync(string email, int newOverallScore)
        {
           

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "UPDATE Drivers SET OverallScore = @OverallScore WHERE Email = @Email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OverallScore", newOverallScore);
                cmd.Parameters.AddWithValue("@Email", email);

                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteDriverAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "DELETE FROM Drivers WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
