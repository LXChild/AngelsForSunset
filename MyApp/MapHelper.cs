using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Maps;

namespace MyApp
{
    class MapHelper
    {
        public static async Task<Geopoint> Locate()
        {
            Geopoint location = null;
            try
            {
                //Set your current location
                var accessStatus = await Geolocator.RequestAccessAsync();
                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:

                        // Get the current location
                        Geolocator geolocator = new Geolocator();
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        location = pos.Coordinate.Point;
                        break;

                    case GeolocationAccessStatus.Denied:
                        // Handle when access to location is denied
                        System.Diagnostics.Debug.WriteLine("Server Denied!!!");
                        MessageDialog message1 = new MessageDialog("Server Denied!!!");
                        await message1.ShowAsync();
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        // Handle when an unspecified error occurs
                        System.Diagnostics.Debug.WriteLine("Server Unspecified!!!");
                        MessageDialog message2 = new MessageDialog("Server Unspecified!!!");
                        await message2.ShowAsync();
                        break;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return location;
        }

        public static void AddILocationIcon(MapControl map)
        {
            MapIcon mapIcon1 = new MapIcon();
            mapIcon1.Location = map.Center;
            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            // mapIcon1.Title = "MyLocation";
            RandomAccessStreamReference mapIconStreamReference;
            mapIconStreamReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/position/elder.png"));
            mapIcon1.Image = mapIconStreamReference;
            mapIcon1.ZIndex = 0;
            map.MapElements.Add(mapIcon1);
        }

        public static async void ShowRoute(MapControl map, BasicGeoposition startLocation, BasicGeoposition endLocation)
        {
            //// Start at Microsoft in Redmond, Washington.
            //BasicGeoposition startLocation = new BasicGeoposition() { Latitude = 47.643, Longitude = -122.131 };

            //// End at the city of Seattle, Washington.
            //BasicGeoposition endLocation = new BasicGeoposition() { Latitude = 47.604, Longitude = -122.329 };


            // Get the route between the points.
            MapRouteFinderResult routeResult =
                  await MapRouteFinder.GetDrivingRouteAsync(
                  new Geopoint(startLocation),
                  new Geopoint(endLocation),
                  MapRouteOptimization.Time,
                  MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                map.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await map.TrySetViewBoundsAsync(
                      routeResult.Route.BoundingBox,
                      null,
                      MapAnimationKind.None);
            }
        }
    }
}
