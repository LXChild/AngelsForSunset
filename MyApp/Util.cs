using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.ApplicationModel.Resources.Core;

namespace MyApp
{
    class Util
    {
        public static bool IsRunningOnPhone()
        {
            ResourceContext rescontext = ResourceContext.GetForCurrentView();

            string value = rescontext.QualifierValues["Devicefamily"];
            //if (value.Equals("Desktop"))
            //{
            //    return false;
            //}
            if (value.Equals("Mobile"))
            {
                return true;
            }
            return false;
        }

        public static string GetHMFromDatatime(string datetime)
        {
            string time = datetime.Split(new char[] { ' ' })[1];
            string[] hms = time.Split(new char[] { ':' });
            string result = hms[0] + ":" + hms[1];
            return result;
        }

        public static string GetHMFromTime(string time)
        {
            string[] hms = time.Split(new char[] { ':' });
            string result = hms[1] + ":" + hms[2].Substring(0, 2);
            return result;
        }

        public static string GetTimeFromDateTime(DateTime datetime)
        {
            return datetime.ToString().Split(new char[] {' '})[1];
        }

        public static string GetDateFromDatatime(string datetime)
        {
            return datetime.Split(new char[] { ' ' })[0];
        }

        public static DateTimeOffset StringToDateOffset(string date)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";

            DateTime result = Convert.ToDateTime(date, dtFormat);
            return result;
        }

        public static DateTimeOffset StringToDateTimeOffset(string datetime)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd hh:mm:ss";

            DateTime result = Convert.ToDateTime(datetime, dtFormat);
            return result;
        }
        public static DateTime StringToDateTime(string datetime)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd hh:mm:ss";

            return Convert.ToDateTime(datetime, dtFormat);
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        public static string ObjectToJsonData(object item)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());
            string result = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, item);
                ms.Position = 0;
                using (StreamReader reader = new StreamReader(ms))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
        public static T DataContractJsonDeSerializer<T>(string jsonString)
        {
            var ds = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ds.ReadObject(ms);
            ms.Dispose();
            return obj;
        }

        public static bool IsRecieveDataVaild(string data)
        {
            if (data.Contains("X"))
            {
                if (data.Contains("T"))
                {
                    if (data.Contains("W"))
                    {
                        if (data.Contains("C"))
                        {
                            if (data.Contains("K"))
                            {
                                if (data.Contains("B"))
                                {
                                    if (data.Contains("S"))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static int GetHeartRateFromBluetoothRecieve(string data)
        {
            try
            {
                string temp = data.Split(new char[] { 'T' })[0];
                return int.Parse(temp.Split(new char[] { 'X' })[1]);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return 0;
        }

        public static double GetTemperatureFromBluetoothRecieve(string data)
        {
            try
            {
                string temp = data.Split(new char[] { 'W' })[0];
                return double.Parse(temp.Split(new char[] { 'T' })[1]);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return 0;
        }

        public static string GetRoomFromBluetoothRecieve(string data)
        {
            try
            {
                string temp = data.Split(new char[] { 'W' })[1];
                if (temp.Contains("1"))
                {
                    temp = temp.Split(new char[] { '1' })[0];
                    if (temp == "")
                    {
                        return "W";
                    }
                    else
                    {
                        string[] arr = temp.Split(new char[] { '0' });
                        return arr[arr.Length - 1];
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public static bool GetTripFromBluetoothRecieve(string data)
        {
            try
            {
                string temp = data.Split(new char[] { 'S' })[1];
                if (temp.Contains("1"))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }

        public static bool GetLavatoryNullFromRecieve(string data)
        {
            try
            {
                string temp = data.Split(new char[] { 'B' })[1];
                string res = temp.Split(new char[] { 'S' })[0];
                if (res == "0")
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }
    }
}
