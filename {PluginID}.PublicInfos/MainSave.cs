using {PluginID}.Sdk.Cqp;
using {PluginID}.Tool.IniConfig;
using System.Collections.Generic;
using System.IO;

namespace PublicInfos
{
    public static class MainSave
    {
        /// <summary>
        /// 保存各种事件的数组
        /// </summary>
        public static List<IOrderModel> Instances { get; set; } = new List<IOrderModel>();
        public static CQLog CQLog { get; set; }
        public static CQApi CQApi { get; set; }
        public static string AppDirectory { get; set; }
        public static string ImageDirectory { get; set; }

        static IniConfig configMain;
        public static IniConfig ConfigMain
        {
            get
            {
                if (configMain != null)
                    return configMain;
                configMain = new IniConfig(Path.Combine(AppDirectory, "Config.ini"), System.Text.Encoding.UTF8);
                configMain.Load();
                return configMain;
            }
            set { configMain = value; }
        }
    }
}