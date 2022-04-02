using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using clashN.Base;
using clashN.Mode;
using clashN.Resx;

namespace clashN.Handler
{
    class UpdateHandle
    {
        Action<bool, string> _updateFunc;
        private Config _config;

        public event EventHandler<ResultEventArgs> AbsoluteCompleted;

        public class ResultEventArgs : EventArgs
        {
            public bool Success;
            public string Msg;

            public ResultEventArgs(bool success, string msg)
            {
                this.Success = success;
                this.Msg = msg;
            }
        }

        private readonly string nLatestUrl = Global.NUrl + "/latest";
        private const string nUrl = Global.NUrl + "/download/{0}/clashN.zip";
        private readonly string clashCoreLatestUrl = Global.clashCoreUrl + "/latest";
        private const string clashCoreUrl32 = Global.clashCoreUrl + "/download/{0}/clash-windows-386-{0}.zip";
        private const string clashCoreUrl64 = Global.clashCoreUrl + "/download/{0}/clash-windows-amd64-{0}.zip";
        private readonly string clashMetaCoreLatestUrl = Global.clashMetaCoreUrl + "/latest";
        private const string clashMetaCoreUrl32 = Global.clashMetaCoreUrl + "/download/{0}/Clash.Meta-windows-386-{0}.zip";
        private const string clashMetaCoreUrl64 = Global.clashMetaCoreUrl + "/download/{0}/Clash.Meta-windows-amd64-{0}.zip";
        private const string geoUrl = "https://github.com/Loyalsoldier/v2ray-rules-dat/releases/latest/download/{0}.dat";

        public void CheckUpdateGuiN(Config config, Action<bool, string> update)
        {
            _config = config;
            _updateFunc = update;
            var url = string.Empty;

            DownloadHandle downloadHandle = null;
            if (downloadHandle == null)
            {
                downloadHandle = new DownloadHandle();

                downloadHandle.UpdateCompleted += (sender2, args) =>
                {
                    if (args.Success)
                    {
                        _updateFunc(false, ResUI.MsgDownloadCoreSuccessfully);

                        try
                        {
                            string fileName = Utils.GetPath(Utils.GetDownloadFileName(url));
                            fileName = Utils.UrlEncode(fileName);
                            Process process = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = "clashUpgrade.exe",
                                    Arguments = "\"" + fileName + "\"",
                                    WorkingDirectory = Utils.StartupPath()
                                }
                            };
                            process.Start();
                            if (process.Id > 0)
                            {
                                _updateFunc(true, "");
                            }
                        }
                        catch (Exception ex)
                        {
                            _updateFunc(false, ex.Message);
                        }
                    }
                    else
                    {
                        _updateFunc(false, args.Msg);
                    }
                };
                downloadHandle.Error += (sender2, args) =>
                {
                    _updateFunc(false, args.GetException().Message);
                };
            }
            AbsoluteCompleted += (sender2, args) =>
            {
                if (args.Success)
                {
                    _updateFunc(false, string.Format(ResUI.MsgParsingSuccessfully, "clashN"));

                    url = args.Msg;
                    askToDownload(downloadHandle, url, true);
                }
                else
                {
                    _updateFunc(false, args.Msg);
                }
            };
            _updateFunc(false, string.Format(ResUI.MsgStartUpdating, "clashN"));
            CheckUpdateAsync(ECoreType.clashN);
        }


        public void CheckUpdateCore(ECoreType type, Config config, Action<bool, string> update)
        {
            _config = config;
            _updateFunc = update;
            var url = string.Empty;

            DownloadHandle downloadHandle = null;
            if (downloadHandle == null)
            {
                downloadHandle = new DownloadHandle();
                downloadHandle.UpdateCompleted += (sender2, args) =>
                {
                    if (args.Success)
                    {
                        _updateFunc(false, ResUI.MsgDownloadCoreSuccessfully);
                        _updateFunc(false, ResUI.MsgUnpacking);

                        try
                        {
                            _updateFunc(true, url);
                        }
                        catch (Exception ex)
                        {
                            _updateFunc(false, ex.Message);
                        }
                    }
                    else
                    {
                        _updateFunc(false, args.Msg);
                    }
                };
                downloadHandle.Error += (sender2, args) =>
                {
                    _updateFunc(true, args.GetException().Message);
                };
            }

            AbsoluteCompleted += (sender2, args) =>
            {
                if (args.Success)
                {
                    _updateFunc(false, string.Format(ResUI.MsgParsingSuccessfully, "Core"));
                    url = args.Msg;
                    askToDownload(downloadHandle, url, true);
                }
                else
                {
                    _updateFunc(false, args.Msg);
                }
            };
            _updateFunc(false, string.Format(ResUI.MsgStartUpdating, "Core"));
            CheckUpdateAsync(type);
        }


        public void UpdateSubscriptionProcess(Config config, bool blProxy, Action<bool, string> update)
        {
            _config = config;
            _updateFunc = update;

            _updateFunc(false, ResUI.MsgUpdateSubscriptionStart);

            if (config.profileItems == null || config.profileItems.Count <= 0)
            {
                _updateFunc(false, ResUI.MsgNoValidSubscription);
                return;
            }

            foreach (var item in config.profileItems)
            {
                if (item.enabled == false)
                {
                    continue;
                }
                string indexId = item.indexId.TrimEx();
                string url = item.url.TrimEx();
                string userAgent = item.userAgent.TrimEx();
                string groupId = item.groupId.TrimEx();
                string hashCode = $"{item.remarks}->";
                if (Utils.IsNullOrEmpty(indexId) || Utils.IsNullOrEmpty(url))
                {
                    //_updateFunc(false, $"{hashCode}{ResUI.MsgNoValidSubscription}");
                    continue;
                }

                Task.Run(async () =>
                {
                    _updateFunc(false, $"{hashCode}{ResUI.MsgStartGettingSubscriptions}");
                    var result = await (new DownloadHandle()).DownloadStringAsync(url, blProxy, userAgent);

                    _updateFunc(false, $"{hashCode}{ResUI.MsgGetSubscriptionSuccessfully}");

                    if (Utils.IsNullOrEmpty(result))
                    {
                        _updateFunc(false, $"{hashCode}{ResUI.MsgSubscriptionDecodingFailed}");
                        return;
                    }

                    int ret = ConfigHandler.AddBatchProfiles(ref config, result, indexId, groupId);
                    if (ret == 0)
                    {
                    }
                    else
                    {
                        _updateFunc(false, $"{hashCode}{ResUI.MsgFailedImportSubscription}");
                    }
                    _updateFunc(true, $"{hashCode}{ResUI.MsgUpdateSubscriptionEnd}");
                });
            }
        }


        public void UpdateGeoFile(string geoName, Config config, Action<bool, string> update)
        {
            _config = config;
            _updateFunc = update;
            var url = string.Format(geoUrl, geoName);

            DownloadHandle downloadHandle = null;
            if (downloadHandle == null)
            {
                downloadHandle = new DownloadHandle();

                downloadHandle.UpdateCompleted += (sender2, args) =>
                {
                    if (args.Success)
                    {
                        _updateFunc(false, string.Format(ResUI.MsgDownloadGeoFileSuccessfully, geoName));

                        try
                        {
                            string fileName = Utils.GetPath(Utils.GetDownloadFileName(url));
                            if (File.Exists(fileName))
                            {
                                string targetPath = Utils.GetPath($"{geoName}.dat");
                                if (File.Exists(targetPath))
                                {
                                    File.Delete(targetPath);
                                }
                                File.Move(fileName, targetPath);
                                _updateFunc(true, "");
                            }
                        }
                        catch (Exception ex)
                        {
                            _updateFunc(false, ex.Message);
                        }
                    }
                    else
                    {
                        _updateFunc(false, args.Msg);
                    }
                };
                downloadHandle.Error += (sender2, args) =>
                {
                    _updateFunc(false, args.GetException().Message);
                };
            }

            askToDownload(downloadHandle, url, false);
        }

        #region private

        private async void CheckUpdateAsync(ECoreType type)
        {
            try
            {
                Utils.SetSecurityProtocol(LazyConfig.Instance.GetConfig().enableSecurityProtocolTls13);
                SocketsHttpHandler webRequestHandler = new SocketsHttpHandler
                {
                    AllowAutoRedirect = false,
                    Proxy = new WebProxy($"socks5://{Global.Loopback}:{LazyConfig.Instance.GetConfig().socksPort}")
                };
                HttpClient httpClient = new HttpClient(webRequestHandler);

                string url;
                if (type == ECoreType.clash)
                {
                    url = clashCoreLatestUrl;
                }
                else if (type == ECoreType.clash_meta)
                {
                    url = clashMetaCoreLatestUrl;
                }
                else if (type == ECoreType.clashN)
                {
                    url = nLatestUrl;
                }
                else
                {
                    throw new ArgumentException("Type");
                }
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.StatusCode.ToString() == "Redirect")
                {
                    responseHandler(type, response.Headers.Location.ToString());
                }
                else
                {
                    Utils.SaveLog("StatusCode error: " + url);
                    return;
                }
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
                _updateFunc(false, ex.Message);
            }
        }

        /// <summary>
        /// 获取Core版本
        /// </summary>
        private string getCoreVersion(ECoreType type)
        {
            try
            {
                var core = string.Empty;
                var match = string.Empty;
                if (type == ECoreType.clash)
                {
                    core = "clash-windows-amd64.exe";
                    match = "Clash";
                }
                else if (type == ECoreType.clash_meta)
                {
                    core = "Clash.Meta-windows-amd64.exe";
                    match = "Clash Meta";
                }
                string filePath = Utils.GetPath(core);
                if (!File.Exists(filePath))
                {
                    string msg = string.Format(ResUI.NotFoundCore, @"");
                    //ShowMsg(true, msg);
                    return "";
                }

                Process p = new Process();
                p.StartInfo.FileName = filePath;
                p.StartInfo.Arguments = "-v";
                p.StartInfo.WorkingDirectory = Utils.StartupPath();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                p.Start();
                p.WaitForExit(5000);
                string echo = p.StandardOutput.ReadToEnd();
                string version = Regex.Match(echo, $"v[0-9.]+").Groups[0].Value;
                return version;
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
                _updateFunc(false, ex.Message);
                return "";
            }
        }
        private void responseHandler(ECoreType type, string redirectUrl)
        {
            try
            {
                string version = redirectUrl.Substring(redirectUrl.LastIndexOf("/", StringComparison.Ordinal) + 1);

                string curVersion;
                string message;
                string url;
                if (type == ECoreType.clash)
                {
                    curVersion = getCoreVersion(type);
                    message = string.Format(ResUI.IsLatestCore, curVersion);
                    if (Environment.Is64BitProcess)
                    {
                        url = string.Format(clashCoreUrl64, version);
                    }
                    else
                    {
                        url = string.Format(clashCoreUrl32, version);
                    }
                }
                else if (type == ECoreType.clash_meta)
                {
                    curVersion = getCoreVersion(type);
                    message = string.Format(ResUI.IsLatestCore, curVersion);
                    if (Environment.Is64BitProcess)
                    {
                        url = string.Format(clashMetaCoreUrl64, version);
                    }
                    else
                    {
                        url = string.Format(clashMetaCoreUrl32, version);
                    }
                }
                else if (type == ECoreType.clashN)
                {
                    curVersion = FileVersionInfo.GetVersionInfo(Utils.GetExePath()).FileVersion.ToString();
                    message = string.Format(ResUI.IsLatestN, curVersion);
                    url = string.Format(nUrl, version);
                }
                else
                {
                    throw new ArgumentException("Type");
                }

                if (curVersion == version)
                {
                    AbsoluteCompleted?.Invoke(this, new ResultEventArgs(false, message));
                    return;
                }

                AbsoluteCompleted?.Invoke(this, new ResultEventArgs(true, url));
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
                _updateFunc(false, ex.Message);
            }
        }

        private void askToDownload(DownloadHandle downloadHandle, string url, bool blAsk)
        {
            bool blDownload = false;
            if (blAsk)
            {
                if (UI.ShowYesNo(string.Format(ResUI.DownloadYesNo, url)) == DialogResult.Yes)
                {
                    blDownload = true;
                }
            }
            else
            {
                blDownload = true;
            }
            if (blDownload)
            {
                downloadHandle.DownloadFileAsync(url, true, 600);
            }
        }

        private int httpProxyTest()
        {
            var statistics = new SpeedtestHandler(ref _config);
            return statistics.RunAvailabilityCheck();
        }
        #endregion
    }
}
