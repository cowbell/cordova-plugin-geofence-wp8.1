using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation.Geofencing;

namespace GeofenceComponent
{
    public sealed class GeoNotificationManager
    {
        public static void AddOrUpdate(GeoNotification geo)
        {
            GeoNotificationManager.Remove(geo.Id);
            GeofenceMonitor.Current.Geofences.Add(geo.ToGeofence());
            GeoNotificationStore.Set(geo);
        }

        public static void Remove(string id)
        {
            var geofence = GeofenceMonitor.Current.Geofences.FirstOrDefault(x => x.Id == id);
            if (geofence != null)
            {
                GeofenceMonitor.Current.Geofences.Remove(geofence);
            }
            GeoNotificationStore.Remove(id);
        }

        public static void RemoveAll()
        {
            GeofenceMonitor.Current.Geofences.Clear();
            GeoNotificationStore.Clear();
        }
    }
}
