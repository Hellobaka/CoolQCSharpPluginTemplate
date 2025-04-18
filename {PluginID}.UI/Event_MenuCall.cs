﻿using System;
using System.Threading;
using {PluginID}.Sdk.Cqp.EventArgs;
using {PluginID}.Sdk.Cqp.Interface;
using {PluginID}.PublicInfos;

namespace {PluginID}.UI
{
    public class Event_MenuCall : IMenuCall
    {
        private MainWindow window = null;
        public void MenuCall(object sender, CQMenuCallEventArgs e)
        {
            try
            {
                if (window == null)
                {
                    Thread thread = new Thread(() =>
                    {
                        window = new MainWindow();
                        window.Closing += Window_Closing;
                        window.ShowDialog();
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
                else
                {
                    window.Activate();
                }
            }
            catch (Exception exc)
            {
                MainSave.CQLog.Info("Error", exc.Message, exc.StackTrace);
            }
        }

        ///<summary>
        ///窗体关闭时触发
        ///</summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            window = null;
        }
    }
}
