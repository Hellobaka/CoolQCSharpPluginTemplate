using System;
using System.Threading;
using {PluginID}.Sdk.Cqp.EventArgs;
using {PluginID}.Sdk.Cqp.Interface;
using {PluginID}.PublicInfos;
using System.Windows;


namespace {PluginID}.UI
{
    public class Event_MenuCall : IMenuCall
    {
        private App App { get; set; }

        public void MenuCall(object sender, CQMenuCallEventArgs e)
        {
            try
            {
                if (App == null)
                {
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            App = new();
                            App.ShutdownMode = ShutdownMode.OnMainWindowClose;
                            App.InitializeComponent();
                            App.Run();
                        }
                        catch (Exception exc)
                        {
                            MainSave.CQLog?.Error("UI异常", exc.ToString());
                        }
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
                else
                {
                    MainWindow.Instance.Dispatcher.BeginInvoke(new Action(MainWindow.Instance.Show));
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
            MainWindow.Instance.Dispatcher.BeginInvoke(new Action(MainWindow.Instance.Hide));
        }
    }
}
