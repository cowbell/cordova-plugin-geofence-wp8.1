using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;
using System.Diagnostics;

namespace GeofenceComponent
{
    public sealed class GeofenceTrigger : IBackgroundTask
    {
        public static async void Register()
        {
            Debug.WriteLine("Geofence plugin - initalize");
            Debug.WriteLine(GeofenceMonitor.Current.Geofences.Count);

            // Get permission for a background task from the user. If the user has already answered once,
            // this does nothing and the user must manually update their preference via PC Settings.
            BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            // Regardless of the answer, register the background task. If the user later adds this application
            // to the lock screen, the background task will be ready to run.
            // Create a new background task builder
            BackgroundTaskBuilder geofenceTaskBuilder = new BackgroundTaskBuilder();

            geofenceTaskBuilder.Name = "GeofenceTrigger";
            geofenceTaskBuilder.TaskEntryPoint = typeof(GeofenceComponent.GeofenceTrigger).ToString();

            // Create a new location trigger
            var trigger = new LocationTrigger(LocationTriggerType.Geofence);

            // Associate the locationi trigger with the background task builder
            geofenceTaskBuilder.SetTrigger(trigger);

            // If it is important that there is user presence and/or
            // internet connection when OnCompleted is called
            // the following could be called before calling Register()
            // SystemCondition condition = new SystemCondition(SystemConditionType.UserPresent | SystemConditionType.InternetAvailable);
            // geofenceTaskBuilder.AddCondition(condition);

            // Register the background task
            var geofenceTask = geofenceTaskBuilder.Register();

            switch (backgroundAccessStatus)
            {
                case BackgroundAccessStatus.Unspecified:
                case BackgroundAccessStatus.Denied:
                    //rootPage.NotifyUser("This application must be added to the lock screen before the background task will run.", NotifyType.ErrorMessage);
                    break;
            }
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();

            try
            {
                IReadOnlyList<GeofenceStateChangeReport> reports =
                  GeofenceMonitor.Current.ReadReports();
                
                // check reports for our geofence and if it's exited replace it with a new one.
                // slight problem here if this fails because it'll mean that we won't get
                // invoked again in the future and so we'll fail and have little chance of 
                // repair :(
                if (reports != null)
                {
                    foreach (var report in reports)
                    {
                        var geoNotification = GeoNotificationStore.Get(report.Geofence.Id);
                        if (geoNotification != null)
                        {
                            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                            var toastText = toastXml.GetElementsByTagName("text");
                            (toastText[0] as XmlElement).InnerText = geoNotification.NotificationTitle;
                            (toastText[1] as XmlElement).InnerText = geoNotification.NotificationText;
                            var toast = new ToastNotification(toastXml);
                            toastNotifier.Show(toast);
                        }
                    }
                }
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
