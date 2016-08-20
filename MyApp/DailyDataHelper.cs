using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MyApp
{
    public class DailyDataHelper
    {
        //日常列表
        private List<Daily> _data;

        // 获取日常列表
        public async Task<List<Daily>> Getdata()
        {
            if (_data == null)
            {
                bool isExist = await LoadFromFile();
                if (!isExist)
                {
                    _data = new List<Daily>();
                }
            }
            return _data;
        }

        // 添加一条日常记录
        public async void AddNew(Daily item)
        {
            await Getdata();
            _data.Add(item);
            if (_data != null)
            {
                await StorageFileHelper.WriteAsync(_data, "Daily.dat");
            }
            else
            {
                MessageDialog dialog = new MessageDialog("data is null!");
                await dialog.ShowAsync();
            }
        }
        // 读取日常列表
        public async Task<bool> LoadFromFile()
        {
            _data = await StorageFileHelper.ReadAsync<List<Daily>>("Daily.dat");
            return (_data != null);
        }
        //保存日常列表
        //public async void SaveToFile()
        //{
        //    if (_data != null)
        //    {
        //        await StorageFileHelper.WriteAsync(_data, "Daily.dat");
        //    }
        //    else
        //    {
        //        MessageDialog dialog = new MessageDialog("data is null!");
        //        await dialog.ShowAsync();
        //    }
        //}

        // 移除一条记录
        public void Remove(Daily item)
        {
            _data.Remove(item);
        }
    }
}
