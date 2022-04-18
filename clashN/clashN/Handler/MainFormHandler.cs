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
                Color color = ColorTranslator.FromHtml("#3399CC");
                int index = (int)config.sysProxyType;
                if (index > 0)
                {
                    color = (new Color[] { Color.Red, Color.Purple, Color.DarkGreen, Color.Orange, Color.DarkSlateBlue, Color.RoyalBlue })[index - 1];
                    //color = ColorTranslator.FromHtml(new string[] { "#CC0066", "#CC6600", "#99CC99", "#666699" }[index - 1]);
                }

                int width = 128;
                int height = 128;

                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                SolidBrush drawBrush = new SolidBrush(color);

                var customIcon = false;

                if (!customIcon)
                {
                    graphics.FillEllipse(drawBrush, new Rectangle(0, 0, width, height));
                    int zoom = 16;
                    graphics.DrawImage(new Bitmap(Properties.Resources.notify, width - zoom, width - zoom), zoom / 2, zoom / 2);
                }

                Icon createdIcon = Icon.FromHandle(bitmap.GetHicon());

                drawBrush.Dispose();
                graphics.Dispose();
                bitmap.Dispose();

                return createdIcon;
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
                        updateHandle.UpdateSubscriptionProcess(config, true, (bool success, string msg) =>
                        {
                            update(success, msg);
                            if (success)
                                Utils.SaveLog("subscription" + msg);
                        });
                        autoUpdateSubTime = dtNow;
                    }
                    Thread.Sleep(60000);
                }

                if (config.autoUpdateInterval > 0)
                {
                    if ((dtNow - autoUpdateGeoTime).Hours % config.autoUpdateInterval == 0)
                    {
                        updateHandle.UpdateGeoFile("geosite", config, (bool success, string msg) =>
                        {
                            update(false, msg);
                            if (success)
                                Utils.SaveLog("geosite" + msg);
                        });

                        updateHandle.UpdateGeoFile("geoip", config, (bool success, string msg) =>
                        {
                            update(false, msg);
                            if (success)
                                Utils.SaveLog("geoip" + msg);
                        });
                        autoUpdateGeoTime = dtNow;
                    }
                }

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

        public void GetClashProxies(Config config, Action<ClashProxies> update)
        {
            Task.Run(() => GetClashProxiesAsync(config, update));
        }

        private async Task GetClashProxiesAsync(Config config, Action<ClashProxies> update)
        {
            var url = $"{Global.httpProtocol}{Global.Loopback}:{config.APIPort}/proxies";
            var result = await Base.HttpClientHelper.GetInstance().GetAsync(url);

            var clashProxies = Utils.FromJson<ClashProxies>(result);

            update(clashProxies);
        }

        public void ClashProxiesDelayTest(Action<string> update)
        {
            Task.Run(() =>
            {
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
                    if (kv.Value.type == "Selector"
                        || kv.Value.type == "URLTest"
                        || kv.Value.type == "Direct"
                        || kv.Value.type == "Reject"
                    )
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
    }
}