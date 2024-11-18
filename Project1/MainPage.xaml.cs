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
            String traffic = @"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=""utf-8"" />
                <title>Google Maps with Traffic and Current Location</title>
                <style>
                    html, body, #map {
                        height: 100%;
                        margin: 0;
                        padding: 0;
                    }
                </style>
                <script src=""https://maps.googleapis.com/maps/api/js?key=AIzaSyCYW11TqCgGPoGYLtb-GtfsqBDfSjS25Vk""></script>
                <script>
                    // Function to initialize the map
                    function initMap(latitude, longitude) {
                        const mapOptions = {
                            center: { lat: latitude, lng: longitude },
                            zoom: 15
                        };
                        const map = new google.maps.Map(document.getElementById('map'), mapOptions);

                        // Add Traffic Layer
                        const trafficLayer = new google.maps.TrafficLayer();
                        trafficLayer.setMap(map);

                        // Optional: Add a marker at the user's location
                        new google.maps.Marker({
                            position: { lat: latitude, lng: longitude },
                            map: map,
                            title: ""You are here!""
                        });
                    }

                    // Function to get current location
                    function getCurrentLocation() {
                        if (navigator.geolocation) {
                            navigator.geolocation.getCurrentPosition(
                                (position) => {
                                    const lat = position.coords.latitude;
                                    const lng = position.coords.longitude;
                                    initMap(lat, lng); // Initialize map with user's location
                                },
                                (error) => {
                                    console.error(""Error getting location:"", error);
                                    // Default location if user denies permission or error occurs
                                    initMap(43.47958, -80.51768);
                                }
                            );
                        } else {
                            console.log(""Geolocation is not supported by this browser."");
                            // Initialize map with a default location if geolocation is unavailable
                            initMap(43.47958, -80.51768);
                        }
                    }

                    // Load the map when the page loads
                    window.onload = getCurrentLocation;
                </script>
            </head>
            <body>
                <div id=""map""></div>
            </body>
            </html>
            ";
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
