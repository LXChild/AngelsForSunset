using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ElderPage : Page
    {
        public ElderPage()
        {
            this.InitializeComponent();
           // NavigationCacheMode = NavigationCacheMode.Enabled;

            if (App.IsHardwareButtonsAPIPresent)
            {
                btn_back.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_back.Visibility = Visibility.Visible;
            }
        }
        private void btn_remind_onClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RemindPage));
        }

        private async void btn_health_onClicked(object sender, RoutedEventArgs e)
        {
            //await UploadFunctions.UploadTemperature(37.8);
            // await UploadFunctions.UploadHeartbeat(34);
            DailyDetail detail_dining = new DailyDetail { time = 3, detail = new List<string> { "07:43 - 08:11", "12:10 - 12:42", "17:06 - 17:34" }, comment = "diet condition is good." };
            DailyDetail detail_sleep = new DailyDetail { time = 1, detail = new List<string> { "22:10 - 8:31" }, comment = "sleeping time is too long, please develop a health living habit." };
            DailyDetail detail_toilet = new DailyDetail { time = 4, detail = new List<string> { "09:43 - 10:21"}, comment = "lavatory condition is abnormal, please pay attention to his/her diet habit, eat more fruit and vegetables." };
            DailyDetail detail_parlour = new DailyDetail { time = 2, detail = new List<string> { "08:40 - 09:38", "14:03 - 15:22" }, comment = "parlour condition is good." };
            DailyDetail detail_outdoor = new DailyDetail { time = 1, detail = new List<string> { "10:30 - 11:10" }, comment = "Outdoor exercise time is too short, Strongly recommend him/her do more exercises after meals." };


            //  Util.ObjectToJsonData(detail_dining)
            string result = await UploadFunctions.UploadDaily(2.7, Util.ObjectToJsonData(detail_dining), 10.2, Util.ObjectToJsonData(detail_sleep), 1.3, Util.ObjectToJsonData(detail_toilet), 8.4, Util.ObjectToJsonData(detail_parlour), 0.7, Util.ObjectToJsonData(detail_outdoor));
           // await UploadFunctions.UploadPosition(108.232591096998, 34.5341);
            //await UploadFunctions.UploadIndoorPosition("C");
            //  System.Diagnostics.Debug.WriteLine("result:" + result);
            this.Frame.Navigate(typeof(HealthPage));
        }

        private async void SOS_OnClicked(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("tel:" + "18940862123"));
        }

        private void BindDevice_OnClicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BindDevicePage));
        }

        private void btn_home_onClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GoHomePage));
        }

        private void btn_exit_clicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
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
