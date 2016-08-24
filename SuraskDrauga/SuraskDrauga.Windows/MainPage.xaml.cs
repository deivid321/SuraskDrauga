using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SuraskDrauga
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Configuration configuration = new Configuration();
        public MainPage()
        {
            this.InitializeComponent();
            configuration.SetGuid();
            nameTextBox.Text = configuration.GetName();
            //Window.Current.Content = new MapPage(nameTextBox.Text, new Configuration().GetGuid());
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            //if (IsTaskRegistered("FirstTask"))
            //{
            //    Unregister("FirstTask");
            //}
            //await AddTask();
            configuration.SetName(nameTextBox.Text);
            Window.Current.Content = new MapPage(nameTextBox.Text, configuration.GetGuid());
            
            // Window.Current.Content = new BigPage();
        }

        //public static bool IsTaskRegistered(string TaskName)
        //{
        //    var Registered = false;
        //    var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(keyval => keyval.Value.Name == TaskName);

        //    if (entry.Value != null)
        //        Registered = true;

        //    return Registered;
        //}

        //public static void Unregister(string TaskName)
        //{
        //    var entry = BackgroundTaskRegistration.AllTasks.FirstOrDefault(keyval => keyval.Value.Name == TaskName);
        //    if (entry.Value != null)
        //        entry.Value.Unregister(true);
        //}

        //private async Task AddTask()
        //{
        //    BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

        //    var geofenceTaskBuilder = new BackgroundTaskBuilder
        //    {
        //        Name = "FirstTask",
        //        TaskEntryPoint = "MyTask.FirstTask"
        //    };
        //    //var trigger = new TimeTrigger(15, true);
        //    var trigger = new LocationTrigger(LocationTriggerType.Geofence);
        //    geofenceTaskBuilder.SetTrigger(trigger);

        //    var geofenceTask = geofenceTaskBuilder.Register();
        //    //geofenceTask.Completed += GeofenceTask_Completed;

        //    switch (backgroundAccessStatus)
        //    {
        //        case BackgroundAccessStatus.Unspecified:
        //        case BackgroundAccessStatus.Denied:
        //            ShowMessage("This application is denied of the background task ");
        //            break;
        //    }
        //}

        //private async void ShowMessage(string message)
        //{
        //    MessageDialog dialog = new MessageDialog(message);
        //    await dialog.ShowAsync();
        //}
    }
}
