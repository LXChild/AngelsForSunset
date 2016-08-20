using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MyApp
{
    public class PieChartDataHelper
    {
        //饼图数据列表
        private ObservableCollection<PieChartDataItem> data;
        public const string FILE_PIE_NAME = "Pie.dat";

        // 获取饼图数据列表
        public async Task<ObservableCollection<PieChartDataItem>> Getdata()
        {
            if (this.data == null)
            {
                bool isExist = await LoadFromFile();
                if (!isExist)
                {
                    this.data = new ObservableCollection<PieChartDataItem>();
                }
            }
            return this.data;
        }
        // 添加一条饼图记录
        public async void AddNew(PieChartDataItem item)
        {
            await Getdata();
            this.data.Add(item);
        }
        // 读取饼图数据列表
        public async Task<bool> LoadFromFile()
        {
            try
            {
                data = await StorageFileHelper.ReadAsync<ObservableCollection<PieChartDataItem>>(FILE_PIE_NAME);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return (data != null);
        }
        // 保存饼图数据列表
        public async void SaveToFile()
        {
            try
            {
                await StorageFileHelper.WriteAsync(data, FILE_PIE_NAME);
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
        // 清空记录
        public void Clear()
        {
            if (data != null)
            {
                data.Clear();
            }
        }
    }
}
