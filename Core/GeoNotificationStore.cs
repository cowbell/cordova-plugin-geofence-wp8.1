using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace com.cowbell.cordova.geofence.core
{
    public class GeoNotificationStore
    {
        public static string CONTAINER_NAME = "GeoNotificationStoreContainer";

        public static ApplicationDataContainer Container
        {
            get
            {
                if(!ApplicationData.Current.LocalSettings.Containers.Any(x=>x.Key == CONTAINER_NAME))
                {
                    ApplicationData.Current.LocalSettings.CreateContainer(CONTAINER_NAME, Windows.Storage.ApplicationDataCreateDisposition.Always);
                }
                return ApplicationData.Current.LocalSettings.Containers[CONTAINER_NAME];
            }
        }

        public static void set(GeoNotification geo)
        {
            Container.Values[geo.Id] = JsonConvert.SerializeObject(geo);
        }
        public static GeoNotification get(String id)
        {
            var geoJson = (string) Container.Values[id];
            if (geoJson != null)
            {
                return JsonConvert.DeserializeObject<GeoNotification>(geoJson);
            }
            return null;
        }
        public static IList<GeoNotification> getAll()
        {
            List<GeoNotification> results = new List<GeoNotification>();
            foreach (var geoObj in Container.Values)
            {
                results.Add(JsonConvert.DeserializeObject<GeoNotification>((string)geoObj.Value));
            }
            return results;
        }
        public static void remove(String id)
        {
            Container.Values.Remove(id);
        }
        public static void clear()
        {
            ApplicationData.Current.LocalSettings.DeleteContainer(CONTAINER_NAME);
        }
    }
}
