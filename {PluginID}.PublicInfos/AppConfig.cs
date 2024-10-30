using System;
using System.Collections.Generic;
using System.IO;

namespace {PluginID}.PublicInfos
{
    public class AppConfig : ConfigBase
    {
        public AppConfig(string path)
            : base(path)
        {
            LoadConfig();
            Instance = this;
        }

        public override void LoadConfig()
        {
        }
    }
}