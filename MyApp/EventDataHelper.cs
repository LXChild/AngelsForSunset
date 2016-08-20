using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MyApp
{
    public class EventDataHelper
    {
        //事项列表
        private ObservableCollection<EventListViewItem> _data;

        // 获取事项列表
        public async Task<ObservableCollection<EventListViewItem>> Getdata()
        {
            if (_data == null)
            {
                bool isExist = await LoadFromFile();
                if (!isExist)
                {
                    _data = new ObservableCollection<EventListViewItem>();
                }
            }
            return _data;
        }

        // 添加一条事项记录
        public async void AddNew(EventListViewItem item)
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
        // 读取事项列表
        public async Task<bool> LoadFromFile()
        {
            _data = await StorageFileHelper.ReadAsync<ObservableCollection<EventListViewItem>>("Event.dat");
            return (_data != null);
        }
        //保存事项列表
        //public async void SaveToFile()
        //{
        //    if (_data != null)
        //    {
        //        await StorageFileHelper.WriteAsync(_data, "Event.dat");
        //    }
        //    else
        //    {
        //        MessageDialog dialog = new MessageDialog("Event data is null!");
        //        await dialog.ShowAsync();
        //    }
        //}

        // 移除一条记录
        public void Remove(EventListViewItem item)
        {
            _data.Remove(item);
        }
    }
}
