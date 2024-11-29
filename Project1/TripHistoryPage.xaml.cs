using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Project1.Data;
using System.Threading.Tasks;
using System.Linq;

namespace Project1
{
    public partial class TripHistoryPage : ContentPage
    {
        private readonly string _driverEmail;
        private readonly AzureSQLAccess _azureSQLAccess;

        public ObservableCollection<ListItem> Items { get; set; }

        public TripHistoryPage(string driverEmail)
        {
            InitializeComponent();
            _driverEmail = driverEmail;
            _azureSQLAccess = new AzureSQLAccess();

            Items = new ObservableCollection<ListItem>();

            ItemsCollectionView.ItemsSource = Items;

            // Fetch and display the trip records
            FetchTripRecordsAsync();
        }

        private async void FetchTripRecordsAsync()
        {
            var tripRecords = await _azureSQLAccess.GetTripsByEmailAsync(_driverEmail);
            var scores = new List<int>();

            foreach (var trip in tripRecords)
            {
                Items.Add(new ListItem
                {
                    ItemName = $"TripID {trip.TripNumber} - Score: {trip.OverallScore}"
                });
                scores.Add(trip.OverallScore);
            }

            // Calculate and display the average score
            if (scores.Count > 0)
            {
                double averageScore = scores.Average();
                AverageScoreLabel.Text = $"Average Score: {averageScore:F2}";
            }
            else
            {
                AverageScoreLabel.Text = "No trips available to calculate average score.";
            }
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            // Navigate to MainPage with the driver email
            await Navigation.PushAsync(new MainPage(_driverEmail));
        }

        private async void OnButtonTripClicked(object sender, EventArgs e)
        {
            // Prompt user to show that they are already on the page
            await DisplayAlert("Attention", "Currently on the Trip Page.", "OK");
        }

        private async void OnButtonSettingsClicked(object sender, EventArgs e)
        {
            // Navigate to SettingsPage with the driver email
            await Navigation.PushAsync(new SettingsPage(_driverEmail));
        }
    }

    public class ListItem
    {
        public string ItemName { get; set; }
    }
}
