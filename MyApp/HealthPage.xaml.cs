using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HealthPage : Page
    {
        public HealthPage()
        {
            InitializeComponent();
            if (App.IsHardwareButtonsAPIPresent)
            {
                btn_back.Visibility = Visibility.Collapsed;
            }
            else
            {
                btn_back.Visibility = Visibility.Visible;
            }

            List<HealthListItem> mainItem = new List<HealthListItem>();
            mainItem.Add(new HealthListItem { Date = Util.GetDateFromDatatime(System.DateTime.Now.Date.ToString()), Content = "Heart Rate : Normal" });
            mainItem.Add(new HealthListItem { Date = Util.GetDateFromDatatime(System.DateTime.Now.Date.ToString()), Content = "Temperature : Normal" });
            mainItem.Add(new HealthListItem { Date = Util.GetDateFromDatatime(System.DateTime.Now.Date.ToString()), Content = "Event : Outdoor exercise time is too little." });
            mainItem.Add(new HealthListItem { Date = Util.GetDateFromDatatime(System.DateTime.Now.Date.ToString()), Content = "Suggest : Strongly recommend him/her do more exercises after meals" });

                //mainItem.Add(new HealthListItem { Date = "2015/03/20", Content = "Heart Rate: Normal" });
                //mainItem.Add(new HealthListItem { Date = "2015/03/20", Content = "Temperature: low fever(37.8 ℃)" });
                //mainItem.Add(new HealthListItem { Date = "2015/03/20", Content = "Event: low fever" });
                //mainItem.Add(new HealthListItem { Date = "2015/03/20", Content = "Suggest: Strongly recommend you to do physical examination in order to prevent the potential severity disease." });

                //mainItem.Add(new HealthListItem { Date = "2015/03/19", Content = "Heart Rate: arrhythmia" });
                //mainItem.Add(new HealthListItem { Date = "2015/03/19", Content = "Temperature: Normal" });
                //mainItem.Add(new HealthListItem { Date = "2015/03/19", Content = "Event: Heart rate arrhythmia" });
                //mainItem.Add(new HealthListItem { Date = "2015/03/19", Content = "Suggest: Please relax and keep a regular lifestyle." });

            List<HealthListItemsInGroup> Items = (from item in mainItem group item by item.Date into newItems select new HealthListItemsInGroup { Key = newItems.Key, ItemContent = newItems.ToList() }).ToList();
            itemcollectSource.Source = Items;
            // 分别对两个视图进行绑定 
            outView.ItemsSource = itemcollectSource.View.CollectionGroups;
            inView.ItemsSource = itemcollectSource.View;
        }

        private void btn_back_onClicked(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        //private async void OnSelectChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    foreach (var item in e.AddedItems)
        //    {
        //        switch ((item as HealthListItem).Content)
        //        {
        //            case "Heart Rate : Normal":
        //                await new MessageDialog("\t\tHeart Rate \n\tMax : 71 Time : 14:26\n\tMin : 62 Time : 9:21\n\tComment : Normal").ShowAsync();
        //                break;
        //            case "Temperature: Normal":
        //                await new MessageDialog("\t\tTemperature \n\tMax : 37.1 Time : 15:48\n\tMin : 36.2 Time : 03:21\n\tComment : Normal").ShowAsync();
        //                break;
        //            case "Event: None":
        //                await new MessageDialog("\t\tEvent \n\tNone").ShowAsync();
        //                break;
        //            case "Suggest: None":
        //                await new MessageDialog("\t\tSuggest \n\tNone").ShowAsync();
        //                break;
        //        }
        //    }
        //}
    }
}
