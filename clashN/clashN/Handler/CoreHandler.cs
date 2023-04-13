using ClashN.Mode;
using ClashN.Resx;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ClashN.Handler
{
    /// <summary>
    /// core进程处理类
    /// </summary>
    internal class CoreHandler
    {
        private static string coreConfigRes = Global.coreConfigFileName;
        private CoreInfo coreInfo;
        private Process _process;
        private Action<bool, string> _updateFunc;

        public CoreHandler(Action<bool, string> update)
        {
            _updateFunc = update;
        }

        /// <summary>
        /// 载入Core
        /// </summary>
        public void LoadCore(Config config)
        {
            if (Global.reloadCore)
            {
                var item = ConfigProc.GetDefaultProfile(ref config);
                if (item == null)
                {
                    CoreStop();
                    ShowMsg(false, ResUI.CheckProfileSettings);
                    return;
                }

                if (config.EnableTun && !Utils.IsAdministrator())
                {
                    ShowMsg(true, ResUI.EnableTunModeFailed);
                    return;
                }
                if (config.EnableTun && item.coreType == CoreKind.Clash)
                {
                    ShowMsg(true, ResUI.TunModeCoreTip);
                    return;
                }

                SetCore(config, item, out bool blChanged);
                string fileName = Utils.GetConfigPath(coreConfigRes);
                if (CoreConfigHandler.GenerateClientConfig(item, fileName, false, out string msg) != 0)
                {
                    CoreStop();
                    ShowMsg(false, msg);
                }
                else
                {
                    ShowMsg(true, msg);

                    if (_process != null && !_process.HasExited && !blChanged)
                    {
                        MainFormHandler.Instance.ClashConfigReload(fileName);
                    }
                    else
                    {
                        CoreRestart(item);
                    }
                }
            }
        }

        /// <summary>
        /// Core重启
        /// </summary>
        private void CoreRestart(ProfileItem item)
        {
            CoreStop();
            Thread.Sleep(1000);
            CoreStart(item);
        }

        /// <summary>
        /// Core停止
        /// </summary>
        public void CoreStop()
        {
            try
            {
                if (_process != null)
                {
                    KillProcess(_process);
                    _process.Dispose();
                    _process = null;
                }
                else
                {
                    if (coreInfo == null || coreInfo.coreExes == null)
                    {
                        return;
                    }

                    foreach (string vName in coreInfo.coreExes)
                    {
                        Process[] existing = Process.GetProcessesByName(vName);
                        foreach (Process p in existing)
                        {
                            string path = p.MainModule.FileName;
                            if (path == $"{Utils.GetBinPath(vName, coreInfo.coreType)}.exe")
                            {
                                KillProcess(p);
                            }
                        }
                    }
                }

                //bool blExist = true;
                //if (processId > 0)
                //{
                //    Process p1 = Process.GetProcessById(processId);
                //    if (p1 != null)
                //    {
                //        p1.Kill();
                //        blExist = false;
                //    }
                //}
                //if (blExist)
                //{
                //    foreach (string vName in lstCore)
                //    {
                //        Process[] killPro = Process.GetProcessesByName(vName);
                //        foreach (Process p in killPro)
                //        {
                //            p.Kill();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }
        }

        /// <summary>
        /// Core停止
        /// </summary>
        public void CoreStopPid(int pid)
        {
            try
            {
                Process _p = Process.GetProcessById(pid);
                KillProcess(_p);
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }
        }

        private string FindCoreExe()
        {
            string fileName = string.Empty;
            foreach (string name in coreInfo.coreExes)
            {
                string vName = string.Format("{0}.exe", name);
                vName = Utils.GetBinPath(vName, coreInfo.coreType);
                if (File.Exists(vName))
                {
                    fileName = vName;
                    break;
                }
            }
            if (string.IsNullOrEmpty(fileName))
            {
                string msg = string.Format(ResUI.NotFoundCore, coreInfo.coreUrl);
                ShowMsg(false, msg);
            }
            return fileName;
        }

        /// <summary>
        /// Core启动
        /// </summary>
        private void CoreStart(ProfileItem item)
        {
            ShowMsg(false, string.Format(ResUI.StartService, DateTime.Now.ToString()));
            ShowMsg(false, $"{ResUI.TbCoreType} {coreInfo.coreType.ToString()}");

            try
            {
                string fileName = FindCoreExe();
                if (fileName == "") return;

                //Portable Mode
                var arguments = coreInfo.arguments;
                var data = Utils.GetPath("data");
                if (Directory.Exists(data))
                {
                    arguments += $" -d \"{data}\"";
                }

                Process p = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        WorkingDirectory = Utils.GetConfigPath(),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
                    }
                };
                //if (config.enableTun)
                //{
                //    p.StartInfo.Verb = "runas";
                //}
                p.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        string msg = e.Data + Environment.NewLine;
                        ShowMsg(false, msg);
                    }
                });
                p.Start();
                //p.PriorityClass = ProcessPriorityClass.High;
                p.BeginOutputReadLine();
                //processId = p.Id;
                _process = p;

                if (p.WaitForExit(1000))
                {
                    throw new Exception(p.StandardError.ReadToEnd());
                }

                Global.processJob.AddProcess(p.Handle);
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
                string msg = ex.Message;
                ShowMsg(true, msg);
            }
        }

        private void ShowMsg(bool updateToTrayTooltip, string msg)
        {
            _updateFunc(updateToTrayTooltip, msg);
        }

        private void KillProcess(Process p)
        {
            try
            {
                p.CloseMainWindow();
                p.WaitForExit(100);
                if (!p.HasExited)
                {
                    p.Kill();
                    p.WaitForExit(100);
                }
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }
        }

        private int SetCore(Config config, ProfileItem item, out bool blChanged)
        {
            blChanged = true;
            if (item == null)
            {
                return -1;
            }
            var coreType = LazyConfig.Instance.GetCoreType(item);
            var tempInfo = LazyConfig.Instance.GetCoreInfo(coreType);
            if (tempInfo != null && coreInfo != null && tempInfo.coreType == coreInfo.coreType)
            {
                blChanged = false;
            }

            coreInfo = tempInfo;
            if (coreInfo == null)
            {
                return -1;
            }
            return 0;
        }
    }
}