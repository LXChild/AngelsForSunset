using System;
using System.Diagnostics;
using Windows.Storage;

namespace MyApp
{
    class LocalSettingsHelper
    {
        //检索本地应用数据存储
        public static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public const string KEY_ACCOUNT = "account";
        public const string KEY_USERNAME = "username";
        public const string KEY_PASSWORD = "password";
        public static ApplicationDataCompositeValue GetAccount()
        {
            return (ApplicationDataCompositeValue)localSettings.Values[KEY_ACCOUNT];
        }
        public static void SetAccount(string username, string password)
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite[KEY_USERNAME] = username;
            composite[KEY_PASSWORD] = password;

            localSettings.Values[KEY_ACCOUNT] = composite;
        }
        public static string GetUsername()
        {
            ApplicationDataCompositeValue composite = GetAccount();

            if (composite != null)
            {
                return (string)composite[KEY_USERNAME];
            }
            return null;
        }

        public const string KEY_SIGNIN = "isSignIn";
        public const string KEY_ROLE = "isElder";
        public static bool GetBoolStatus(string key)
        {
            object status = localSettings.Values[key];
            if (status != null)
            {
                return (bool)status;
            }
            return false;
        }
        public static bool SetBoolStatus(string key, bool status)
        {
            localSettings.Values[key] = status;
            return GetBoolStatus(key);
        }

        public const string KEY_SLEEP_START = "sleepTime_start";
        /// <summary>
        /// 清空睡眠开始时间
        /// </summary>
        public static void ClearStartSleepTime()
        {
            localSettings.Values[KEY_SLEEP_START] = DateTime.MinValue.ToString();
        }
        /// <summary>
        /// 获取睡眠开始时间
        /// </summary>
        public static DateTime GetSleepStartTime()
        {
            return Util.StringToDateTime((string)localSettings.Values[KEY_SLEEP_START]);
        }
        /// <summary>
        /// 设置睡眠开始时间
        /// </summary>
        public static void SetStartSleepTime()
        {
            localSettings.Values[KEY_SLEEP_START] = DateTime.Now.ToString();
        }

        public const string KEY_LAVATORY = "lavatory";
        public const string KEY_LAVATORY_STARTTIME = "lavatory_startTime";
        /// <summary>
        /// 获取如厕次数
        /// </summary>
        //public static ApplicationDataCompositeValue GetLavatory()
        //{
        //    return (ApplicationDataCompositeValue)localSettings.Values[KEY_LAVATORY];
        //}
        public static DateTime GetLavatoryStartTime()
        {
            //ApplicationDataCompositeValue composite = GetLavatory();

            //if (composite != null)
            //{
            //    return (DateTime)composite[KEY_LAVATORY_STARTTIME];
            //}
            //return DateTime.MinValue;
            return Util.StringToDateTime((string)localSettings.Values[KEY_LAVATORY_STARTTIME]);
        }
        public static void SetLavatoryStartTime()
        {
            localSettings.Values[KEY_LAVATORY_STARTTIME] = DateTime.Now.ToString();
        }
        //public static int GetLavatoryCount()
        //{
        //    ApplicationDataCompositeValue composite = GetLavatory();

        //    if (composite != null)
        //    {
        //        return (int)composite[KEY_LAVATORY_COUNT];
        //    }
        //    return 0;
        //}
        /// <summary>
        /// 记录如厕次数
        /// </summary>
        //public static void AddLavatory()
        //{
        //    ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();

        //    if (DateTime.Now.Hour < GetLavatoryStartTime().Hour)
        //    {
        //        composite[KEY_LAVATORY_COUNT] = 0;
        //    }
        //    composite[KEY_LAVATORY_COUNT] = GetLavatoryCount() + 1;
        //    composite[KEY_LAVATORY_STARTTIME] = DateTime.Now;

        //    localSettings.Values[KEY_LAVATORY] = composite;
        //}

        public static TimeSpan KEY_DINING = TimeSpan.Zero;
        public static string KEY_DINING_DETAIL = "{}";
        public static TimeSpan KEY_SLEEP = TimeSpan.Zero;
        public static string KEY_SLEEP_DETAIL = "{}";
        public static TimeSpan KEY_TOILET = TimeSpan.Zero;
        public static string KEY_TOILET_DETAIL = "{}";
        public static TimeSpan KEY_PARLOUR = TimeSpan.Zero;
        public static string KEY_PARLOUR_DETAIL = "{}";
        public static TimeSpan KEY_OUTDOOR = TimeSpan.Zero;
        public static string KEY_OUTDOOR_DETAIL = "{}";

        //public const string KEY_DAILY = "daily";
        //public const string KEY_DINING = "dining";
        //public const string KEY_DINING_DETAIL = "dining_detail";
        //public const string KEY_SLEEP = "sleep";
        //public const string KEY_SLEEP_DETAIL = "sleep_detail";
        //public const string KEY_TOILET = "toilet";
        //public const string KEY_TOILET_DETAIL = "toilet_detail";
        //public const string KEY_PARLOUR = "parlour";
        //public const string KEY_PARLOUR_DETAIL = "parlour_detail";
        //public const string KEY_OUTDOOR = "outdoor";
        //public const string KEY_OUTDOOR_DETAIL = "outdoor_detail";

        //public static void ClearDaily()
        //{
        //    ApplicationDataCompositeValue composite = GetDaily();
        //    if (composite != null)
        //    {
        //        composite[KEY_DINING] = "";
        //        composite[KEY_SLEEP] = "";
        //        composite[KEY_TOILET] = "";
        //        composite[KEY_PARLOUR] = "";
        //        composite[KEY_OUTDOOR] = "";
        //        composite[KEY_DINING_DETAIL] = "";
        //        composite[KEY_SLEEP_DETAIL] = "";
        //        composite[KEY_TOILET_DETAIL] = "";
        //        composite[KEY_PARLOUR_DETAIL] = "";
        //        composite[KEY_OUTDOOR_DETAIL] = "";
        //    }
        //}
        //public static ApplicationDataCompositeValue GetDaily()
        //{
        //    return (ApplicationDataCompositeValue)localSettings.Values[KEY_DAILY];
        //}
        //public static void SetDailyTime(string item, string content)
        //{
        //    ApplicationDataCompositeValue composite = GetDaily();

        //    if (composite == null)
        //    {
        //        composite = new ApplicationDataCompositeValue();
        //    }

        //    switch (item)
        //    {
        //        case "dining":
        //            composite[KEY_DINING] = content;
        //            break;
        //        case "sleep":
        //            composite[KEY_SLEEP] = content;
        //            break;
        //        case "toilet":
        //            composite[KEY_TOILET] = content;
        //            break;
        //        case "parlour":
        //            composite[KEY_PARLOUR] = content;
        //            break;
        //        case "outdoor":
        //            composite[KEY_OUTDOOR] = content;
        //            break;
        //        default:
        //            break;
        //    }
        //    localSettings.Values[KEY_DAILY] = composite;
        //}
        //public static void SetDailyDetail(string item, string content)
        //{
        //    ApplicationDataCompositeValue composite = GetDaily();

        //    if (composite == null)
        //    {
        //        composite = new ApplicationDataCompositeValue();
        //    }

        //    switch (item)
        //    {
        //        case "dining_detail":
        //            composite[KEY_OUTDOOR_DETAIL] = content;
        //            break;
        //        case "sleep_detail":
        //            composite[KEY_SLEEP_DETAIL] = content;
        //            break;
        //        case "toilet_detail":
        //            composite[KEY_TOILET_DETAIL] = content;
        //            break;
        //        case "parlour_detail":
        //            composite[KEY_PARLOUR_DETAIL] = content;
        //            break;
        //        case "outdoor_detail":
        //            composite[KEY_OUTDOOR_DETAIL] = content;
        //            break;
        //        default:
        //            break;
        //    }
        //    localSettings.Values[KEY_DAILY] = composite;
        //}
        //public static string GetDailyTime(string item)
        //{
        //    ApplicationDataCompositeValue composite = GetDaily();

        //    if (composite != null)
        //    {
        //        switch (item)
        //        {
        //            case "dining":
        //                return (string)composite[KEY_DINING];
        //            case "sleep":
        //                return (string)composite[KEY_SLEEP];
        //            case "toilet":
        //                return (string)composite[KEY_TOILET];
        //            case "parlour":
        //                return (string)composite[KEY_PARLOUR];
        //            case "outdoor":
        //                return (string)composite[KEY_OUTDOOR];
        //            default:
        //                break;
        //        }
        //    }
        //    return "";
        //}
        //public static string GetDailyDetail(string item)
        //{
        //    ApplicationDataCompositeValue composite = GetDaily();

        //    if (composite != null)
        //    {
        //        switch (item)
        //        {
        //            case "dining_detail":
        //                return (string)composite[KEY_DINING_DETAIL];
        //            case "sleep_detail":
        //                return (string)composite[KEY_SLEEP_DETAIL];
        //            case "toilet_detail":
        //                return (string)composite[KEY_TOILET_DETAIL];
        //            case "parlour_detail":
        //                return (string)composite[KEY_PARLOUR_DETAIL];
        //            case "outdoor_detail":
        //                return (string)composite[KEY_OUTDOOR_DETAIL];
        //            default:
        //                break;
        //        }
        //    }
        //    return "";
        //}

        public const string KEY_DAILY_DP_DATE = "dp_date";
        public static DateTimeOffset GetDPDate()
        {
            if (localSettings.Values.ContainsKey(KEY_DAILY_DP_DATE))
            {
                return (DateTimeOffset)localSettings.Values[KEY_DAILY_DP_DATE];
            } else
            {
                return Util.StringToDateOffset(DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
        }
        public static void SetDPDate(DateTimeOffset date)
        {
            localSettings.Values[KEY_DAILY_DP_DATE] = date;
        }
    }
}
