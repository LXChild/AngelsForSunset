using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BindRelationPage : Page
    {
        public BindRelationPage()
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

        private async void abb_bind_onClicked(object sender, RoutedEventArgs e)
        {

            string result = await UploadFunctions.UploadRequestBindRelationInfo(usernameInput.Text.Trim());
            System.Diagnostics.Debug.WriteLine(result);

        }

        private void abb_cancel_onClicked(object sender, RoutedEventArgs e)
        {
            //获取Frame对象实例
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null)
            {
                //如果Frame对象可以进行后退操作
                //则应该组织时间传播到系统
                //并调用GoBack方法
                if (rootFrame.CanGoBack)
                {
                    rootFrame.GoBack();
                }
            }
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
