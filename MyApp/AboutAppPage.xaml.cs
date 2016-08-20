using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutAppPage : Page
    {
        public AboutAppPage()
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
            InitInfo();
        }

        private void InitInfo()
        {
            ObservableCollection<AboutListItem> data = new ObservableCollection<AboutListItem>();
            data.Add(new AboutListItem { title = "Team: ", content = "Angle for sunset" });
            data.Add(new AboutListItem { title = "School: ", content = "City Institute DaLian University Of Technology" });
            data.Add(new AboutListItem { title = "Member: ", content = "Shu fang, Lu chun lei, Lu bi ying, Wang chong" });
            data.Add(new AboutListItem { title = "Slogan: ", content = "Nothing is impossible to a willing heart" });
            infoList.ItemsSource = data;
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
