using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Project1
{
    public partial class TripSummaryPage : ContentPage
    {
        public TripSummaryPage(TimeSpan tripDuration, int tripScore, List<SpeedRecord> exceedingSpeedRecords)
        {
            InitializeComponent();

            if (exceedingSpeedRecords == null || exceedingSpeedRecords.Count == 0)
            {
                DisplayAlert("Summary", "No speed violations recorded.", "OK");
                exceedingSpeedRecords = new List<SpeedRecord>();
            }

            BindingContext = new SpeedViewModel(tripDuration, tripScore, exceedingSpeedRecords);
        }
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
}
