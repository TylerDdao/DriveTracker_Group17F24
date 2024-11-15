using InTrip;

namespace Project1
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            LoadGoogleMap();
        }

        //Map
        private void LoadGoogleMap()
        {
            String googleMap = @"
            <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <title>Google Maps</title>
                    <script src='https://maps.googleapis.com/maps/api/js?key=AIzaSyCYW11TqCgGPoGYLtb-GtfsqBDfSjS25Vk&callback=initMap' async defer></script>
                    <style>
                        html, body, #map {
                            height: 100%;
                            margin: 0;
                            padding: 0;
                        }
                    </style>
                </head>
                <body>
                    <div id='map'></div>
                    <script>
                        function initMap() {
                            var map = new google.maps.Map(document.getElementById('map'), {
                                center: {lat: 43.4643, lng: -80.5204}, // Coordinates for Waterloo, ON
                                zoom: 15
                            });

                            var marker = new google.maps.Marker({
                                position: {lat: 43.4643, lng: -80.5204}, // Same coordinates for marker
                                map: map,
                                title: 'Waterloo, ON'
                            });
                        }
                    </script>
                </body>
                </html>";            // This should now correctly reference the AzureMapWebView defined in XAML
            String traffic = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title>Google Maps with Traffic and Current Location</title>\r\n    <style>\r\n        html, body, #map {\r\n            height: 100%;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n    </style>\r\n    <script src=\"https://maps.googleapis.com/maps/api/js?key=AIzaSyCYW11TqCgGPoGYLtb-GtfsqBDfSjS25Vk\"></script>\r\n    <script>\r\n        // Function to initialize the map\r\n        function initMap(latitude, longitude) {\r\n            const mapOptions = {\r\n                center: { lat: latitude, lng: longitude },\r\n                zoom: 12\r\n            };\r\n            const map = new google.maps.Map(document.getElementById('map'), mapOptions);\r\n\r\n            // Add Traffic Layer\r\n            const trafficLayer = new google.maps.TrafficLayer();\r\n            trafficLayer.setMap(map);\r\n\r\n            // Optional: Add a marker at the user's location\r\n            new google.maps.Marker({\r\n                position: { lat: latitude, lng: longitude },\r\n                map: map,\r\n                title: \"You are here!\"\r\n            });\r\n        }\r\n\r\n        // Function to get current location\r\n        function getCurrentLocation() {\r\n            if (navigator.geolocation) {\r\n                navigator.geolocation.getCurrentPosition(\r\n                    (position) => {\r\n                        const lat = position.coords.latitude;\r\n                        const lng = position.coords.longitude;\r\n                        initMap(lat, lng); // Initialize map with user's location\r\n                    },\r\n                    (error) => {\r\n                        console.error(\"Error getting location:\", error);\r\n                        // Default location if user denies permission or error occurs\r\n                        initMap(37.7749, -122.4194); // Example: San Francisco\r\n                    }\r\n                );\r\n            } else {\r\n                console.log(\"Geolocation is not supported by this browser.\");\r\n                // Initialize map with a default location if geolocation is unavailable\r\n                initMap(37.7749, -122.4194);\r\n            }\r\n        }\r\n\r\n        // Load the map when the page loads\r\n        window.onload = getCurrentLocation;\r\n    </script>\r\n</head>\r\n<body>\r\n    <div id=\"map\"></div>\r\n</body>\r\n</html>\r\n";
            GoogleMapWebView.Source = new HtmlWebViewSource
            {
                Html = traffic
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
