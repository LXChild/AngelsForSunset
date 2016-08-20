using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MyApp
{
    public class SerialChartDataHelper
    {
        //折线数据列表
        private ObservableCollection<SerialChartDataItem> data;
        private string name;
        public const string FILE_TEMP_NAME = "Temp.dat";
        public const string FILE_BEAT_NAME = "Beat.dat";

        public SerialChartDataHelper(string name)
        {
            this.name = name;
        }

        // 获取折线数据列表
        public async Task<ObservableCollection<SerialChartDataItem>> Getdata()
        {
            if (this.data == null)
            {
                bool isExist = await LoadFromFile();
                if (!isExist)
                {
                    this.data = new ObservableCollection<SerialChartDataItem>();
                }
            }
            return this.data;
        }
        // 添加一条折线记录
        public async void AddNew(SerialChartDataItem item)
        {
            await Getdata();
            this.data.Add(item);
        }
        // 读取折线数据列表
        public async Task<bool> LoadFromFile()
        {
            try
            {
                data = await StorageFileHelper.ReadAsync<ObservableCollection<SerialChartDataItem>>(name);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return (data != null);
        }
        // 保存折线数据列表
        public async void SaveToFile()
        {
            try
            {
                await StorageFileHelper.WriteAsync(data, name);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }
        // 移除一条记录
        public void RemoveAt(int index)
        {
            data.RemoveAt(index);
        }
    }
}
