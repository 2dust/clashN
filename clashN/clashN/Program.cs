using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using clashN.Forms;
using clashN.Properties;
using clashN.Tool;

namespace clashN
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            if (!IsDuplicateInstance())
            {
                Logging.Setup();
                Utils.SaveLog($"clashN start up | {Utils.GetVersion()} | {Utils.GetExePath()}");

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
            else
            {
                UI.ShowWarning($"clashN is already running(clashN已经运行)");
            }
        }

        //private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    try
        //    {
        //        string resourceName = "clashN.LIB." + new AssemblyName(args.Name).Name + ".dll";
        //        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
        //        {
        //            if (stream == null)
        //            {
        //                return null;
        //            }
        //            byte[] assemblyData = new byte[stream.Length];
        //            stream.Read(assemblyData, 0, assemblyData.Length);
        //            return Assembly.Load(assemblyData);
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        /// <summary> 
        /// 检查是否已在运行
        /// </summary> 
        public static bool IsDuplicateInstance()
        {
            //string name = "clashN";

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
