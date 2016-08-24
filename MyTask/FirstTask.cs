using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using System.Threading;
using Windows.Devices.Geolocation.Geofencing;

namespace MyTask
{
    public sealed class FirstTask : IBackgroundTask
    {
        private string guid;
        private double x=10, y=5;

        public void Run(IBackgroundTaskInstance taskInstance)
            {
            // simple example with a Toast, to enable this go to manifest file
            // and mark App as TastCapable - it won't work without this
            // The Task will start but there will be no Toast.
            guid = GetGuid();
            var Reports = GeofenceMonitor.Current.ReadReports();
            var SelectedReport =
                Reports.FirstOrDefault(report => (report.Geofence.Id == "My Home Geofence"));// && (report.NewState == GeofenceState.Entered || report.NewState == GeofenceState.Exited));
            x = SelectedReport.Geoposition.Coordinate.Latitude;
            //x = GeofenceMonitor.Current.LastKnownGeoposition.Coordinate.Longitude;
            //await GetCoordinates();
            //Geolocator geolocator = new Geolocator();
            //CancellationTokenSource CancellationTokenSrc = new CancellationTokenSource();
            //CancellationToken token = CancellationTokenSrc.Token;
            //var position = await geolocator.GetGeopositionAsync(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)).AsTask(token);
            //x = position.Coordinate.Longitude;
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList textElements = toastXml.GetElementsByTagName("text");
            textElements[0].AppendChild(toastXml.CreateTextNode(y.ToString()));
            textElements[1].AppendChild(toastXml.CreateTextNode("exit "+ x.ToString()+" I'm wergwerg message from your background task!"));
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));
            }

        private async Task GetCoordinates()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            Geoposition geoposition = null;
            try
            {
                geoposition = await geolocator.GetGeopositionAsync(
                     maximumAge: TimeSpan.FromMinutes(5),
                     timeout: TimeSpan.FromSeconds(10)
                    );
            }
            //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
            //The second is that the user doesn't turned on the Location Services
            catch (Exception ex)
            {
                //exception
            }
            x = geoposition.Coordinate.Latitude;
            y = geoposition.Coordinate.Longitude;
        }

        private string GetGuid()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object value = localSettings.Values["GUID"];
            if (value != null)
            {
                return (string)localSettings.Values["GUID"];
            }
            return "undefined";
        }


    }
}
