using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

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

        public MapPage(string name, string guid)
        {
            this.InitializeComponent();
            myAzure = new MyAzure(this);
            map.Language = Windows.Globalization.Language.CurrentInputMethodLanguageTag;
            navigation = new Navigation(this, name, guid);
            UpdateMap();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            UpdateMap(true);
        }

        public async void UpdateMap(bool userLocation = false)
        {
            map.Children.Clear();
            if (userLocation)
            {
                var pin = new Grid()
                {
                    Width = 25,
                    Height = 25,
                    Margin = new Windows.UI.Xaml.Thickness(-12)
                };
                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Red),
                    Width = 25,
                    Height = 25
                });
                pin.Children.Add(new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Yellow),
                    Width = 10,
                    Height = 10
                });
                var basic = new BasicGeoposition();
                basic.Longitude = navigation.x;
                basic.Latitude = navigation.y;
                Windows.UI.Xaml.Shapes.Ellipse fence = new Windows.UI.Xaml.Shapes.Ellipse();
                map.Children.Add(pin);
                MapControl.SetLocation(pin, new Geopoint(basic));
                MapControl.SetNormalizedAnchorPoint(pin, new Point(0.5, 0.5));
                if (slowView)
                {
                    await map.TrySetViewAsync(new Geopoint(basic), 19, 20, 20, MapAnimationKind.Bow);
                }
            }
            //await myAzure.GetData();
            myAzure.startGetDataTask();
            map.MapElements.Clear();
            if (myAzure.items != null)
                foreach (var item in myAzure.items)
                {
                    if (!string.Equals(item.Id, navigation.guid))
                    {
                        MapIcon mapIcon = new MapIcon();
                        ////mapIcon.Image = RandomAccessStreamReference.CreateFromUri(
                        // //new Uri("ms-appx:///Assets/marker.png"));
                        var basic = new BasicGeoposition();
                        basic.Longitude = item.X;
                        basic.Latitude = item.Y;
                        ////basic.Altitude = item.z;
                        var point = new Geopoint(basic);
                        mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
                        mapIcon.Location = point;
                        map.MapElements.Add(mapIcon);

                        var pin = new Grid()
                        {
                            Width = 100,
                            Height = 30,
                            Margin = new Windows.UI.Xaml.Thickness(-12)
                        };
                        pin.Children.Add(new TextBlock()
                        {
                            Text = item.Name,
                            Foreground = new SolidColorBrush(Colors.Black),
                            FontSize = 15,
                            Width = 100,
                            Height = 30
                        });
                        map.Children.Add(pin);
                        MapControl.SetLocation(pin, new Geopoint(basic));
                        MapControl.SetNormalizedAnchorPoint(pin, new Point(0.5, 0.5));
                    }
                }
            await myAzure.InsertData(new TableItem(navigation.guid, navigation.name, navigation.x, navigation.y, DateTime.Now));
        }

        public void SetStatusTextBox(string text)
        {
            //statusTextBlock.Text = text;
        }

        public async void Message(string text)
        {
            await (new MessageDialog(text)).ShowAsync();
        }

        private void map_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            slowView = false;
        }

        //private async void LocateFriends()
        //{
        //    await myAzure.GetData();
        //    if (myAzure.items != null)
        //        foreach (var item in myAzure.items)
        //        {
        //            MapIcon mapIcon = new MapIcon();
        //            //mapIcon.Image = RandomAccessStreamReference.CreateFromUri(
        //            // new Uri("ms-appx:///Assets/PinkPushPin.png"));
        //            var basic = new BasicGeoposition();
        //            basic.Longitude = item.X;
        //            basic.Latitude = item.Y;
        //            //basic.Altitude = item.z;
        //            var point = new Geopoint(basic);
        //            mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
        //            mapIcon.Location = point;
        //            mapIcon.Title = item.Name;
        //            map.MapElements.Add(mapIcon);

        //            //Pushpin pushpin = new Pushpin();
        //            //pushpin.Text = item.Name;
        //            //MapLayer.SetPosition(pushpin, new Location(item.Y, item.X));
        //            //map.Children.Add(pushpin);
        //        }
        //}

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void map_MapDoubleTapped(MapControl sender, MapInputEventArgs args)
        {
            slowView = false;
        }

        private async void centerMeButton_Click(object sender, RoutedEventArgs e)
        {
            var basic = new BasicGeoposition();
            basic.Longitude = navigation.x;
            basic.Latitude = navigation.y;
            await map.TrySetViewAsync(new Geopoint(basic), 19, 20, 20, MapAnimationKind.Bow);
        }

        int style = 0;
        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            switch (style)
            {
                case (int)MapStyle.None:
                    map.Style = MapStyle.None;
                    break;
                case (int)MapStyle.Road:
                    map.Style = MapStyle.Road;
                    break;
                case (int)MapStyle.Aerial:
                    map.Style = MapStyle.Aerial;
                    break;
                case (int)MapStyle.AerialWithRoads:
                    map.Style = MapStyle.AerialWithRoads;
                    break;
                case (int)MapStyle.Terrain:
                    map.Style = MapStyle.Terrain;
                    break;
            }
            style++;
            if (style == 5) style = 0;
        }

        private async void adressButton_Click(object sender, RoutedEventArgs e)
        {
            MapService.ServiceToken = "abcdef-abcdefghijklmno";
            BasicGeoposition basic = new BasicGeoposition();
            basic.Longitude = navigation.x;
            basic.Latitude = navigation.y;
            basic.Altitude = navigation.z;
            Geopoint point = new Geopoint(basic);

            MapLocationFinderResult FinderResult =
            await MapLocationFinder.FindLocationsAtAsync(point);

            String format = "{0}, {1}, {2}";

            if (FinderResult.Status == MapLocationFinderStatus.Success)
            {
                var selectedLocation = FinderResult.Locations.First();

                string message = String.Format(format, selectedLocation.Address.Street, selectedLocation.Address.StreetNumber, selectedLocation.Address.Town, selectedLocation.Address.District, selectedLocation.Address.Country);
                Message(message);
            }
        }
    }
}
