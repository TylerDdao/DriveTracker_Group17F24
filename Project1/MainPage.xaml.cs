
﻿using Project1.Services;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using InTrip;
using Project1.Models;
using Project1.Data;


﻿using InTrip;
using Project1.Services;

namespace Project1
{
    public partial class MainPage : ContentPage
    {
        private readonly LocationServices _locationServices;
        private readonly AzureSQLAccess _azureSQLAccess;
        private IDispatcherTimer _timer;
        private Driver driverInstance;

        // Constructor that accepts email parameter
        public MainPage(string email)
        {
            InitializeComponent();
            _locationServices = new LocationServices();
            _azureSQLAccess = new AzureSQLAccess();
            BindingContext = this;
            LoadGoogleMap();
            StartUpdatingLocation();
            SetDriverInstanceByEmail(email); // Fetch and set the driver instance by email
        }

        public MainPage()
        {
            InitializeComponent();
            _locationServices = new LocationServices();
            _azureSQLAccess = new AzureSQLAccess();
            BindingContext = this;
            LoadGoogleMap();
            StartUpdatingLocation();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await FetchDriverInfo(); // Fetch driver information when the page appears
        }

        private void DisplayAccountInfo()
        {
            // Display account information (adjust this to fit your UI elements)
            if (driverInstance != null)
            {
                DriverNameLabel.Text = driverInstance.GetName();
            }
        }

        // Method to fetch the driver instance by email
        private async Task SetDriverInstanceByEmail(string email)
        {
            driverInstance = await _azureSQLAccess.GetDriverByEmailAsync(email);
            if (driverInstance != null)
            {
                DisplayAccountInfo();
            }
        }

        // Method to fetch the driver instance from the database
        private async Task FetchDriverInfo()
        {
            if (driverInstance != null)
            {
                driverInstance = await _azureSQLAccess.GetDriverByEmailAsync(driverInstance.GetAccountEmail());
                DisplayAccountInfo();
            }
        }

        // Method to set the Driver instance and display account info
        public void SetDriverInstance(Driver driver)
        {
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

        public async Task<string> GetTrafficHtmlAsync()
        {
            LocationServices location = new LocationServices();
            // Create an instance of the LocationService

            // Retrieve the latitude
            double lat = await location.GetLat();
            double log = await location.GetLog();

            // Generate the HTML
            string traffic = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset=""utf-8"" />
        <title>Google Maps with Traffic and Current Location</title>
        <style>
            html, body, #map {{
                height: 100%;
                margin: 0;
                padding: 0;
            }}
        </style>
        <script src=""https://maps.googleapis.com/maps/api/js?key=AIzaSyCYW11TqCgGPoGYLtb-GtfsqBDfSjS25Vk""></script>
        <script>
            // Function to initialize the map
            function initMap(latitude, longitude) {{
                const mapOptions = {{
                    center: {{ lat: latitude, lng: longitude }},
                    zoom: 15
                }};
                const map = new google.maps.Map(document.getElementById('map'), mapOptions);

                // Add Traffic Layer
                const trafficLayer = new google.maps.TrafficLayer();
                trafficLayer.setMap(map);

                // Optional: Add a marker at the user's location
                new google.maps.Marker({{
                    position: {{ lat: latitude, lng: longitude }},
                    map: map,
                    title: ""You are here!""
                }});
            }}

            // Use latitude from C#
            const mapLat = {lat};
            const mapLog = {log};

            // Load the map with latitude from C#
            window.onload = () => initMap(mapLat, mapLog);
        </script>
    </head>
    <body>
        <div id=""map""></div>
    </body>
    </html>
    ";
            return traffic;
        }
        //Map
        private async void LoadGoogleMap()
        {
            GoogleMapWebView.Source = new HtmlWebViewSource
            {
                Html = await GetTrafficHtmlAsync()
            };
        }



        private async void OnButtonClicked(object sender, EventArgs e)
 
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

        private async void OnButtonStartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InTripPage(driverInstance));
        }

        private async void OnButtonHomeClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Attention", "Currently on the Home Page.", "OK");
        }

        private async void OnButtonTripClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TripHistoryPage(driverInstance.GetAccountEmail()));
        }

        private async void OnButtonSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage(driverInstance.GetAccountEmail()));
        }
        private async void OnInstructionButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InstructionPage());
        }
    }
}