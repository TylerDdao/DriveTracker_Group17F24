using InTrip;
using Project1.Services;
namespace Project1
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            LoadGoogleMap();
        }


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
            // Navigate to SecondPage
            await Navigation.PushAsync(new InTripPage());
        }

        public async Task<bool> RequestLocationPermissionAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status == PermissionStatus.Granted;
        }

    }



}
