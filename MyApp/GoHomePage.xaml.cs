using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GoHomePage : Page
    {
        //检索本地应用数据存储
        private Windows.Storage.ApplicationDataContainer localSettings =
            Windows.Storage.ApplicationData.Current.LocalSettings;
        public const string HOME_POSITION_KEY = "homePosition";
        public const string LONGITUDE_KEY = "longitude";
        public const string LATITUDE_KEY = "latitude";

        public GoHomePage()
        {
            this.InitializeComponent();

            if (App.IsHardwareButtonsAPIPresent)
            {
                btn_back.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_back.Visibility = Visibility.Visible;
            }
        }

        private async void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            disablePanel();
            PopupLoading.IsOpen = true;
            // Specify a known location
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 47.604, Longitude = -122.329 };
            Geopoint cityCenter = new Geopoint(cityPosition);
            map.Center = cityCenter;
            Geopoint pos = await MapHelper.Locate();
            if (pos != null)
            {
                map.Center = pos;
                await UploadFunctions.UploadPosition(pos.Position.Longitude, pos.Position.Latitude);
                MapHelper.AddILocationIcon(map);
                BasicGeoposition startLocation = new BasicGeoposition { Longitude = pos.Position.Longitude, Latitude = pos.Position.Latitude };
                BasicGeoposition endLocation = await GetHomePosition();

                MapHelper.ShowRoute(map, startLocation, endLocation);
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Count not get the location！");
                await dialog.ShowAsync();
            }
            PopupLoading.IsOpen = false;
            enablePanel();
        }

        private void MyMap_LoadingStatuwsChanged(MapControl sender, object args)
        {

        }

        private async void abb_setHome_onClicke(object sender, RoutedEventArgs e)
        {
            disablePanel();
            PopupLoading.IsOpen = true;
            Geopoint pos = await MapHelper.Locate();
            if (pos != null)
            {
                Windows.Storage.ApplicationDataCompositeValue composite = new Windows.Storage.ApplicationDataCompositeValue();
                composite[LONGITUDE_KEY] = pos.Position.Longitude;
                composite[LATITUDE_KEY] = pos.Position.Latitude;

                localSettings.Values[HOME_POSITION_KEY] = composite;
                MessageDialog dialog = new MessageDialog("Set home position succeed！");
                await dialog.ShowAsync();
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Count not get the location！");
                await dialog.ShowAsync();
            }
            PopupLoading.IsOpen = false;
            enablePanel();
        }

        private async System.Threading.Tasks.Task<BasicGeoposition> GetHomePosition()
        {
            // Composite setting
            Windows.Storage.ApplicationDataCompositeValue composite =
               (Windows.Storage.ApplicationDataCompositeValue)localSettings.Values[HOME_POSITION_KEY];
            BasicGeoposition homePosition = new BasicGeoposition();
            if (composite != null)
            {
                homePosition.Longitude = (double)composite[LONGITUDE_KEY];
                homePosition.Latitude = (double)composite[LATITUDE_KEY];
                return homePosition;
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Home position is null!");
                await dialog.ShowAsync();
            }
            return homePosition;
        }

        private void abb_find_onClick(object sender, RoutedEventArgs e)
        {

        }

        private async void abb_locate_onClick(object sender, RoutedEventArgs e)
        {
            disablePanel();
            this.PopupLoading.IsOpen = true;
            Geopoint pos = await MapHelper.Locate();
            if (pos != null)
            {
                map.Center = pos;
                MapHelper.AddILocationIcon(map);
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Count not get the location！");
                await dialog.ShowAsync();
            }
            this.PopupLoading.IsOpen = false;
            enablePanel();
        }

        private void disablePanel()
        {
            abb_locate.IsEnabled = false;
            abb_find.IsEnabled = false;
            abb_setHome.IsEnabled = false;
            map.IsEnabled = false;
        }
        private void enablePanel()
        {
            abb_locate.IsEnabled = true;
            abb_find.IsEnabled = true;
            abb_setHome.IsEnabled = true;
            map.IsEnabled = true;
        }

        private void btn_back_onClicked(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
