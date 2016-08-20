using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Windows.UI.Popups;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Notification notification = null;

        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            disablePanel();
            PopupLoading.IsOpen = true;

            InitInput();
            InitRadioBtn();

            notification = new Notification(this);
            InitPush();
            PopupLoading.IsOpen = false;
            enablePanel();
        }

        private void InitRadioBtn()
        {
            if(LocalSettingsHelper.GetBoolStatus(LocalSettingsHelper.KEY_ROLE))
            {
                rb_elder.IsChecked = true;
            } else
            {
                rb_children.IsChecked = true;
            }
        }

        private async void abb_signin_onClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SaveUserInfo();
            if (rb_elder.IsChecked == true)
            {
                LocalSettingsHelper.SetBoolStatus(LocalSettingsHelper.KEY_ROLE, true);
            }
            else
            {
                LocalSettingsHelper.SetBoolStatus(LocalSettingsHelper.KEY_ROLE, false);
            }
            //Button btn = sender as Button;

            //向布局中添加控件
            //Button jj = new Button();
            //jj.Content = "fasdf";
            //uu.Children.Add(jj);
            disablePanel();
            PopupLoading.IsOpen = true;
            //显示进度条
           // loadingPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            string result = await SignIn();

            if (result.Equals("succeed"))
            {
                LocalSettingsHelper.SetBoolStatus(LocalSettingsHelper.KEY_SIGNIN, true);
                InitPush();
            }
            else
            {
                if(result != "")
                {
                    MessageDialog message = new MessageDialog(result);
                    await message.ShowAsync();
                }
            }
            // loadingPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.PopupLoading.IsOpen = false;
            enablePanel();

            ////构建参数
            //ValueSet p = new ValueSet();
            //p["name"] = account;
            //p["city"] = password;

            //导航到目标页
            //并传递参数
            //  this.Frame.Navigate(typeof(SecondPage), p);
        }

        private async void abb_signup_onClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SaveUserInfo();
            //Button btn = sender as Button;
            disablePanel();
            this.PopupLoading.IsOpen = true;
            //显示进度条
         //   loadingPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            string result = await SignUp();
            if (result.Equals("succeed"))
            {
                LocalSettingsHelper.SetBoolStatus(LocalSettingsHelper.KEY_SIGNIN, true);
            }
            MessageDialog message = new MessageDialog(result);
            await message.ShowAsync();

           // loadingPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            this.PopupLoading.IsOpen = false;
            enablePanel();
        }

        public async Task<string> SignIn()
        {
            ConnectServer conn = new ConnectServer();
            string username = usernameInput.Text;
            string password = passwordInput.Password;
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("username", username);
            dic_param.Add("password", password);

            return await conn.SendPostRequest(ConnectServer.URL_QUERYUSER, dic_param);
        }

        public async Task<string> SignUp()
        {
            ConnectServer conn = new ConnectServer();
            string username = usernameInput.Text;
            string password = passwordInput.Password;
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("username", username);
            dic_param.Add("password", password);

            return await conn.SendPostRequest(ConnectServer.URL_INSERTUSER, dic_param);
        }

        private void SaveUserInfo()
        {
            LocalSettingsHelper.SetAccount(usernameInput.Text, passwordInput.Password);
        }

        private void InitInput()
        {
            Windows.Storage.ApplicationDataCompositeValue composite = LocalSettingsHelper.GetAccount();
            if (composite != null)
            {
                usernameInput.Text = (string)composite[LocalSettingsHelper.KEY_USERNAME];
                passwordInput.Password = (string)composite[LocalSettingsHelper.KEY_PASSWORD];
            }
        }

        private async void InitPush()
        {
            if (LocalSettingsHelper.GetBoolStatus(LocalSettingsHelper.KEY_SIGNIN))
            {
                if (usernameInput.Text.Trim() != "")
                {
                    string result = await notification.openNotificationService(usernameInput.Text);
                    if (result != null && result.Equals("succeed"))
                    {
                        if(LocalSettingsHelper.GetBoolStatus(LocalSettingsHelper.KEY_ROLE))
                        {
                            this.Frame.Navigate(typeof(ElderPage));
                        } else
                        {
                            this.Frame.Navigate(typeof(PivotPage));
                        }
                    }
                }
            }
        }

        private void enablePanel()
        {
            usernameInput.IsEnabled = true;
            passwordInput.IsEnabled = true;
            cb_btm.IsEnabled = true;
            rb_elder.IsEnabled = true;
            rb_children.IsEnabled = true;
            //signInButton.IsEnabled = true;
            //signUpButton.IsEnabled = true;

        }
        private void disablePanel()
        {
            usernameInput.IsEnabled = false;
            passwordInput.IsEnabled = false;
            cb_btm.IsEnabled = false;
            rb_elder.IsEnabled = false;
            rb_children.IsEnabled = false;
            //signInButton.IsEnabled = false;
            //signUpButton.IsEnabled = false;
        }
    }
}
