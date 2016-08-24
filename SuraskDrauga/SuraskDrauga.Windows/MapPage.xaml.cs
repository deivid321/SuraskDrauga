using Bing.Maps;
using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SuraskDrauga
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private Navigation navigation;
        private MyAzure myAzure;
        private bool slowView = true;

        public MapPage()
        {
            this.InitializeComponent();
        }

        public MapPage(string name, string guid)
        {
            this.InitializeComponent();
            myAzure = new MyAzure(this);
            map.Culture = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            navigation = new Navigation(this, name, guid);
            UpdateMap();
            //WelcomeMessage("Welcome to Surask Drauga " + name);
        }

        public async void UpdateMap(bool userLocation = false)
        {
            //if (myAzure.GetData(). await myAzure.GetData();
            myAzure.startGetDataTask();
            map.Children.Clear();
            if (userLocation)
            {
                var pin = new Grid()
                {
                    Width = 40,
                    Height = 40,
                    Margin = new Windows.UI.Xaml.Thickness(-12)
                };
                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Red),
                    Width = 40,
                    Height = 40
                });
                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Yellow),
                    //Stroke = new SolidColorBrush(Colors.White),
                    //StrokeThickness = 3,
                    Width = 25,
                    Height = 25
                });
                MapLayer.SetPosition(pin, new Location(navigation.y, navigation.x));
                map.Children.Add(pin);
                if (slowView)
                {
                    map.SetView(new Location(navigation.y, navigation.x), 19, 20, TimeSpan.FromSeconds(4));
                }
            }
            if (myAzure.items != null)
                foreach (var item in myAzure.items)
                {
                    if (!string.Equals(item.Id, navigation.guid))
                    {
                        //MapIcon MapIcon1 = new MapIcon();
                        //MapIcon1.Location = new Geopoint(new BasicGeoposition()
                        //{
                        //    Latitude = 47.620,
                        //    Longitude = -122.349
                        //});
                        //MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                        //MapIcon1.Title = "Space Needle";
                        //MapControl1.MapElements.Add(MapIcon1);
                        var pin = new Grid()
                        {
                            Width = 400,
                            Height = 400,
                            Margin = new Windows.UI.Xaml.Thickness(-12)
                        };
                        pin.Children.Add(new TextBlock()
                        {
                            Text = item.Name,
                            FontSize = 25,
                            Width = 400,
                            Height = 400
                        });
                        MapLayer.SetPosition(pin, new Location(item.Y + 0.00001, item.X+0.0001));
                        map.Children.Add(pin);
                        Pushpin pushpin = new Pushpin();
                        pushpin.Text = item.Name;
                        pushpin.Tapped += Pushpin_Tapped;
                        MapLayer.SetPosition(pushpin, new Location(item.Y, item.X));
                        map.Children.Add(pushpin);
                    }
                }
            //myAzure.StartInsertDataTask();
            await myAzure.InsertData(new TableItem(navigation.guid, navigation.name, navigation.x, navigation.y, DateTime.Now));
        }

        public async void Message(string text)
        {
            await(new MessageDialog(text)).ShowAsync();
        }

        private async void Pushpin_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var pushpin = sender as Pushpin;
            var point = MapLayer.GetPosition(pushpin);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                map.SetView(new Location(point.Latitude, point.Longitude), 19, 20, TimeSpan.FromSeconds(4));
            });
        }

        private void button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateMap(true);
        }

        //private async void GetCoordinates()
        //{
        //    Geolocator geolocator = new Geolocator();
        //    geolocator.DesiredAccuracyInMeters = 50;
        //    Geoposition geoposition = null;
        //    try
        //    {
        //        geoposition = await geolocator.GetGeopositionAsync(
        //             maximumAge: TimeSpan.FromMinutes(5),
        //             timeout: TimeSpan.FromSeconds(10)
        //            );
        //    }
        //    //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
        //    //The second is that the user doesn't turned on the Location Services
        //    catch (Exception ex)
        //    {
        //        //exception
        //    }
        //    y = geoposition.Coordinate.Latitude;
        //    x = geoposition.Coordinate.Longitude;
        //    UpdateMap();
        //}

        public void SetStreetTextBox(Geoposition street)
        {
            //GeoCoordinate geo = new GeoCoordinate(navigation.y, navigation.x);

            //geo.
            //MapService.ServiceToken = "abcdef-abcdefghijklmno";
           // BasicGeoposition basic = new BasicGeoposition();
           //// basic.
           // Geopoint point = new Geopoint(street);

           // MapLocationFinderResult FinderResult =
           // await MapLocationFinder.FindLocationsAtAsync(point);

           // String format = "{0}, {1}, {2}";

           // if (FinderResult.Status == MapLocationFinderStatus.Success)
           // {
           //     var selectedLocation = FinderResult.Locations.First();

           //     string message = String.Format(format, selectedLocation.Address.Town, selectedLocation.Address.District, selectedLocation.Address.Country);
           //     await ShowMessage(message);
           // }
           // streetTextBlock.Text = street;
        }
        
        public void SetStatusTextBox(string text)
        {
            statusTextBlock.Text = text;
        }

        private async void WelcomeMessage(string text)
        {
            await (new MessageDialog(text, "Welcome")).ShowAsync();
        }

        public async void CenterMe()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                map.SetView(new Location(navigation.y, navigation.x), 19, 20, TimeSpan.FromSeconds(4));
            });
        }

        private void centerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CenterMe();
        }

        private void map_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            slowView = false;
        }
    }
}
