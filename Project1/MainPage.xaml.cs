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
                </html>";
            // This should now correctly reference the AzureMapWebView defined in XAML
            GoogleMapWebView.Source = new HtmlWebViewSource
            {
                Html = googleMap
            };
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            // Navigate to SecondPage
            await Navigation.PushAsync(new InTripPage());
        }

    }

}
