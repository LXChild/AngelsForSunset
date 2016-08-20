using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace MyApp
{
    class ConnectServer
    {
        public const string URL_QUERYUSER = "http://www.careelder.applinzi.com/queryUser.php";
        public const string URL_INSERTUSER = "http://www.careelder.applinzi.com/insertUser.php";
        public const string URL_UPLOADCHANNELURL = "http://www.careelder.applinzi.com/uploadChannelURL.php";
        public const string URL_UPLOADHEARTBEAT = "http://www.careelder.applinzi.com/uploadHeartbeat.php";
        public const string URL_UPLOADTEMPERATURE = "http://www.careelder.applinzi.com/uploadTemperature.php";
        public const string URL_UPLOADDAILY = "http://www.careelder.applinzi.com/uploadDaily.php";
        public const string URL_UPLOADPOSITION = "http://www.careelder.applinzi.com/uploadPosition.php";
        public const string URL_UPLOADINDOORPOSITION = "http://www.careelder.applinzi.com/uploadIndoorPosition.php";
        public const string URL_REQUESTBINDRELATION = "http://www.careelder.applinzi.com/requestBindRelation.php";
        public const string URL_BINDRELATION = "http://www.careelder.applinzi.com/bindRelation.php";
        public const string URL_UPLOADREMIND = "http://www.careelder.applinzi.com/pushChatNotification.php";
        public const string URL_UPLOADTRIP = "http://www.careelder.applinzi.com/uploadTrip.php";
        public const string URL_UPLOADSLEEP = "http://www.careelder.applinzi.com/uploadSleep.php";

        public async Task<string> SendGetRequest()
        {
            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri(URL_QUERYUSER);

            //Send the GET request asynchronously and retrieve the response as a string.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            return httpResponseBody;
        }

        public async Task<string> SendPostRequest(string action, Dictionary<string, string> dic_params)
        {
            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            Uri requestUri = new Uri(action);

            //Send the GET request asynchronously and retrieve the response as a string.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            //将传入的post参数打包
            string post_params = "";
            int i = 0;
            foreach (KeyValuePair<string, string> param in dic_params)
            {
                if (i == 0)
                {
                    post_params += param.Key + "=" + param.Value;
                }
                else
                {
                    post_params += "&" + param.Key + "=" + param.Value;
                }
                i++;
            }

            //post_params = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(post_params));
            try
            {
                //Send the GET request
                HttpStringContent httpStringContent = new HttpStringContent(post_params, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/x-www-form-urlencoded");
                System.Diagnostics.Debug.WriteLine(httpStringContent);
                HttpResponseMessage response = await httpClient.PostAsync(requestUri, httpStringContent);

                httpResponseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            return httpResponseBody;
        }
    }
}
