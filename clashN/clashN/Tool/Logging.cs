using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;

namespace ClashN.Tool
{
    public class Logging
    {
        public static void Setup()
        {
            LoggingConfiguration config = new LoggingConfiguration();
            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);
            fileTarget.Layout = "${longdate}-${level:uppercase=true} ${message}";
            fileTarget.FileName = Utils.GetPath(@"guiLogs/") + "${shortdate}.txt";
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));
            LogManager.Configuration = config;
        }

        public static void ClearLogs()
        {
            Task.Run(() =>
            {
                try
                {
                    var now = DateTime.Now.AddMonths(-1);
                    var dir = Utils.GetPath(@"guiLogs\");
                    var files = Directory.GetFiles(dir, "*.txt");
                    foreach (var filePath in files)
                    {
                        var file = new FileInfo(filePath);
                        if (file.CreationTime < now)
                        {
                            try
                            {
                                file.Delete();
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            });
        }
    }
}