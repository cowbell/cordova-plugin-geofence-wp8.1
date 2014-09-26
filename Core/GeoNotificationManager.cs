using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation.Geofencing;

namespace com.cowbell.cordova.geofence.core
{
    public class GeoNotificationManager
    {
        public static void addOrUpdate(GeoNotification geo)
        {
            GeoNotificationManager.remove(geo.Id);
            GeofenceMonitor.Current.Geofences.Add(geo.ToGeofence());
            GeoNotificationStore.set(geo);
        }

        public static void remove(string id)
        {
            var geofence = GeofenceMonitor.Current.Geofences.FirstOrDefault(x => x.Id == id);
            if (geofence != null)
            {
                GeofenceMonitor.Current.Geofences.Remove(geofence);
            }
            GeoNotificationStore.remove(id);
        }

        public static void removeAll()
        {
            GeofenceMonitor.Current.Geofences.Clear();
            GeoNotificationStore.clear();
        }
    }
}
