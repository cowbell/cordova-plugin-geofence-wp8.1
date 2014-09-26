using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Notifications;
using com.cowbell.cordova.geofence.core;

namespace BackgroundTasks
{
    public sealed class GeofenceTrigger : IBackgroundTask
    {
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
                        var geoNotification = GeoNotificationStore.get(report.Geofence.Id);
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
