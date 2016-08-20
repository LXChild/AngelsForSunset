using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyApp
{
    public class Notification : INotifyPropertyChanged
    {
        private Page page;
        public Notification(Page page)
        {
            this.page = page;
        }

        private static PushNotificationChannel channel = null;
        public async Task<string> openNotificationService(string username)
        {
            try
            {
                //申请通道
                channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                string result = null;
                if (channel != null)
                {
                    //将通道url上传至服务器
                    ConnectServer conn = new ConnectServer();
                    //   string username = usernameInput.Text;
                    //将post使用的参数加入字典
                    Dictionary<string, string> dic_param = new Dictionary<string, string>();
                    Debug.WriteLine(channel.Uri);
                    //   string url = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(channel.Uri));
                    string url = Util.UrlEncode(channel.Uri);
                    dic_param.Add("username", username);
                    dic_param.Add("channelurl", url);
                    result = await conn.SendPostRequest(ConnectServer.URL_UPLOADCHANNELURL, dic_param);
                    //添加推送收取事件
                    channel.PushNotificationReceived += NotificationReceived;
                }
                return result;
            } catch (Exception e)
            {
                MessageDialog dialog = new MessageDialog(e.Message);
                await dialog.ShowAsync();
            }
            return null;
        }

        private string message = string.Empty;

        private const double HEARTBEAT_MAX = 100.0;
        private const double HEARTBEAT_MIN = 60.0;
        public static SerialChartDataHelper beatDataHelper = new SerialChartDataHelper(SerialChartDataHelper.FILE_BEAT_NAME);
        private ObservableCollection<SerialChartDataItem> data_serial_heartbeat = new ObservableCollection<SerialChartDataItem>();
        public ObservableCollection<SerialChartDataItem> Data_serial_heartbeat
        {
            get
            {
                return data_serial_heartbeat;
            }

            set
            {
                data_serial_heartbeat = value;
                OnPropertyChanged("Data_serial_heartbeat");
            }
        }
        private string text_beat = "00.00";
        public string Text_beat
        {
            get
            {
                return text_beat;
            }

            set
            {
                text_beat = value;
                OnPropertyChanged("Text_beat");
            }
        }

        private const double TEMPERATURE_MAX = 37.5;
        private const double TEMPERATURE_MIN = 36.0;
        public static SerialChartDataHelper tempDataHelper = new SerialChartDataHelper(SerialChartDataHelper.FILE_TEMP_NAME);
        private ObservableCollection<SerialChartDataItem> data_serial_temperature = new ObservableCollection<SerialChartDataItem>();
        public ObservableCollection<SerialChartDataItem> Data_serial_temperature
        {
            get
            {
                return data_serial_temperature;
            }

            set
            {
                data_serial_temperature = value;
                OnPropertyChanged("Data_serial_temperature");
            }
        }
        private string text_temp = "00.00";
        public string Text_temp
        {
            get
            {
                return text_temp;
            }

            set
            {
                text_temp = value;
                OnPropertyChanged("Text_temp");
            }
        }

        public static bool isReceiveDaily = false;
        public static DailyDataHelper dailyDataHelper = new DailyDataHelper();
        private DateTimeOffset date_DP;
        public DateTimeOffset Date_DP
        {
            get
            {
                return date_DP;
            }

            set
            {
                date_DP = value;
                OnPropertyChanged("Date_DP");
            }
        }
        public static PieChartDataHelper pieChartDataHelper = new PieChartDataHelper();
        private ObservableCollection<PieChartDataItem> data_pie_daily = new ObservableCollection<PieChartDataItem>();
        public ObservableCollection<PieChartDataItem> Data_pie_daily
        {
            get
            {
                return data_pie_daily;
            }

            set
            {
                data_pie_daily = value;
                OnPropertyChanged("Data_pie_daily");
            }
        }
        private string text_dining = "00.00";
        public string Text_dining
        {
            get
            {
                return text_dining;
            }

            set
            {
                text_dining = value;
                OnPropertyChanged("Text_dining");
            }
        }
        private string text_sleep = "00.00";
        public string Text_sleep
        {
            get
            {
                return text_sleep;
            }

            set
            {
                text_sleep = value;
                OnPropertyChanged("Text_sleep");
            }
        }
        private string text_toilet = "00.00";
        public string Text_toilet
        {
            get
            {
                return text_toilet;
            }

            set
            {
                text_toilet = value;
                OnPropertyChanged("Text_toilet");
            }
        }
        private string text_parlour = "00.00";
        public string Text_parlour
        {
            get
            {
                return text_parlour;
            }

            set
            {
                text_parlour = value;
                OnPropertyChanged("Text_parlour");
            }
        }
        private string text_outdoor = "00.00";
        public string Text_outdoor
        {
            get
            {
                return text_outdoor;
            }

            set
            {
                text_outdoor = value;
                OnPropertyChanged("Text_outdoor");
            }
        }

        private static Position position = new Position() { time = "2016/02/16", latitude = 47.604, longitude = -122.329 };
        private static BasicGeoposition initPos = new BasicGeoposition() { Latitude = position.latitude, Longitude = position.longitude };
        private Geopoint cityCenter = new Geopoint(initPos);
        public Geopoint CityCenter
        {
            get
            {
                return cityCenter;
            }

            set
            {
                cityCenter = value;
                OnPropertyChanged("CityCenter");
            }
        }

        private Thickness room = new Thickness(0, 220, 0, 0);
        public Thickness Room
        {
            get
            {
                return room;
            }

            set
            {
                room = value;
                OnPropertyChanged("Room");
            }
        }

        public static EventDataHelper eventDataHelper = new EventDataHelper();
        private ObservableCollection<EventListViewItem> data_list_event = new ObservableCollection<EventListViewItem>();
        public ObservableCollection<EventListViewItem> Data_list_event
        {
            get
            {
                return data_list_event;
            }

            set
            {
                data_list_event = value;
                OnPropertyChanged("Data_list_event");
            }
        }

        private ObservableCollection<RemindEntity> data_list_remind = new ObservableCollection<RemindEntity>();
        public ObservableCollection<RemindEntity> Data_list_remind
        {
            get
            {
                return data_list_remind;
            }

            set
            {
                data_list_remind = value;
                OnPropertyChanged("Data_list_remind");
            }
        }

        public async void NotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            switch (args.NotificationType)
            {
                case PushNotificationType.Badge:
                    BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(args.BadgeNotification);
                    break;
                case PushNotificationType.Raw:
                    string msg = args.RawNotification.Content;
                    message = msg;
                    Debug.WriteLine("raw:" + msg);
                    JsonObject jsonObject = JsonObject.Parse(msg);
                    string type = jsonObject.GetNamedString("type", "");
                    switch (type)
                    {
                        case "heartbeat":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnHeartbeatReceived);
                            break;
                        case "temperature":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnTemperatureReceived);
                            break;
                        case "event":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnEventReceived);
                            break;
                        case "position":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnPositionReceived);
                            break;
                        case "indoorPosition":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnIndoorPositionReceived);
                            break;
                        case "daily":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnDailyReceived);
                            break;
                        case "bindRelation":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnBindRelationReceived);
                            break;
                        case "requestBindRelation":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnRequestBindRelationReceived);
                            break;
                        case "remind":
                            await page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnRemindReceived);
                            break;
                    }
                    break;
                case PushNotificationType.Tile:
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(args.TileNotification);
                    break;
                case PushNotificationType.Toast:
                    ToastNotificationManager.CreateToastNotifier().Show(args.ToastNotification);
                    break;
                default:
                    break;
            }
        }

        private async void OnHeartbeatReceived()
        {
            try
            {
                Heartbeat heartbeat = Util.DataContractJsonDeSerializer<Heartbeat>(message);
                SerialChartDataItem item = new SerialChartDataItem();
                item.time = Util.GetHMFromDatatime(heartbeat.time.ToString());
                item.max = HEARTBEAT_MAX;
                item.min = HEARTBEAT_MIN;
                item.value = heartbeat.beat;
                Text_beat = heartbeat.beat.ToString();
                if (Data_serial_heartbeat.Count > 5)
                {
                    Data_serial_heartbeat.RemoveAt(0);
                    beatDataHelper.RemoveAt(0);

                }
                Data_serial_heartbeat.Add(item);
                beatDataHelper.AddNew(item);
                beatDataHelper.SaveToFile();
            }
            catch (Exception e)
            {
                if (e.Message != "")
                {
                    await new MessageDialog(e.Message).ShowAsync();
                }
            }
        }
        private async void OnTemperatureReceived()
        {
            try
            {
                Temperature temp = Util.DataContractJsonDeSerializer<Temperature>(message);
                SerialChartDataItem item = new SerialChartDataItem();
                item.time = Util.GetHMFromDatatime(temp.time.ToString());
                item.max = TEMPERATURE_MAX;
                item.min = TEMPERATURE_MIN;
                item.value = temp.temperature;
                Text_temp = temp.temperature.ToString();
                if (Data_serial_temperature.Count > 5)
                {
                    Data_serial_temperature.RemoveAt(0);
                    tempDataHelper.RemoveAt(0);
                }
                Data_serial_temperature.Add(item);
                tempDataHelper.AddNew(item);
                tempDataHelper.SaveToFile();
            }
            catch (Exception e)
            {
                if (e.Message != "")
                {
                    await new MessageDialog(e.Message).ShowAsync();
                }
            }
        }
        private void OnEventReceived()
        {
            try
            {
                Event Event = Util.DataContractJsonDeSerializer<Event>(message);
                EventListViewItem item = new EventListViewItem();
                item.title = Event.title;
                item.content = Event.content;
                item.time = Util.GetHMFromDatatime(Event.time.ToString());
                item.uri = new Uri("ms-appx:Assets/event/warning.png");
                Data_list_event.Add(item);
                eventDataHelper.AddNew(item);
            }
           catch (Exception e)
            {
                Debug.WriteLine("Notification OnEventReceived" + e.Message);
            }
        }
        public void OnPositionReceived()
        {
            Position pos = Util.DataContractJsonDeSerializer<Position>(message);
            CityCenter = new Geopoint(new BasicGeoposition()
            {
                Longitude = pos.longitude,
                Latitude = pos.latitude
            });
        }
        private void OnIndoorPositionReceived()
        {
            IndoorPosition indoorPosition = Util.DataContractJsonDeSerializer<IndoorPosition>(message);
            Debug.WriteLine(indoorPosition.room);
            switch (indoorPosition.room)
            {
                //卧室
                case "W":
                    Room = new Thickness(0, 0, 80, 160);
                    break;
                    //厨房
                case "C":
                    Room = new Thickness(48, 90, 0, 0);
                    break;
                    //卫生间
                case "B":
                    Room = new Thickness(180, 0, 0, 220);
                    break;
                //客厅
                case "K":
                    Room = new Thickness(0, 0, 0, 0);
                    break;
                    //户外
                case "H":
                    Room = new Thickness(0, 220, 0, 0);
                    break;
                case "J":
                    Room = new Thickness(0, 0, 260, 130);
                    break;
                //起床
                case "Q":
                    Room = new Thickness(0, 0, 260, 0);
                    break;
            }
        }
        private void OnDailyReceived()
        {
            try
            {
                isReceiveDaily = true;

                Daily daily = Util.DataContractJsonDeSerializer<Daily>(message);
                //将日常数据加入到本地存储中
                dailyDataHelper.AddNew(daily);
                //  dailyDataHelper.SaveToFile();
                LocalSettingsHelper.SetDPDate(Util.StringToDateOffset(daily.date));
                Date_DP = LocalSettingsHelper.GetDPDate();

                PieChartDataItem item_dining = new PieChartDataItem();
                item_dining.title = "dining";
                item_dining.value = daily.dining;
                Text_dining = daily.dining.ToString();

                PieChartDataItem item_sleep = new PieChartDataItem();
                item_sleep.title = "sleep";
                item_sleep.value = daily.sleep;
                Text_sleep = daily.sleep.ToString();

                PieChartDataItem item_parlour = new PieChartDataItem();
                item_parlour.title = "parlour";
                item_parlour.value = daily.parlour;
                Text_parlour = daily.parlour.ToString();

                PieChartDataItem item_toilet = new PieChartDataItem();
                item_toilet.title = "toilet";
                item_toilet.value = daily.toilet;
                Text_toilet = daily.toilet.ToString();

                PieChartDataItem item_outdoor = new PieChartDataItem();
                item_outdoor.title = "outdoor";
                item_outdoor.value = daily.outdoor;
                Text_outdoor = daily.outdoor.ToString();

                if (Data_pie_daily.Count > 0)
                {
                    int count = int.MaxValue;
                    while (count > 0)
                    {
                        try
                        {
                            Data_pie_daily.Clear();
                            pieChartDataHelper.Clear();
                            count = Data_pie_daily.Count;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                   }
                }

                Data_pie_daily.Add(item_dining);
                Data_pie_daily.Add(item_sleep);
                Data_pie_daily.Add(item_toilet);
                Data_pie_daily.Add(item_parlour);
                Data_pie_daily.Add(item_outdoor);

                pieChartDataHelper.AddNew(item_dining);
                pieChartDataHelper.AddNew(item_sleep);
                pieChartDataHelper.AddNew(item_toilet);
                pieChartDataHelper.AddNew(item_parlour);
                pieChartDataHelper.AddNew(item_outdoor);
                pieChartDataHelper.SaveToFile();
            }
            catch (Exception e)
            {
                if (e.Message != "")
                {
                    Debug.WriteLine("OnDailyReceived: " + e.Message);
                }
            }
        }
        private async void OnBindRelationReceived()
        {
            JsonObject jsonObject = JsonObject.Parse(message);
            string result = jsonObject.GetNamedString("result", "");
            MessageDialog dialog = new MessageDialog(result);
            await dialog.ShowAsync();
        }

        string childname = string.Empty;
        private async void OnRequestBindRelationReceived()
        {
            Debug.WriteLine("in OnRequestBindRelation!");
            JsonObject jsonObject = JsonObject.Parse(message);
            childname = jsonObject.GetNamedString("username", "");
            var dialog = new ContentDialog()
            {
                Title = "Tips",
                Content = childname + " want to bind relation with you",
                PrimaryButtonText = "Yes",
                SecondaryButtonText = "No",
                FullSizeDesired = false,
            };

            dialog.PrimaryButtonClick += onYesClicked;
            dialog.SecondaryButtonClick += onNoClicked;
            await dialog.ShowAsync();
        }

        private void OnRemindReceived()
        {
            Remind remind = Util.DataContractJsonDeSerializer<Remind>(message);
            RemindEntity item = new RemindEntity();
            item.MessageType = RemindEntity.MsgType.From;
            item.Content = remind.message;
            Data_list_remind.Add(item);
        }

        private async void onYesClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string username = LocalSettingsHelper.GetUsername();
            if (username != null)
            {
                await UploadFunctions.UploadBindRelationInfo(username, childname, "yes");
            } else
            {
                MessageDialog dialog = new MessageDialog("Null Username!");
                await dialog.ShowAsync();
            }
        }
        private async void onNoClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string username = LocalSettingsHelper.GetUsername();
            if (username != null)
            {
                await UploadFunctions.UploadBindRelationInfo(username, childname, "no");
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Null Username!");
                await dialog.ShowAsync();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            Debug.WriteLine(name + " Changed!");
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void closeNotificationService()
        {
            //关闭通道
            if (channel != null)
            {
                channel.Close();
                channel = null;
            }
        }
    }
}
