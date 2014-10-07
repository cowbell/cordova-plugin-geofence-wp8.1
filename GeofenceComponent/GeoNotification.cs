using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;

namespace GeofenceComponent
{
    public sealed class GeoNotification
    {
        public string Id { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationText { get; set; }
        public object Data { get; set; }
        public int TransitionType { get; set; }
        public int Radius { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool OpenAppOnClick { get; set; }

        public Geofence ToGeofence()
        {
            BasicGeoposition position;
            position.Latitude = Latitude;
            position.Longitude = Longitude;
            position.Altitude = 0.0;
            
            // the geofence is a circular region
            Geocircle geocircle = new Geocircle(position, Radius);

            // want to listen for enter geofence, exit geofence and remove geofence events
            // you can select a subset of these event states
            MonitoredGeofenceStates mask = 0;

            if (TransitionType == 1)
            {
                mask |= MonitoredGeofenceStates.Entered;
            }
            else
            {
                mask |= MonitoredGeofenceStates.Exited;
            }
            mask |= MonitoredGeofenceStates.Removed;

            // setting up how long you need to be in geofence for enter event to fire
            TimeSpan dwellTime = TimeSpan.FromSeconds(5);
            TimeSpan duration = TimeSpan.FromDays(365);

            // setting up the start time of the geofence
            DateTimeOffset startTime = DateTimeOffset.Now;
            return new Geofence(Id, geocircle, mask, false, dwellTime, startTime, duration);
        }
    }
}
