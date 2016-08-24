using System;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

namespace SuraskDrauga
{
    class Navigation
    {
        Geolocator geolocator = null;
        public string name;
        public string guid;
        public double x, y, z;
        private MapPage map;
        public string status = "";

        public Navigation(MapPage map, string name, string guid)
        {
            this.map = map;
            this.name = name;
            this.guid = guid;
            geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 0;
            geolocator.MovementThreshold = 5; // The units are meters.
            geolocator.StatusChanged += geolocator_StatusChanged;
        }

        private async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            x = args.Position.Coordinate.Longitude;
            y = args.Position.Coordinate.Latitude;
            z = (double) args.Position.Coordinate.Altitude;
            await map.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                map.UpdateMap(true);
                map.SetStatusTextBox("Source: "+args.Position.Coordinate.PositionSource.ToString()+" Position changed, Accuracy: "+args.Position.Coordinate.Accuracy.ToString());
            });
        }

        private async void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            status = "";
            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    // the application does not have the right capability or the location master switch is off
                    status = "location is disabled in phone settings";
                    break;
                case PositionStatus.Initializing:
                    // the geolocator started the tracking operation
                    status = "initializing";
                    break;
                case PositionStatus.NoData:
                    // the location service was not able to acquire the location
                    status = "no data";
                    break;
                case PositionStatus.Ready:
                    // the location service is generating geopositions as specified by the tracking parameters
                    status = "Tracking...";
                    geolocator.PositionChanged += geolocator_PositionChanged;
                    break;
                case PositionStatus.NotAvailable:
                    status = "not available";
                    // not used in WindowsPhone, Windows desktop uses this value to signal that there is no hardware capable to acquire location information
                    break;
                case PositionStatus.NotInitialized:
                    // the initial state of the geolocator, once the tracking operation is stopped by the user the geolocator moves back to this state
                    break;
            }
            await map.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                map.SetStatusTextBox(status);
            });
        }

    }
}
