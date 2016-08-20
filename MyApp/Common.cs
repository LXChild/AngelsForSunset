using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    class Common
    {
        public static async Task<IEnumerable<SerialChartDataItem>> GetBeatRecords()
        {
            return (from c in await Notification.beatDataHelper.Getdata() select c);
        }
        public static async Task<IEnumerable<SerialChartDataItem>> GetTempRecords()
        {
            return await Notification.tempDataHelper.Getdata();
        }
        public static async Task<IEnumerable<PieChartDataItem>> GetPieRecords()
        {
            return await Notification.pieChartDataHelper.Getdata();
        }
        public static async Task<IEnumerable<Daily>> GetThisDayDailyRecords(string date)
        {
            date = Util.GetDateFromDatatime(date);
            System.Diagnostics.Debug.WriteLine(date);
            IEnumerable<Daily> data = null;
            try
            {
                data = (from c in await Notification.dailyDataHelper.Getdata()
                                           where (c.date == date)
                                           select c);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Common GetThisDayDailyRecords" + e.Message);
            }
       
            return data;
        }
        public static async Task<IEnumerable<EventListViewItem>> GetAllEventRecords()
        {
            return (from c in await Notification.eventDataHelper.Getdata() select c);
        }
        public static async Task<IEnumerable<EventListViewItem>> GetThisDayEventRecords(string date)
        {
            string time = Util.GetDateFromDatatime(date);
            return (from c in await Notification.eventDataHelper.Getdata()
                    where (c.time == time)
                    select c);
        }
    }
}
