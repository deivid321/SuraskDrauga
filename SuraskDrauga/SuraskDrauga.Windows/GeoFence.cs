using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace SuraskDrauga
{
    class GeoFence
    {
        Geofence geofence = null;
        private void SetupGeofence()
        {
            //Set up a unique key for the geofence
            string GeofenceKey = "My Home Geofence1";
            BasicGeoposition GeoFenceRootPosition = GetMyHomeLocation();

            double Georadius = 500;

            // the geocircle is the circular region that defines the geofence
            Geocircle geocircle = new Geocircle(GeoFenceRootPosition, Georadius);

            bool singleUse = false;

            //Selecting a subset of the events we need to interact with the geofence
            MonitoredGeofenceStates GeoFenceStates = 0;

            GeoFenceStates |= MonitoredGeofenceStates.Entered;
            GeoFenceStates |= MonitoredGeofenceStates.Exited;

            // setting up how long you need to be in geofence for enter event to fire
            TimeSpan dwellTime;

            dwellTime = TimeSpan.FromSeconds(0);

            // setting up how long the geofence should be active
            TimeSpan GeoFenceDuration;
            GeoFenceDuration = TimeSpan.FromDays(10);

            // setting up the start time of the geofence
            DateTimeOffset GeoFenceStartTime = DateTimeOffset.Now;

            geofence = new Geofence(GeofenceKey, geocircle, GeoFenceStates, singleUse, dwellTime, GeoFenceStartTime, GeoFenceDuration);
            //Add the geofence to the GeofenceMonitor
            GeofenceMonitor.Current.Geofences.Add(geofence);
            //GeofenceMonitor.Current.GeofenceStateChanged +=;
            //x = GeofenceMonitor.Current.LastKnownGeoposition.Coordinate.Longitude;
            //GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;

        }

        private static BasicGeoposition GetMyHomeLocation()
        {
            BasicGeoposition GeoFenceRootPosition;
            GeoFenceRootPosition.Latitude = 23.742766;
            GeoFenceRootPosition.Longitude = 90.417566;
            GeoFenceRootPosition.Altitude = 0.0;
            return GeoFenceRootPosition;
        }
    }
}
