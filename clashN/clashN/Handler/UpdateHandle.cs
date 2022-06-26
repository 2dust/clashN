using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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


        public void UpdateSubscriptionProcess(Config config, bool blProxy, List<ProfileItem> profileItems, Action<bool, string> update)
        {
            _config = config;
            _updateFunc = update;

            _updateFunc(false, ResUI.MsgUpdateSubscriptionStart);

            if (config.profileItems == null || config.profileItems.Count <= 0)
            {
                _updateFunc(false, ResUI.MsgNoValidSubscription);
                return;
            }

            Task.Run(async () =>
            {
                //Turn off system proxy
                bool bSysProxyType = false;
                if (!blProxy && config.sysProxyType == ESysProxyType.ForcedChange)
                {
                    bSysProxyType = true;
                    config.sysProxyType = ESysProxyType.ForcedClear;
                    SysProxyHandle.UpdateSysProxy(config, false);
                    Thread.Sleep(3000);
                }

                if (profileItems == null)
                {
                    profileItems = config.profileItems;
                }
                foreach (var item in profileItems)
                {
                    string indexId = item.indexId.TrimEx();
                    string url = item.url.TrimEx();
                    string userAgent = item.userAgent.TrimEx();
                    string groupId = item.groupId.TrimEx();
                    string hashCode = $"{item.remarks}->";
                    if (item.enabled == false || Utils.IsNullOrEmpty(indexId) || Utils.IsNullOrEmpty(url))
                    {
                        _updateFunc(false, $"{hashCode}{ResUI.MsgSkipSubscriptionUpdate}");
                        continue;
                    }

                    _updateFunc(false, $"{hashCode}{ResUI.MsgStartGettingSubscriptions}");

                    if (item.enableConvert)
                    {
                        if (Utils.IsNullOrEmpty(config.constItem.subConvertUrl))
                        {
                            config.constItem.subConvertUrl = Global.SubConvertUrls[0];
                        }
                        url = String.Format(config.constItem.subConvertUrl, Utils.UrlEncode(url));
                    }
                    var result = await (new DownloadHandle()).DownloadStringAsync(url, blProxy, userAgent);
                    if (blProxy && Utils.IsNullOrEmpty(result))
                    {
                        result = await (new DownloadHandle()).DownloadStringAsync(url, false, userAgent);
                    }

                    _updateFunc(false, $"{hashCode}{ResUI.MsgGetSubscriptionSuccessfully}");

                    if (Utils.IsNullOrEmpty(result))
                    {
                        _updateFunc(false, $"{hashCode}{ResUI.MsgSubscriptionDecodingFailed}");
                    }
                    else
                    {
                        int ret = ConfigHandler.AddBatchProfiles(ref config, result, indexId, groupId);
                        if (ret == 0)
                        {
                            item.updateTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                            _updateFunc(false, $"{hashCode}{ResUI.MsgUpdateSubscriptionEnd}");
                        }
                        else
                        {
                            _updateFunc(false, $"{hashCode}{ResUI.MsgFailedImportSubscription}");
                        }
                    }
                    _updateFunc(false, $"-------------------------------------------------------");

                }
                //restore system proxy
                if (bSysProxyType)
                {
                    config.sysProxyType = ESysProxyType.ForcedChange;
                    SysProxyHandle.UpdateSysProxy(config, false);
                }
                _updateFunc(true, $"{ResUI.MsgUpdateSubscriptionEnd}");
            });
        }


        public void UpdateGeoFile(string geoName, Config config, Action<bool, string> update)
        {
            _config = config;
            _updateFunc = update;
            var url = string.Format(Global.geoUrl, geoName);

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
                                //_updateFunc(true, "");
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
                var coreInfo = LazyConfig.Instance.GetCoreInfo(type);
                string url = coreInfo.coreLatestUrl;

                var result = await (new DownloadHandle()).UrlRedirectAsync(url, true);
                if (!Utils.IsNullOrEmpty(result))
                {
                    responseHandler(type, result);
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
                var coreInfo = LazyConfig.Instance.GetCoreInfo(type);
                string filePath = string.Empty;
                foreach (string name in coreInfo.coreExes)
                {
                    string vName = string.Format("{0}.exe", name);
                    vName = Utils.GetPath(vName);
                    if (File.Exists(vName))
                    {
                        filePath = vName;
                        break;
                    }
                }
                if (Utils.IsNullOrEmpty(filePath))
                {
                    string msg = string.Format(ResUI.NotFoundCore, @"");
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
                var coreInfo = LazyConfig.Instance.GetCoreInfo(type);

                string curVersion;
                string message;
                string url;
                if (type == ECoreType.clash)
                {
                    curVersion = getCoreVersion(type);
                    message = string.Format(ResUI.IsLatestCore, curVersion);
                    if (Environment.Is64BitProcess)
                    {
                        url = string.Format(coreInfo.coreDownloadUrl64, version);
                    }
                    else
                    {
                        url = string.Format(coreInfo.coreDownloadUrl32, version);
                    }
                }
                else if (type == ECoreType.clash_meta)
                {
                    curVersion = getCoreVersion(type);
                    message = string.Format(ResUI.IsLatestCore, curVersion);
                    if (Environment.Is64BitProcess)
                    {
                        url = string.Format(coreInfo.coreDownloadUrl64, version);
                    }
                    else
                    {
                        url = string.Format(coreInfo.coreDownloadUrl32, version);
                    }
                }
                else if (type == ECoreType.clashN)
                {
                    curVersion = FileVersionInfo.GetVersionInfo(Utils.GetExePath()).FileVersion.ToString();
                    message = string.Format(ResUI.IsLatestN, curVersion);
                    url = string.Format(coreInfo.coreDownloadUrl64, version);
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
