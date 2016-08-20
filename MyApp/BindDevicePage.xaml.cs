using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Networking;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
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
    public sealed partial class BindDevicePage : Page
    {
        public BindDevicePage()
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await ScanDevice();
            //Debug.WriteLine(Util.GetRoomFromBluetoothRecieve("X0T26.2W1C0K0B0S1"));
            //if (Util.GetTripFromBluetoothRecieve("X0T26.2W1C0K0B0S1"))
            //{
            //    Debug.WriteLine(UploadFunctions.UploadTripInfo("bedroom"));
            //}
        }

        private async void ABB_Refresh_OnClicked(object sender, RoutedEventArgs e)
        {
            await ScanDevice();
        }

        private async void ABB_Accept_OnClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (await ConnectToDevice() == false)
                    return;
                if (await FindService() == false)
                    return;
                if (await FindCharacteristic() == false)
                    return;
                getValue();
            }
            catch (Exception exc)
            {
                Debug.WriteLine("BindDevicePage ABB_Accept_OnClicked: " + exc.Message);
            }

            MessageDialog dialog = new MessageDialog("Bind Device successfully!");
            dialog.Commands.Add(new UICommand("OK", cmd => { }, commandId: 0));
            IUICommand result = await dialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                if (this.Frame.CanGoBack)
                {
                    this.Frame.GoBack();
                }
            }
        }

        DeviceInformationCollection bleDevices;
        BluetoothLEDevice _device;
        IReadOnlyList<GattDeviceService> _services;
        GattDeviceService _service;
        GattCharacteristic _characteristic;

        public string str;
        bool _notificationRegistered;
        private async Task<int> ScanDevice()
        {
            try
            {
                // bleDevices = await DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(GattServiceUuids.GenericAccess));
                bleDevices = await DeviceInformation.FindAllAsync(BluetoothLEDevice.GetDeviceSelector());

                Debug.WriteLine("Found " + bleDevices.Count + " device(s)");

                if (bleDevices.Count == 0)
                {
                    await new MessageDialog("No BLE Devices found - make sure you've paired your device").ShowAsync();
                    await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:", UriKind.RelativeOrAbsolute));
                }
                deviceList.ItemsSource = bleDevices;
            }
            catch (Exception ex)
            {
                //if((uint)ex.HResult == 0x8007048F)
                //{
                //    await new MessageDialog("Bluetooth is turned off!").ShowAsync();
                //}
                await new MessageDialog("Failed to find BLE devices: " + ex.Message).ShowAsync();
            }

            return bleDevices.Count;
        }
        private async Task<bool> ConnectToDevice()
        {
            try
            {
                for (int i = 0; i < bleDevices.Count; i++)
                {
                    if (bleDevices[i].Name == "VICTOR")
                    {
                        _device = await BluetoothLEDevice.FromIdAsync(bleDevices[i].Id);
                        _services = _device.GattServices;
                        Debug.WriteLine("Found Device: " + _device.Name);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog("Connection failed: " + ex.Message).ShowAsync();
                return false;
            }
            await new MessageDialog("Unable to find device VICTOR - has it been paired?").ShowAsync();

            return true;
        }
        private async Task<bool> FindService()
        {
            foreach (GattDeviceService s in _services)
            {
                Debug.WriteLine("Found Service: " + s.Uuid);

                if (s.Uuid == new Guid("0000fff0-0000-1000-8000-00805f9b34fb"))
                {
                    _service = s;
                    return true;
                }
            }
            await new MessageDialog("Unable to find VICTOR Service 0000fff0").ShowAsync();
            return false;
        }
        private async Task<bool> FindCharacteristic()
        {
            foreach (var c in _service.GetCharacteristics(new Guid("0000fff1-0000-1000-8000-00805f9b34fb")))
            {
                //"unauthorized access" without proper permissions
                _characteristic = c;
                Debug.WriteLine("Found characteristic: " + c.Uuid);

                return true;
            }
            //IReadOnlyList<GattCharacteristic> characteristics = _service.GetAllCharacteristics();

            await new MessageDialog("Could not find characteristic or permissions are incorrrect").ShowAsync();
            return false;
        }

        public async void getValue()
        {
            //注册改变
            await RegisterNotificationAsync();
            GattReadResult x = await _characteristic.ReadValueAsync();

            if (x.Status == GattCommunicationStatus.Success)
            {
                str = ParseString(x.Value);
                Debug.WriteLine(str);
            }
        }
        public static string ParseString(IBuffer buffer)
        {
            DataReader reader = DataReader.FromBuffer(buffer);
            return reader.ReadString(buffer.Length);
        }

        private int data_rate = 0;
        private int count_rate = 0;

        private double data_temp = 0;
        private int count_temp = 0;

        private string room = string.Empty;
        private int count_room = 0;

        private int count_trip = 0;

        private int count_sleep = 0;
        private int count_getup = 0;

        List<string> details_sleep = new List<string>();
        List<string> details_lavatory = new List<string>();
        //TimeSpan span_lavatory;
        //DateTime start_lavatory;
        //TimeSpan span_sleep;
        //DateTime start_sleep;

        //bool isSleeped = false;
        bool isInLavatory = false;
        //接受蓝牙数据并校验进行上传
        public async void CharacteristicValueChanged_Handler(GattCharacteristic sender, GattValueChangedEventArgs obj)
        {
            str = ParseString(obj.CharacteristicValue);
            Debug.WriteLine(str);
            if (Util.IsRecieveDataVaild(str))
            {
                //校验心跳数据，如果相同数据收到5次及以上则上传数据
                int rate = Util.GetHeartRateFromBluetoothRecieve(str);
                if (rate != 0)
                {
                    if (data_rate != rate)
                    {
                        data_rate = rate;
                        count_rate = 0;
                    }
                    else
                    {
                        count_rate++;
                        if (count_rate >= 5)
                        {
                            Debug.WriteLine(await UploadFunctions.UploadHeartbeat(rate));
                            count_rate = 0;
                        }
                    }
                }
                //校验体温数据，如果相同数据收到5次及以上则上传数据
                double temp = Util.GetTemperatureFromBluetoothRecieve(str);
                if (temp != 0)
                {
                    if (data_temp != temp)
                    {
                        data_temp = temp;
                        count_temp = 0;
                    }
                    else
                    {
                        count_temp++;
                        if (count_temp >= 5)
                        {
                            Debug.WriteLine(await UploadFunctions.UploadTemperature(temp));
                            count_temp = 0;
                        }
                    }
                }
                //校验房间数据，如果相同数据收到5次及以上，则上传数据
                if (Util.GetRoomFromBluetoothRecieve(str) != null && Util.GetRoomFromBluetoothRecieve(str) != "S")
                {
                    string room_r = Util.GetRoomFromBluetoothRecieve(str);
                    if (room != room_r)
                    {
                        // room = (room == room_r) ? room : room_r;
                        room = room_r;
                        count_room = 0;
                    }
                    else
                    {
                        count_room++;
                        if (count_room >= 35)
                        {
                            Debug.WriteLine(await UploadFunctions.UploadIndoorPosition(room));
                            if (room == "B")
                            {
                                //开始记录如厕次数
                              //  LocalSettingsHelper.SetLavatoryStartTime();
                              if (!isInLavatory)
                                {
                                    Debug.WriteLine(await UploadFunctions.UploadSleepInfo("go to the Lavatory"));
                                }
                                isInLavatory = true;
                            }

                            //string dining = LocalSettingsHelper.GetDailyTime("dining");
                            //string dining_detail = LocalSettingsHelper.GetDailyDetail("dining_detail");
                            //string sleep = LocalSettingsHelper.GetDailyTime("sleep");
                            //string sleep_detail = LocalSettingsHelper.GetDailyDetail("sleep_detail");
                            //string toilet = LocalSettingsHelper.GetDailyTime("toilet");
                            //string toilet_detail = LocalSettingsHelper.GetDailyDetail("toilet_detail");
                            //string parlour = LocalSettingsHelper.GetDailyTime("parlour");
                            //string parlour_detail = LocalSettingsHelper.GetDailyDetail("parlour_detail");
                            //string outdoor = LocalSettingsHelper.GetDailyTime("outdoor");
                            //string outdoor_detail = LocalSettingsHelper.GetDailyDetail("outdoor_detail");

                            ////将如厕时间记录到日常中
                            //LocalSettingsHelper.SetDailyTime("toilet", toiletTime.ToString());
                            //DailyDetail toiletDetail = Util.DataContractJsonDeSerializer<DailyDetail>(toilet_detail);
                            //toiletDetail.time += 1;
                            //if (toiletDetail.time > 3)
                            //{
                            //    toiletDetail.comment = "too many times during the day.   It is the urination phenomenon.  Please pay more attention to him/her.  Strongly recommend you to take him/her to do physical examination in order to prevent the potential severity disease.";
                            //}
                            //toiletDetail.detail.Add(Util.GetTimeFromDateTime(LocalSettingsHelper.GetLavatoryStartTime()) + " - " + Util.GetTimeFromDateTime(DateTime.Now));
                            //string toiletJsonString = Util.ObjectToJsonData(toiletDetail);
                            //LocalSettingsHelper.SetDailyDetail("toilet_detail", toiletJsonString);

                            //上传如厕次数和详情
                            //  Debug.WriteLine(UploadFunctions.UploadDaily(dining, dining_detail, sleep, sleep_detail, LocalSettingsHelper.GetDailyTime("toilet"), toiletJsonString, parlour, parlour_detail, outdoor, outdoor_detail));

                            count_room = 0;
                        }
                        if (Util.GetLavatoryNullFromRecieve(str))
                        {
                            if (isInLavatory)
                            {
                                isInLavatory = false;
                                Debug.WriteLine(await UploadFunctions.UploadIndoorPosition("H"));
                            }
                        }
                        
                        //if (room_r != null && room_r != "B")
                        //{
                        //    if (isInLavatory)
                        //    {
                        //        isInLavatory = false;
                        //        TimeSpan toiletTime = DateTime.Now - LocalSettingsHelper.GetLavatoryStartTime();

                        //        LocalSettingsHelper.KEY_TOILET += toiletTime;
                        //        DailyDetail lavatoryDetail;
                        //        if (LocalSettingsHelper.KEY_TOILET_DETAIL != null && LocalSettingsHelper.KEY_TOILET_DETAIL != "")
                        //        {
                        //            lavatoryDetail = Util.DataContractJsonDeSerializer<DailyDetail>(LocalSettingsHelper.KEY_TOILET_DETAIL);
                        //        }
                        //        else
                        //        {
                        //            lavatoryDetail = new DailyDetail();
                        //        }
                        //        try
                        //        {
                        //            lavatoryDetail.time += 1;
                        //            details_lavatory.Add(Util.GetTimeFromDateTime(LocalSettingsHelper.GetLavatoryStartTime()) + " - " + Util.GetTimeFromDateTime(DateTime.Now));
                        //            lavatoryDetail.detail = details_lavatory;
                        //            if (lavatoryDetail.time > 3)
                        //            {
                        //                lavatoryDetail.comment = "too many times during the day.   It is the urination phenomenon.  Please pay more attention to him/her.  Strongly recommend you to take him/her to do physical examination in order to prevent the potential severity disease.";
                        //            }
                        //            else
                        //            {
                        //                lavatoryDetail.comment = "lavatory condition is normal";
                        //            }

                        //            string lavatoryJsonString = Util.ObjectToJsonData(lavatoryDetail);
                        //            LocalSettingsHelper.KEY_TOILET_DETAIL = lavatoryJsonString;

                        //            //上传睡眠次数和详情
                        //            Debug.WriteLine(UploadFunctions.UploadDaily(LocalSettingsHelper.KEY_DINING, LocalSettingsHelper.KEY_DINING_DETAIL, LocalSettingsHelper.KEY_SLEEP, LocalSettingsHelper.KEY_SLEEP_DETAIL, LocalSettingsHelper.KEY_TOILET, LocalSettingsHelper.KEY_TOILET_DETAIL, LocalSettingsHelper.KEY_PARLOUR, LocalSettingsHelper.KEY_PARLOUR_DETAIL, LocalSettingsHelper.KEY_OUTDOOR, LocalSettingsHelper.KEY_OUTDOOR_DETAIL));
                        //        }
                        //        catch (Exception e)
                        //        {
                        //            Debug.WriteLine("Lavatory : " + e.Message);
                        //        }
                        //   }
                        // }
                    }
                }
                //校验摔倒数据，如果相同数据收到5次及以上，则上传数据
                if (Util.GetTripFromBluetoothRecieve(str))
                {
                    count_trip++;
                    if (count_trip >= 5)
                    {
                        string place = string.Empty;
                        switch (room)
                        {
                            case "W":
                                place = "bedroom";
                                break;
                            case "C":
                                place = "diningroom";
                                break;
                            case "K":
                                place = "parlour";
                                break;
                            case "B":
                                place = "toilet";
                                break;
                        }
                        if (room == string.Empty || room == "")
                        {
                            Debug.WriteLine(await UploadFunctions.UploadTripInfo("outdoor"));
                        }
                        else
                        {
                            Debug.WriteLine(await UploadFunctions.UploadTripInfo(place));
                        }
                        count_trip = 0;
                    }
                }
                else
                {
                    count_trip = 0;
                }
                //校验入睡数据，如果数据收到5次及以上则上传数据
                if (str.Contains("J"))
                {
                    count_sleep++;
                    if (count_sleep >= 10)
                    {
                        count_sleep = 0;
                     //   isSleeped = true;
                        //try
                        //{
                        //    //开始记录睡眠时间
                        //    LocalSettingsHelper.SetStartSleepTime();
                        //}
                        //catch (Exception e)
                        //{
                        //    Debug.WriteLine("RecordSleep: " + e.Message);
                        //}
                        //上传入睡信息
                        Debug.WriteLine(await UploadFunctions.UploadSleepInfo("sleeped"));
                        await UploadFunctions.UploadIndoorPosition("J");
                    }
                }
                else
                {
                    count_sleep = 0;
                }
                //校验起床数据，如果数据收到5次及以上则上传数据
                if (str.Contains("Q"))
                {
                    count_getup++;
                    if (count_getup >= 3)
                    {
                        count_getup = 0;
                        //if (isSleeped)
                        //{
                        //    //开始记录睡眠时间
                        //    if (LocalSettingsHelper.GetSleepStartTime() != DateTime.MinValue)
                        //    {
                        //        TimeSpan sleepTime = DateTime.Now - LocalSettingsHelper.GetSleepStartTime();

                        //        //string dining = LocalSettingsHelper.GetDailyTime("dining");
                        //        //string dining_detail = LocalSettingsHelper.GetDailyDetail("dining_detail");
                        //        //string sleep = LocalSettingsHelper.GetDailyTime("sleep");
                        //        //string sleep_detail = LocalSettingsHelper.GetDailyDetail("sleep_detail");
                        //        //string toilet = LocalSettingsHelper.GetDailyTime("toilet");
                        //        //string toilet_detail = LocalSettingsHelper.GetDailyDetail("toilet_detail");
                        //        //string parlour = LocalSettingsHelper.GetDailyTime("parlour");
                        //        //string parlour_detail = LocalSettingsHelper.GetDailyDetail("parlour_detail");
                        //        //string outdoor = LocalSettingsHelper.GetDailyTime("outdoor");
                        //        //string outdoor_detail = LocalSettingsHelper.GetDailyDetail("outdoor_detail");

                        //        ////将睡觉时间记录到日常中
                        //        //LocalSettingsHelper.SetDailyTime("sleep", sleepTime.ToString());
                        //        LocalSettingsHelper.KEY_SLEEP += sleepTime;
                        //        DailyDetail sleepDetail;
                        //        if (LocalSettingsHelper.KEY_SLEEP_DETAIL != null && LocalSettingsHelper.KEY_SLEEP_DETAIL != "")
                        //        {
                        //            sleepDetail = Util.DataContractJsonDeSerializer<DailyDetail>(LocalSettingsHelper.KEY_SLEEP_DETAIL);
                        //        }
                        //        else
                        //        {
                        //            sleepDetail = new DailyDetail();
                        //        }
                        //        try
                        //        {
                        //            sleepDetail.time += 1;
                        //            details_sleep.Add(Util.GetTimeFromDateTime(LocalSettingsHelper.GetSleepStartTime()) + " - " + Util.GetTimeFromDateTime(DateTime.Now));
                        //            sleepDetail.detail = details_sleep;
                        //            sleepDetail.comment = "Sleeping condition is normal";

                        //            string sleepJsonString = Util.ObjectToJsonData(sleepDetail);
                        //            LocalSettingsHelper.KEY_SLEEP_DETAIL = sleepJsonString;

                                    //LocalSettingsHelper.SetDailyDetail("sleep_detail", sleepJsonString);

                                    //上传睡眠次数和详情
                         //           //Debug.WriteLine(UploadFunctions.UploadDaily(dining, dining_detail, LocalSettingsHelper.GetDailyTime("sleep"), sleepJsonString, toilet, toilet_detail, parlour, parlour_detail, outdoor, outdoor_detail));
                         //           Debug.WriteLine(UploadFunctions.UploadDaily(LocalSettingsHelper.KEY_DINING, LocalSettingsHelper.KEY_DINING_DETAIL, LocalSettingsHelper.KEY_SLEEP, LocalSettingsHelper.KEY_SLEEP_DETAIL, LocalSettingsHelper.KEY_TOILET, LocalSettingsHelper.KEY_TOILET_DETAIL, LocalSettingsHelper.KEY_PARLOUR, LocalSettingsHelper.KEY_PARLOUR_DETAIL, LocalSettingsHelper.KEY_OUTDOOR, LocalSettingsHelper.KEY_OUTDOOR_DETAIL));

                                    //清空睡眠开始时间
                                    // LocalSettingsHelper.ClearStartSleepTime();
                                    //TODO 上传入睡数据
                                    Debug.WriteLine(await UploadFunctions.UploadSleepInfo("get up"));
                                    await UploadFunctions.UploadIndoorPosition("Q");
                                //}
                                //catch (Exception e)
                                //{
                                //    Debug.WriteLine("GetUp: " + e.Message);
                                //}
                            //}
                            //else
                            //{
                            //    Debug.WriteLine("Start time is null!");
                            //}
                            //isSleeped = false;
                       // }

                    }
                }
                else
                {
                    count_getup = 0;
                }
            }
        }
        public async Task RegisterNotificationAsync()
        {
            if (_notificationRegistered)
            {
                return;
            }
            GattCommunicationStatus writeResult;
            if (((_characteristic.CharacteristicProperties & GattCharacteristicProperties.Notify) != 0))
            {

                _characteristic.ValueChanged +=
                    new TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs>(CharacteristicValueChanged_Handler);
                writeResult = await _characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                    GattClientCharacteristicConfigurationDescriptorValue.Notify);
                _notificationRegistered = true;
            }
        }

        private async void ABB_Bluetooth_OnClicked(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
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
