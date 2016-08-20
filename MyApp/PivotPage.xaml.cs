using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Data.Json;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace MyApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PivotPage : Page
    {
        public PivotPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            initBeatSerialChartData();
            initTempSerialChartData();
            initDailyPieChartData();
            this.DataContext = MainPage.notification;

            //TimeSpan span = new DateTime(2016, 08, 23, 23, 20, 12) - new DateTime(2016, 08, 24, 1, 20, 12);
            //Debug.WriteLine(span);
        }

        private async void initBeatSerialChartData()
        {
            try
            {
                // 获取所有心跳记录
                IEnumerable<SerialChartDataItem> allRecords = await Common.GetBeatRecords();
                foreach (var record in allRecords)
                {
                    SerialChartDataItem data = new SerialChartDataItem
                    {
                        time = record.time,
                        max = record.max,
                        min = record.min,
                        value = record.value
                    };
                    MainPage.notification.Data_serial_heartbeat.Add(data);
                    MainPage.notification.Text_beat = record.value + "";
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }
        private async void initTempSerialChartData()
        {
            // 获取所有体温记录
            IEnumerable<SerialChartDataItem> allRecords = await Common.GetTempRecords();
            foreach (var record in allRecords)
            {
                SerialChartDataItem data = new SerialChartDataItem
                {
                    time = record.time,
                    max = record.max,
                    min = record.min,
                    value = record.value
                };
                MainPage.notification.Data_serial_temperature.Add(data);
                MainPage.notification.Text_temp = record.value + "";
            }
        }
        private async void initDailyPieChartData()
        {
            try
            {
                //设置日期控件日期
                MainPage.notification.Date_DP = LocalSettingsHelper.GetDPDate();

                // 获取饼图记录
                IEnumerable<PieChartDataItem> allRecords = await Common.GetPieRecords();
                if (allRecords != null)
                {
                    foreach (var record in allRecords)
                    {
                        PieChartDataItem data = new PieChartDataItem
                        {
                            title = record.title,
                            value = record.value
                        };
                        MainPage.notification.Data_pie_daily.Add(data);
                        switch (record.title)
                        {
                            case "dining":
                                MainPage.notification.Text_dining = record.value.ToString();
                                break;
                            case "sleep":
                                MainPage.notification.Text_sleep = record.value.ToString();
                                break;
                            case "toilet":
                                MainPage.notification.Text_toilet = record.value.ToString();
                                break;
                            case "parlour":
                                MainPage.notification.Text_parlour = record.value.ToString();
                                break;
                            case "outdoor":
                                MainPage.notification.Text_outdoor = record.value.ToString();
                                break;
                        }
                    }
                }
            } catch (Exception e)
            {
                Debug.WriteLine("InitDaily: " + e.Message);
            }
        }

        //pivot3 daily
        private void tb_dining_OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowDetail("detail_dining");
        }
        private void tb_toilet_onTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowDetail("detail_toilet");
        }
        private void tb_sleep_onTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowDetail("detail_sleep");
        }
        private void tb_parlour_OnTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowDetail("detail_parlour");
        }
        private void tb_outdoor_onTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowDetail("detail_outdoor");
        }

        private async void ShowDetail(string item)
        {
            string message = string.Empty;
            IEnumerable<Daily> allRecords = null;
            try
            {
                allRecords = await Common.GetThisDayDailyRecords(dp_date.Date.ToString("yyyy/MM/dd"));
            } catch(Exception e)
            {
                Debug.WriteLine("PivotPage ShowDetail: " + e.Message);
            }
            if (allRecords != null)
            {
                foreach (var record in allRecords)
                {
                    switch (item)
                    {
                        case "detail_dining":
                            //message = "Diet condition \n\n" + "time :\t" + "3" + "\ndetial :";
                            //message += "\t" + "7:43 - 8:11" + "\n" + "\t" + "12:10 - 12:42" + "\n" + "\t" + "17:06 - 17:34" + "\n\n" + "Comment：diet condition is good.";
                            DailyDetail detail_dining = Util.DataContractJsonDeSerializer<DailyDetail>(record.detail_dining);
                            message = "Diet condition\n\ntime :\t" + detail_dining.time + "\ndetail :";
                            foreach (var data in detail_dining.detail)
                            {
                                message += "\t" + data + "\n";
                            }
                            message += "\nComment : " + detail_dining.comment;
                            break;
                        case "detail_sleep":
                            //message = "Sleeping condition \n\n" + "detial :";
                            //message += "\t" + "22:10 - 6:31\n\n" + "Comment：sleeping condition is good.";

                            DailyDetail detail_sleep = Util.DataContractJsonDeSerializer<DailyDetail>(record.detail_sleep);
                            message = "Sleeping condition\n\ntime :\t" + detail_sleep.time + "\ndetail :";
                            foreach (var data in detail_sleep.detail)
                            {
                                message += "\t" + data + "\n";
                            }
                            message += "\nComment : " + detail_sleep.comment;
                            break;
                        case "detail_toilet":
                            //message = "Lavatory condition \n\ntime :\t" + "6";
                            //message += "\n\n" + "Comment：lavatory condition is normal.";

                            DailyDetail detail_lavatory = Util.DataContractJsonDeSerializer<DailyDetail>(record.detail_toilet);
                            message = "Lavatory condition\n\ntime :\t" + detail_lavatory.time + "\ndetail :";
                            foreach (var data in detail_lavatory.detail)
                            {
                                message += "\t" + data + "\n";
                            }
                            message += "\nComment : " + detail_lavatory.comment;
                            break;
                        case "detail_parlour":
                            DailyDetail detail_parlour = Util.DataContractJsonDeSerializer<DailyDetail>(record.detail_parlour);
                            message = "Parlour condition\n\ntime :\t" + detail_parlour.time + "\ndetail :";
                            foreach (var data in detail_parlour.detail)
                            {
                                message += "\t" + data + "\n";
                            }
                            message += "\nComment : " + detail_parlour.comment;
                            break;
                        case "detail_outdoor":
                            //message = "Outdoor condition \n\n" + "time :\t" + "2" + "\ndetial :";
                            //message += "\t" + "9:22-10:10" + "\n" + "\t" + "15:20-16:18" + "\n\n" + "Comment：Strongly recommend him/her do more exercises after meals";
                            DailyDetail detail_outdoor = Util.DataContractJsonDeSerializer<DailyDetail>(record.detail_outdoor);
                            message = "Outdoor condition\n\ntime :\t" + detail_outdoor.time + "\ndetail :";
                            foreach (var data in detail_outdoor.detail)
                            {
                                message += "\t" + data + "\n";
                            }
                            message += "\nComment : " + detail_outdoor.comment;
                            break;
                    }
                }
                if (message.Trim() != "")
                {
                    //使用语音播报
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    SpeechSynthesisStream stream = await synthesizer.SynthesizeTextToStreamAsync(message);
                    me.SetSource(stream, stream.ContentType);
                    me.AutoPlay = true;

                    MessageDialog dialog = new MessageDialog(message);
                    await dialog.ShowAsync();
                    message = "";
                    me.AutoPlay = false;
                }
            }
        }
        private async void DP_onDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            try
            {
                bool isNull = true;
                if (dp_date.Date != DateTime.Now.Date)
                {
                    IEnumerable<Daily> allRecords = await Common.GetThisDayDailyRecords(dp_date.Date.ToString("yyyy/MM/dd"));
                    if (allRecords != null)
                    {
                        foreach (var record in allRecords)
                        {
                            isNull = false;
                            if (MainPage.notification.Data_pie_daily != null && MainPage.notification.Data_pie_daily.Count > 0)
                            {
                                int count = int.MaxValue;
                                while (count > 0)
                                {
                                    try
                                    {
                                        MainPage.notification.Data_pie_daily.Clear();
                                        count = MainPage.notification.Data_pie_daily.Count;
                                    }
                                    catch (Exception exc)
                                    {
                                        Debug.WriteLine(exc.Message);
                                    }
                                }
                            }
                            PieChartDataItem item_dining = new PieChartDataItem();
                            item_dining.title = "dining";
                            item_dining.value = record.dining;
                            MainPage.notification.Data_pie_daily.Add(item_dining);
                            MainPage.notification.Text_dining = record.dining.ToString();

                            PieChartDataItem item_sleep = new PieChartDataItem();
                            item_sleep.title = "sleep";
                            item_sleep.value = record.sleep;
                            MainPage.notification.Data_pie_daily.Add(item_sleep);
                            MainPage.notification.Text_sleep = record.sleep.ToString();

                            PieChartDataItem item_toilet = new PieChartDataItem();
                            item_toilet.title = "toilet";
                            item_toilet.value = record.toilet;
                            MainPage.notification.Data_pie_daily.Add(item_toilet);
                            MainPage.notification.Text_toilet = record.toilet.ToString();

                            PieChartDataItem item_parlour = new PieChartDataItem();
                            item_parlour.title = "parlour";
                            item_parlour.value = record.parlour;
                            MainPage.notification.Data_pie_daily.Add(item_parlour);
                            MainPage.notification.Text_parlour = record.parlour.ToString();

                            PieChartDataItem item_outdoor = new PieChartDataItem();
                            item_outdoor.title = "outdoor";
                            item_outdoor.value = record.outdoor;
                            MainPage.notification.Data_pie_daily.Add(item_outdoor);
                            MainPage.notification.Text_outdoor = record.outdoor.ToString();
                        }
                    }

                }
                if (isNull)
                {
                    dp_date.Date = LocalSettingsHelper.GetDPDate();
                    string message = "No records in the day";
                    MessageDialog dialog = new MessageDialog(message);
                    await dialog.ShowAsync();
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine("PivotPage DP_Changed: " + exc.Message);
            }
        }

        //pivot4 position
        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            // Specify a known location
            //BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 47.604, Longitude = -122.329 };
            //Geopoint cityCenter = new Geopoint(cityPosition);

            //Binding posBind_out = new Binding();
            //posBind_out.Source = MainPage.notification;
            //posBind_out.Path = new PropertyPath("CityCenter");
            //posBind_out.Mode = BindingMode.TwoWay;
            //map.SetBinding(MapControl.CenterProperty, posBind_out);
            // Set map location
            MapHelper.AddILocationIcon(map);
        }

        private void Map_CenterChanged(MapControl sender, object args)
        {
            MapHelper.AddILocationIcon(map);
        }

        private void AddIndoor()
        {
            //Binding posBind_in = new Binding();
            //posBind_in.Source = MainPage.notification;
            //posBind_in.Path = new PropertyPath("Room");
            //posBind_in.Mode = BindingMode.TwoWay;
            //img_in_pos.SetBinding(MarginProperty, posBind_in);
            //餐桌坐标
            //img.Margin = new Thickness(48, 90, 0, 0);
            //客厅坐标
            //img.Margin = new Thickness(0, 0, 80, 160);
            //卧室
            //img.Margin = new Thickness(0, 0, 260, 160);
            //厕所
            //img.Margin = new Thickness(180, 0, 0, 250);
            //户外
            //img.Margin = new Thickness(0, 220, 0, 0);
        }

        private void MyMap_LoadingStatusChanged(MapControl sender, object args)
        {
            if (sender != null && (sender.LoadingStatus).Equals(MapLoadingStatus.Loaded))
            {
                //ShowRoute();
                // Debug.Output("Status Changed！{}" + sender.LoadingStatus);
            }
        }

        //pivot5 settings
        private string UserName = LocalSettingsHelper.GetUsername();
        ObservableCollection<SettingsListViewItem> m_items2 = null;
        private void SettingsPivot_Loaded(object sender, RoutedEventArgs e)
        {
            m_items2 = new ObservableCollection<SettingsListViewItem>();
            settingsList.ItemsSource = m_items2;
            // 添加项列表
            m_items2.Add(new SettingsListViewItem { Text = "Remind", Uri = new Uri("ms-appx:Assets/settings/remind.png") });
            m_items2.Add(new SettingsListViewItem { Text = "BindRelation", Uri = new Uri("ms-appx:Assets/settings/bind.png") });
            m_items2.Add(new SettingsListViewItem { Text = "Events", Uri = new Uri("ms-appx:Assets/settings/event.png") });
            m_items2.Add(new SettingsListViewItem { Text = "Settings", Uri = new Uri("ms-appx:Assets/settings/settings.png") });
            m_items2.Add(new SettingsListViewItem { Text = "AboutApp", Uri = new Uri("ms-appx:Assets/settings/about.png") });
        }

        private void SettingsPivot_Unloaded(object sender, RoutedEventArgs e)
        {
            if (m_items2 != null)
            {
                m_items2.Clear();
            }
        }

        private void Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                switch ((item as SettingsListViewItem).Text)
                {
                    case "Remind":
                        this.Frame.Navigate(typeof(RemindPage));
                        break;
                    case "Events":
                        this.Frame.Navigate(typeof(EventsPage));
                        break;
                    case "BindRelation":
                        this.Frame.Navigate(typeof(BindRelationPage));
                        break;
                    case "AboutApp":
                        this.Frame.Navigate(typeof(AboutAppPage));
                        break;
                }
            }
        }

        private void LogOutBtn_Clicked(object sender, RoutedEventArgs e)
        {
            LocalSettingsHelper.SetBoolStatus(LocalSettingsHelper.KEY_SIGNIN, false);
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
