using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.Sdk.Cqp.Model;
using Native.Tool.IniConfig;

namespace PublicInfos
{
    public static class CommonHelper
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
        /// <summary>
        /// 获取CQ码中的图片网址
        /// </summary>
        /// <param name="imageCQCode">需要解析的图片CQ码</param>
        /// <returns></returns>
        public static string GetImageURL(string imageCQCode)
        {
            string path = MainSave.ImageDirectory + CQCode.Parse(imageCQCode)[0].Items["file"] + ".cqimg";
            IniConfig image = new IniConfig(path);
            image.Load();
            return image.Object["image"]["url"].ToString();
        }
        public static string GetAppImageDirectory()
        {
            var ImageDirectory = Path.Combine(Environment.CurrentDirectory, "data", "image\\");
            return ImageDirectory;
        }
        

        public static async Task<string> Get(string url, string cookie = "")
        {
            try
            {
                HttpClientHandler handler = new();
                handler.CookieContainer = new CookieContainer();
                if (!string.IsNullOrEmpty(cookie))
                {
                    foreach (var item in cookie.Split(';'))
                    {
                        if (string.IsNullOrEmpty(item) is false)
                        {
                            string[] c = item.Split('=');
                            handler.CookieContainer.Add(new Uri("https://weibo.com/"), new Cookie(c.First(), c.Last()));
                        }
                    }
                }

                using var http = new HttpClient(handler);
                http.DefaultRequestHeaders.Add("user-agent", UA);
                var r = await http.GetAsync(url);
                r.Content.Headers.ContentType.CharSet = "UTF-8";
                return await r.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                MainSave.CQLog.Debug("Get", e.Message);
                return string.Empty;
            }
        }

        public static async Task<string> Post(string url, HttpContent body)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("user-agent", UA);
                var r = await http.PostAsync(url, body);
                r.Content.Headers.ContentType.CharSet = "UTF-8";
                return await r.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                MainSave.CQLog.Debug("Post", e.Message);
                return string.Empty;
            }
        }

        public static string ParseLongNumber(int num)
        {
            string numStr = num.ToString();
            int step = 1;
            for (int i = numStr.Length - 1; i > 0; i--)
            {
                if (step % 3 == 0)
                {
                    numStr = numStr.Insert(i, ",");
                }
                step++;
            }
            return numStr;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="path">目标文件夹</param>
        /// <param name="overwrite">重复时是否覆写</param>
        /// <returns></returns>
        public static async Task<bool> DownloadFile(string url, string path, bool overwrite = false)
        {
            using var http = new HttpClient();
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return false;
                }

                string fileName = GetFileNameFromURL(url);
                if (!overwrite && File.Exists(Path.Combine(path, fileName)))
                {
                    return true;
                }

                var r = await http.GetAsync(url);
                byte[] buffer = await r.Content.ReadAsByteArrayAsync();
                Directory.CreateDirectory(path);
                File.WriteAllBytes(Path.Combine(path, fileName), buffer);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public static string GetFileNameFromURL(this string url)
        {
            return url.Split('/').Last().Split('?').First();
        }

        public static string ParseNum2Chinese(this int num)
        {
            return num > 10000 ? $"{num / 10000.0:f1}万" : num.ToString();
        }

        public static bool CompareNumString(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return a.Length > b.Length;
            }

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                {
                    return a[i] > b[i];
                }
            }
            return false;
        }
    }
}
