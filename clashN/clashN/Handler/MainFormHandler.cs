using NHotkey;
using NHotkey.WindowsForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using clashN.Mode;
using System.Linq;
using clashN.Resx;
using static clashN.Mode.ClashProxies;
using clashN.Base;
using static clashN.Mode.ClashProviders;

namespace clashN.Handler
{
    public sealed class MainFormHandler
    {
        private static readonly Lazy<MainFormHandler> instance = new Lazy<MainFormHandler>(() => new MainFormHandler());
        public static MainFormHandler Instance
        {
            get { return instance.Value; }
        }
        public Icon GetNotifyIcon(Config config, Icon def)
        {
            try
            {
                int index = (int)config.sysProxyType;

                //Load from local file
                var fileName = Utils.GetPath($"NotifyIcon{index + 1}.ico");
                if (File.Exists(fileName))
                {
                    return new Icon(fileName);
                }
                switch (index)
                {
                    case 0:
                        return Properties.Resources.NotifyIcon1;
                    case 1:
                        return Properties.Resources.NotifyIcon2;
                    case 2:
                        return Properties.Resources.NotifyIcon3;
                }

                return Properties.Resources.NotifyIcon1;
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
                return def;
            }
        }

        public void BackupGuiNConfig(Config config, bool auto = false)
        {
            string fileName = $"guiNConfig_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff")}.json";
            if (auto)
            {
                fileName = Utils.GetBackupPath(fileName);
            }
            else
            {
                SaveFileDialog fileDialog = new SaveFileDialog
                {
                    FileName = fileName,
                    Filter = "guiNConfig|*.json",
                    FilterIndex = 2,
                    RestoreDirectory = true
                };
                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                fileName = fileDialog.FileName;
            }
            if (Utils.IsNullOrEmpty(fileName))
            {
                return;
            }
            var ret = Utils.ToJsonFile(config, fileName);
            if (!auto)
            {
                if (ret == 0)
                {

                    UI.Show(ResUI.OperationSuccess);
                }
                else
                {
                    UI.ShowWarning(ResUI.OperationFailed);
                }
            }
        }

        public void UpdateTask(Config config, Action<bool, string> update)
        {
            Task.Run(() => UpdateTaskRun(config, update));
        }

        private void UpdateTaskRun(Config config, Action<bool, string> update)
        {
            var autoUpdateSubTime = DateTime.Now;
            var autoUpdateGeoTime = DateTime.Now;

            Thread.Sleep(60000);
            Utils.SaveLog("UpdateTaskRun");

            var updateHandle = new UpdateHandle();
            while (true)
            {
                var dtNow = DateTime.Now;

                if (config.autoUpdateSubInterval > 0)
                {
                    if ((dtNow - autoUpdateSubTime).Hours % config.autoUpdateSubInterval == 0)
                    {
                        updateHandle.UpdateSubscriptionProcess(config, true, null, (bool success, string msg) =>
                        {
                            update(success, msg);
                            if (success)
                                Utils.SaveLog("subscription" + msg);
                        });
                        autoUpdateSubTime = dtNow;
                    }
                    Thread.Sleep(60000);
                }

                //if (config.autoUpdateInterval > 0)
                //{
                //    if ((dtNow - autoUpdateGeoTime).Hours % config.autoUpdateInterval == 0)
                //    {
                //        updateHandle.UpdateGeoFile("geosite", config, (bool success, string msg) =>
                //        {
                //            update(false, msg);
                //            if (success)
                //                Utils.SaveLog("geosite" + msg);
                //        });

                //        updateHandle.UpdateGeoFile("geoip", config, (bool success, string msg) =>
                //        {
                //            update(false, msg);
                //            if (success)
                //                Utils.SaveLog("geoip" + msg);
                //        });
                //        autoUpdateGeoTime = dtNow;
                //    }
                //}

                Thread.Sleep(1000 * 3600);
            }
        }

        public void RegisterGlobalHotkey(Config config, EventHandler<HotkeyEventArgs> handler, Action<bool, string> update)
        {
            if (config.globalHotkeys == null)
            {
                return;
            }

            foreach (var item in config.globalHotkeys)
            {
                if (item.KeyCode == null)
                {
                    continue;
                }

                Keys keys = (Keys)item.KeyCode;
                if (item.Control)
                {
                    keys |= Keys.Control;
                }
                if (item.Alt)
                {
                    keys |= Keys.Alt;
                }
                if (item.Shift)
                {
                    keys |= Keys.Shift;
                }

                try
                {
                    HotkeyManager.Current.AddOrReplace(((int)item.eGlobalHotkey).ToString(), keys, handler);
                    var msg = string.Format(ResUI.RegisterGlobalHotkeySuccessfully, $"{item.eGlobalHotkey.ToString()} = {keys}");
                    update(false, msg);
                }
                catch (Exception ex)
                {
                    var msg = string.Format(ResUI.RegisterGlobalHotkeyFailed, $"{item.eGlobalHotkey.ToString()} = {keys}", ex.Message);
                    update(false, msg);
                    Utils.SaveLog(msg);
                }
            }
        }

        public void GetClashProxies(Config config, Action<ClashProxies, ClashProviders> update)
        {
            Task.Run(() => GetClashProxiesAsync(config, update));
        }

        private async Task GetClashProxiesAsync(Config config, Action<ClashProxies, ClashProviders> update)
        {
            for (var i = 0; i < 5; i++)
            {
                var url = $"{Global.httpProtocol}{Global.Loopback}:{config.APIPort}/proxies";
                var result = await HttpClientHelper.GetInstance().GetAsync(url);
                var clashProxies = Utils.FromJson<ClashProxies>(result);

                var url2 = $"{Global.httpProtocol}{Global.Loopback}:{config.APIPort}/providers/proxies";
                var result2 = await HttpClientHelper.GetInstance().GetAsync(url2);
                var clashProviders = Utils.FromJson<ClashProviders>(result2);

                if (clashProxies != null || clashProviders != null)
                {
                    update(clashProxies, clashProviders);
                    return;
                }
                Thread.Sleep(5000);
            }
            update(null, null);
        }

        public void ClashProxiesDelayTest(Action<string> update)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (LazyConfig.Instance.GetProxies() == null)
                    {
                        Thread.Sleep(5000);
                        continue;
                    }
                }
                var proxies = LazyConfig.Instance.GetProxies();
                if (proxies == null)
                {
                    return;
                }
                var urlBase = $"{Global.httpProtocol}{Global.Loopback}:{LazyConfig.Instance.GetConfig().APIPort}/proxies";
                urlBase += @"/{0}/delay?timeout=10000&url=" + LazyConfig.Instance.GetConfig().constItem.speedPingTestUrl;

                List<Task> tasks = new List<Task>();
                foreach (KeyValuePair<string, ProxiesItem> kv in proxies)
                {
                    if (Global.notAllowTestType.Contains(kv.Value.type.ToLower()))
                    {
                        continue;
                    }
                    var name = kv.Value.name;
                    var url = string.Format(urlBase, name);
                    tasks.Add(Task.Run(() =>
                    {
                        var tt = HttpClientHelper.GetInstance().GetAsync(url);
                    }));
                }
                Task.WaitAll(tasks.ToArray());

                Thread.Sleep(5000);
                update("");
            });
        }

        public void ClashProxiesDelayTestPart(List<ProxiesItem> lstProxy, Action<ProxiesItem, string> update)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    if (LazyConfig.Instance.GetProxies() == null)
                    {
                        Thread.Sleep(5000);
                        continue;
                    }
                }
                var proxies = LazyConfig.Instance.GetProxies();
                if (proxies == null || lstProxy == null)
                {
                    return;
                }
                var urlBase = $"{Global.httpProtocol}{Global.Loopback}:{LazyConfig.Instance.GetConfig().APIPort}/proxies";
                urlBase += @"/{0}/delay?timeout=10000&url=" + LazyConfig.Instance.GetConfig().constItem.speedPingTestUrl;

                List<Task> tasks = new List<Task>();
                foreach (var it in lstProxy)
                {
                    if (Global.notAllowTestType.Contains(it.type.ToLower()))
                    {
                        continue;
                    }
                    var name = it.name;
                    var url = string.Format(urlBase, name);
                    tasks.Add(Task.Run(async () =>
                    {
                        var result = await HttpClientHelper.GetInstance().GetAsync(url);
                        update(it, result);
                    }));
                }
                Task.WaitAll(tasks.ToArray());
            });
        }

        public void InitRegister(Config config)
        {
            Task.Run(() =>
            {
                //URL Schemes
                Utils.RegWriteValue(Global.MyRegPathClasses, "", "URL:clash");
                Utils.RegWriteValue(Global.MyRegPathClasses, "URL Protocol", "");
                Utils.RegWriteValue($"{Global.MyRegPathClasses}\\shell\\open\\command", "", $"\"{Utils.GetExePath()}\" \"%1\"");

            });
        }

        public List<ProxiesItem> GetClashProxyGroups()
        {
            try
            {
                var fileContent = LazyConfig.Instance.ProfileContent;
                if (!fileContent.ContainsKey("proxy-groups"))
                {
                    return null;
                }
                return Utils.FromJson<List<ProxiesItem>>(Utils.ToJson(fileContent["proxy-groups"]));
            }
            catch (Exception ex)
            {
                Utils.SaveLog("GetClashProxyGroups", ex);
                return null;
            }
        }

        public async void ClashSetActiveProxy(string name, string nameNode)
        {
            var url = $"{Global.httpProtocol}{Global.Loopback}:{LazyConfig.Instance.GetConfig().APIPort}/proxies/{name}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("name", nameNode);
            await HttpClientHelper.GetInstance().PutAsync(url, headers);

        }

        public void ClashConfigUpdate(Dictionary<string, string> headers)
        {
            Task.Run(async () =>
            {
                var proxies = LazyConfig.Instance.GetProxies();
                if (proxies == null)
                {
                    return;
                }

                var urlBase = $"{Global.httpProtocol}{Global.Loopback}:{LazyConfig.Instance.GetConfig().APIPort}/configs";

                await HttpClientHelper.GetInstance().PatchAsync(urlBase, headers);
            });
        }

        public async void ClashConfigReload( string filePath)
        {
            var url = $"{Global.httpProtocol}{Global.Loopback}:{LazyConfig.Instance.GetConfig().APIPort}/configs";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("path", filePath);
            await HttpClientHelper.GetInstance().PutAsync(url, headers);

        }

    }
}