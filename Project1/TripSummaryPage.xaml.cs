using System.Collections.ObjectModel;
using System.Collections.Generic;
using Project1.Data;
using Project1.Models;
using System;

namespace Project1
{
    public partial class TripSummaryPage : ContentPage
    {
        private readonly AzureSQLAccess _azureSQLAccess;
        private readonly Driver _driver;

        public TripSummaryPage(TimeSpan tripDuration, int tripScore, List<SpeedRecord> exceedingSpeedRecords, Driver driver)
        {
            InitializeComponent();
            _azureSQLAccess = new AzureSQLAccess();
            _driver = driver;

            if (exceedingSpeedRecords == null || exceedingSpeedRecords.Count == 0)
            {
                DisplayAlert("Summary", "No speed violations recorded.", "OK");
                exceedingSpeedRecords = new List<SpeedRecord>();
            }

            BindingContext = new SpeedViewModel(tripDuration, tripScore, exceedingSpeedRecords);
            UpdateDriverScore(tripScore);
            AddTripRecord(_driver.GetAccountEmail(), tripScore);
        }

        private async void UpdateDriverScore(int tripScore)
        {
            await _azureSQLAccess.UpdateDriverOverallScoreAsync(_driver.GetAccountEmail(), tripScore); // Use the driver's email for updating
        }

        private async void AddTripRecord(string email, int tripScore)
        {
            // Generate a trip number or get the latest trip number from the database
            var tripNumber = new Random().Next(1, 1000); // You should implement a better logic for trip numbers
            await _azureSQLAccess.AddTripAsync(email, tripNumber, tripScore);
        }

        public class SpeedViewModel
        {
            public ObservableCollection<SpeedRecord> ExceedingSpeedRecords { get; set; }
            public string TripDuration { get; set; }
            public string TripScore { get; set; }

            public SpeedViewModel(TimeSpan tripDuration, int tripScore, List<SpeedRecord> exceedingSpeedRecords)
            {
                ExceedingSpeedRecords = new ObservableCollection<SpeedRecord>(exceedingSpeedRecords);
                TripDuration = $"Duration: {tripDuration:hh\\:mm\\:ss}";
                TripScore = $"Score: {tripScore}";
            }
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            // Navigate to Main page with the driver's email
            await Navigation.PushAsync(new MainPage(_driver.GetAccountEmail()));
        }
    }
}
