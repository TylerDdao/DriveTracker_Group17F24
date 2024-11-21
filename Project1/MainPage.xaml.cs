using Project1.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using InTrip;
using Project1.Models;


namespace Project1
{
    public partial class MainPage : ContentPage
    {
        private readonly LocationServices _locationServices;
        private IDispatcherTimer _timer;

        // Assume this driver instance is set somewhere in the application, e.g., AccountPage
        Driver driverInstance;
   

        public MainPage()
        {
            InitializeComponent();
            _locationServices = new LocationServices();
            BindingContext = this;

            

            LoadGoogleMap();
            StartUpdatingLocation();
        }

        private void DisplayAccountInfo()
        {
            // Display account information (adjust this to fit your UI elements)
            DriverNameLabel.Text = ($"{driverInstance.GetName()}");
        }
        // Method to set the Driver instance and display account info
         public void SetDriverInstance(Driver driver) { 
            driverInstance = driver; 
            DisplayAccountInfo(); 
        }


        private async void LoadGoogleMap()
        {
            // Get the current location using device's GPS
            var location = await _locationServices.GetCurrentLocationAsync();
            if (location != null)
            {
                double latitude = location.Latitude;
                double longitude = location.Longitude;

                // Load the map with the current location
                string traffic = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8' />
                    <title>Google Maps with Traffic and Current Location</title>
                    <style>
                        html, body, #map {{
                            height: 100%;
                            margin: 0;
                            padding: 0;
                        }}
                    </style>
                    <script src='https://maps.googleapis.com/maps/api/js?key=AIzaSyCYW11TqCgGPoGYLtb-GtfsqBDfSjS25Vk'></script>
                    <script>
                        function initMap(latitude, longitude) {{
                            const mapOptions = {{
                                center: {{ lat: latitude, lng: longitude }},
                                zoom: 15
                            }};
                            const map = new google.maps.Map(document.getElementById('map'), mapOptions);

                            const trafficLayer = new google.maps.TrafficLayer();
                            trafficLayer.setMap(map);

                            new google.maps.Marker({{
                                position: {{ lat: latitude, lng: longitude }},
                                map: map,
                                title: 'You are here!'
                            }});
                        }}

                        window.onload = () => initMap({latitude}, {longitude});
                    </script>
                </head>
                <body>
                    <div id='map'></div>
                </body>
                </html>
                ";
                GoogleMapWebView.Source = new HtmlWebViewSource
                {
                    Html = traffic
                };
            }
            else
            {
                await DisplayAlert("Error", "Unable to get the current location.", "OK");
                Console.WriteLine("Unable to get the current location.");
            }
        }

        private void StartUpdatingLocation()
        {
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += async (s, e) => await UpdateCurrentLocation();
            _timer.Start();
        }

        private async Task UpdateCurrentLocation()
        {
            try
            {
                var location = await _locationServices.GetCurrentLocationAsync();
                if (location != null)
                {
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;
                    UpdateGoogleMap(latitude, longitude);
                }
                else
                {
                    Console.WriteLine("Unable to get the current location.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void UpdateGoogleMap(double latitude, double longitude)
        {
            string script = $@"
                function updateLocation(latitude, longitude) {{
                    if (typeof map === 'undefined') {{
                        const mapOptions = {{
                            center: {{ lat: latitude, lng: longitude }},
                            zoom: 15
                        }};
                        map = new google.maps.Map(document.getElementById('map'), mapOptions);

                        marker = new google.maps.Marker({{
                            position: {{ lat: latitude, lng: longitude }},
                            map: map,
                            title: 'You are here!'
                        }});
                    }} else {{
                        var newPosition = {{ lat: latitude, lng: longitude }};
                        map.setCenter(newPosition);
                        marker.setPosition(newPosition);
                    }}
                }}
                updateLocation({latitude}, {longitude});
            ";

            GoogleMapWebView.Eval(script);
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            // Navigate to InTripPage
            await Navigation.PushAsync(new InTripPage());

        }
    }
}
