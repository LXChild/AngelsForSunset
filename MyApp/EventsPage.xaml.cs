using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EventsPage : Page
    {
        public EventsPage()
        {
            this.InitializeComponent();
            //NavigationCacheMode = NavigationCacheMode.Enabled;
            initEventListViewData();

            if (App.IsHardwareButtonsAPIPresent)
            {
                btn_back.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_back.Visibility = Visibility.Visible;
            }
            eventList.ItemsSource = MainPage.notification.Data_list_event;
        }

        private async void initEventListViewData()
        {
            if(MainPage.notification.Data_list_event.Count > 0)
            {
                MainPage.notification.Data_list_event.Clear();
            }
            // 获取所有事项记录
            IEnumerable<EventListViewItem> allRecords = await Common.GetAllEventRecords();
            foreach (var record in allRecords)
            {
                EventListViewItem data = new EventListViewItem
                {
                    time = record.time,
                    title = record.title,
                    content = record.content,
                    uri = record.uri
                };
                MainPage.notification.Data_list_event.Add(data);
            }
        }

        private void SelectChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach(var item in e.AddedItems)
            {
                System.Diagnostics.Debug.WriteLine((item as EventListViewItem).title + "," + (item as EventListViewItem).content);
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
