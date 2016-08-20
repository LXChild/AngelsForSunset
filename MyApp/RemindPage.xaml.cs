using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RemindPage : Page
    {
        public RemindPage()
        {
            this.InitializeComponent();
           // NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: 准备此处显示的页面。
            if (App.IsHardwareButtonsAPIPresent)
            {
                btn_back.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_back.Visibility = Visibility.Visible;
            }

            try
            {
                lvMsgs.ItemsSource = MainPage.notification.Data_list_remind;
            }
            catch (Exception exc)
            {
                MessageDialog dialog = new MessageDialog("Remind initialize failed!\n" + exc.Message);
                await dialog.ShowAsync();
            }

            //lvMsgs.ItemsSource = new RemindMessageEntity[]
            //{
            //    new RemindMessageEntity { MessageType = RemindMessageEntity.MsgType.From, Content = "小明，你今天在家吗?" },
            //    new RemindMessageEntity { MessageType = RemindMessageEntity.MsgType.To, Content = "在啊。" },
            //    new RemindMessageEntity { MessageType = RemindMessageEntity.MsgType.From, Content = "待会儿帮你把东西送过去。"},
            //    new RemindMessageEntity { MessageType = RemindMessageEntity.MsgType.To, Content = "好的，十分感谢。" }
            //};
        }

        private async void btn_send_onClicked(object sender, RoutedEventArgs e)
        {
            input_message.IsEnabled = false;
            btn_send.IsEnabled = false;
            btn_send.Content = "Sending";
            if (input_message.Text.Trim() != "")
            {
                string result = await UploadFunctions.UploadRemind(input_message.Text.Trim());
                System.Diagnostics.Debug.WriteLine(result);
                if(result != "")
                {
                    await new MessageDialog(result).ShowAsync();
                }
                RemindEntity remind = new RemindEntity() { MessageType = RemindEntity.MsgType.To, Content = input_message.Text.Trim() };
                MainPage.notification.Data_list_remind.Add(remind);
                input_message.Text = "";
            }
            input_message.IsEnabled = true;
            btn_send.IsEnabled = true;
            btn_send.Content = "Send";
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
