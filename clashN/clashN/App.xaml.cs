using ClashN.Handler;
using ClashN.Mode;
using ClashN.Tool;
using System.Windows;
using System.Windows.Threading;

namespace ClashN
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EventWaitHandle? ProgramStarted;
        private Config? _config;

        static App()
        {
            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "ProgramStartedEvent", out bool bCreatedNew);
            if (!bCreatedNew)
            {
                ProgramStarted.Set();
                App.Current.Shutdown();
                Environment.Exit(-1);
                return;
            }
        }

        public App()
        {
            // Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        /// <summary>
        /// 只打开一个进程
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            foreach (string arg in e.Args)
            {
                Utils.SetClipboardData(arg);
            }

            Global.processJob = new Job();

            Logging.Setup();
            Utils.SaveLog($"ClashN start up | {Utils.GetVersion()} | {Utils.GetExePath()}");
            Logging.ClearLogs();

            Init();

            string lang = Utils.RegReadValue(Global.MyRegPath, Global.MyRegKeyLanguage, Global.Languages[0]);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);

            base.OnStartup(e);
        }

        private void Init()
        {
            if (ConfigProc.LoadConfig(ref _config) != 0)
            {
                UI.ShowWarning($"Loading GUI configuration file is abnormal,please restart the application{Environment.NewLine}加载GUI配置文件异常,请重启应用");
                Application.Current.Shutdown();
                Environment.Exit(0);
                return;
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Utils.SaveLog("App_DispatcherUnhandledException", e.Exception);
            e.Handled = true;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                Utils.SaveLog("CurrentDomain_UnhandledException", (Exception)e.ExceptionObject!);
            }
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            Utils.SaveLog("TaskScheduler_UnobservedTaskException", e.Exception);
        }
    }
}