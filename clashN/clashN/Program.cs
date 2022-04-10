using clashN.Forms;
using clashN.Tool;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace clashN
{
    static class Program
    {
        public static EventWaitHandle ProgramStarted;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                Utils.SetClipboardData(args[0]);
            }

            ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, "ProgramStartedEvent", out bool bCreatedNew);
            if (!bCreatedNew)
            {
                ProgramStarted.Set();
                return;
            }

            Global.processJob = new Job();

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Logging.Setup();
            Utils.SaveLog($"clashN start up | {Utils.GetVersion()} | {Utils.GetExePath()}");
            Logging.ClearLogs();

            string lang = Utils.RegReadValue(Global.MyRegPath, Global.MyRegKeyLanguage, "zh-Hans");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);

            string font = Utils.RegReadValue(Global.MyRegPath, Global.MyRegKeyFont, "");
            if (!Utils.IsNullOrEmpty(font))
            {
                Application.SetDefaultFont(Utils.FromJson<Font>(font));
            }

            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary> 
        /// 检查是否已在运行
        /// </summary> 
        public static bool IsDuplicateInstance()
        {
            string name = Utils.GetExePath(); // Allow different locations to run
            name = name.Replace("\\", "/"); // https://stackoverflow.com/questions/20714120/could-not-find-a-part-of-the-path-error-while-creating-mutex

            Global.mutexObj = new Mutex(false, name, out bool bCreatedNew);
            return !bCreatedNew;
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Utils.SaveLog("Application_ThreadException", e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Utils.SaveLog("CurrentDomain_UnhandledException", (Exception)e.ExceptionObject);
        }
    }
}
