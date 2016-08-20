using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MyApp
{
    class UploadFunctions
    {
        public static async Task<string> UploadHeartbeat(int beat)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("time", DateTime.Now.ToString());
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("beat", beat.ToString());

            return await conn.SendPostRequest(ConnectServer.URL_UPLOADHEARTBEAT, dic_param);
        }

        public static async Task<string> UploadTemperature(double temperature)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("time", DateTime.Now.ToString());
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("temperature", temperature.ToString());

            return await conn.SendPostRequest(ConnectServer.URL_UPLOADTEMPERATURE, dic_param);
        }

        public static async Task<string> UploadDaily(double dining, string detail_dining, double sleep, string detail_sleep, double toilet, string detail_toilet, double parlour, string detail_parlour, double outdoor, string detail_outdoor)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            //  dic_param.Add("date", DateTime.Now.Date.ToString("yyyy/MM/dd"));
             dic_param.Add("date", DateTime.Now.Date.ToString("2016/04/25"));

            //dic_param.Add("date", "2016/02/18");
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("dining", dining.ToString());
            dic_param.Add("detail_dining", detail_dining);
            dic_param.Add("sleep", sleep.ToString());
            dic_param.Add("detail_sleep", detail_sleep);
            dic_param.Add("toilet", toilet.ToString());
            dic_param.Add("detail_toilet", detail_toilet);
            dic_param.Add("parlour", parlour.ToString());
            dic_param.Add("detail_parlour", detail_parlour);
            dic_param.Add("outdoor", outdoor.ToString());
            dic_param.Add("detail_outdoor", detail_outdoor);

            return await conn.SendPostRequest(ConnectServer.URL_UPLOADDAILY, dic_param);
        }

        public static async Task<string> UploadPosition(double longitude, double latitude)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("time", DateTime.Now.ToString());
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("longitude", longitude.ToString());
            dic_param.Add("latitude", latitude.ToString());

            return await conn.SendPostRequest(ConnectServer.URL_UPLOADPOSITION, dic_param);
        }

        public static async Task<string> UploadIndoorPosition(string room)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("time", DateTime.Now.ToString());
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("room", room);

            return await conn.SendPostRequest(ConnectServer.URL_UPLOADINDOORPOSITION, dic_param);
        }

        public static async Task<string> UploadRequestBindRelationInfo(string parentname)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("parentname", parentname);
            return await conn.SendPostRequest(ConnectServer.URL_REQUESTBINDRELATION, dic_param);
        }

        public static async Task<string> UploadBindRelationInfo(string parentname, string childname, string answer)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("parentname", parentname);
            dic_param.Add("childname", childname);
            dic_param.Add("answer", answer);
            return await conn.SendPostRequest(ConnectServer.URL_BINDRELATION, dic_param);
        }

        public static async Task<string> UploadRemind(string message)
        {
            string role = "";
            if (LocalSettingsHelper.GetBoolStatus(LocalSettingsHelper.KEY_ROLE))
            {
                role = "parent";
            } else
            {
                role = "child";
            }
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("time", DateTime.Now.ToString());
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("role", role);
            dic_param.Add("message", message);
            return await conn.SendPostRequest(ConnectServer.URL_UPLOADREMIND, dic_param);
        }

        public static async Task<string> UploadTripInfo(string place)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("time", DateTime.Now.ToString());
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("place", place);
            return await conn.SendPostRequest(ConnectServer.URL_UPLOADTRIP, dic_param);
        }

        public static async Task<string> UploadSleepInfo(string action)
        {
            ConnectServer conn = new ConnectServer();
            //将post使用的参数加入字典
            Dictionary<string, string> dic_param = new Dictionary<string, string>();
            dic_param.Add("time", DateTime.Now.ToString());
            dic_param.Add("username", LocalSettingsHelper.GetUsername());
            dic_param.Add("action", action);
            return await conn.SendPostRequest(ConnectServer.URL_UPLOADSLEEP, dic_param);
        }
    }
}
