using ClashN.Mode;
using ClashN.Resx;
using System.IO;

namespace ClashN.Handler
{
    /// <summary>
    /// Core配置文件处理类
    /// </summary>
    internal class CoreConfigHandler
    {
        /// <summary>
        /// 生成配置文件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="fileName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static int GenerateClientConfig(ProfileItem node, string fileName, bool blExport, out string msg)
        {
            if (node == null)
            {
                msg = ResUI.CheckProfileSettings;
                return -1;
            }

            msg = ResUI.InitialConfiguration;

            try
            {
                //检查GUI设置
                if (node == null)
                {
                    msg = ResUI.CheckProfileSettings;
                    return -1;
                }

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                string addressFileName = node.address;
                if (string.IsNullOrEmpty(addressFileName))
                {
                    msg = ResUI.FailedGetDefaultConfiguration;
                    return -1;
                }
                if (!File.Exists(addressFileName))
                {
                    addressFileName = Path.Combine(Utils.GetConfigPath(), addressFileName);
                }
                if (!File.Exists(addressFileName))
                {
                    msg = ResUI.FailedReadConfiguration + "1";
                    return -1;
                }

                string tagYamlStr1 = "!<str>";
                string tagYamlStr2 = "__strn__";
                string tagYamlStr3 = "!!str";
                var config = LazyConfig.Instance.Config;
                var txtFile = File.ReadAllText(addressFileName);
                txtFile = txtFile.Replace(tagYamlStr1, tagYamlStr2);

                var fileContent = Utils.FromYaml<Dictionary<string, object>>(txtFile);
                if (fileContent == null)
                {
                    msg = ResUI.FailedConversionConfiguration;
                    return -1;
                }
                //mixed-port
                fileContent["mixed-port"] = config.MixedPort;
                //port
                fileContent["port"] = config.HttpPort;
                //socks-port
                fileContent["socks-port"] = config.SocksPort;
                //log-level
                fileContent["log-level"] = config.LogLevel;
                //external-controller
                fileContent["external-controller"] = $"{Global.Loopback}:{config.ApiPort}";
                //allow-lan
                if (config.AllowLANConn)
                {
                    fileContent["allow-lan"] = "true";
                    fileContent["bind-address"] = "*";
                }
                else
                {
                    fileContent["allow-lan"] = "false";
                }

                //ipv6
                fileContent["ipv6"] = config.EnableIpv6;

                //mode
                if (!fileContent.ContainsKey("mode"))
                {
                    fileContent["mode"] = ERuleMode.Rule.ToString().ToLower();
                }
                else
                {
                    if (config.ruleMode != ERuleMode.Unchanged)
                    {
                        fileContent["mode"] = config.ruleMode.ToString().ToLower();
                    }
                }

                //enable tun mode
                if (config.EnableTun)
                {
                    string tun = Utils.GetEmbedText(Global.SampleTun);
                    if (!string.IsNullOrEmpty(tun))
                    {
                        var tunContent = Utils.FromYaml<Dictionary<string, object>>(tun);
                        if (tunContent != null)
                            fileContent["tun"] = tunContent["tun"];
                    }
                }

                //Mixin
                try
                {
                    MixinContent(fileContent, config, node);
                }
                catch (Exception ex)
                {
                    Utils.SaveLog("GenerateClientCustomConfig-Mixin", ex);
                }

                var txtFileNew = Utils.ToYaml(fileContent).Replace(tagYamlStr2, tagYamlStr3);
                File.WriteAllText(fileName, txtFileNew);
                //check again
                if (!File.Exists(fileName))
                {
                    msg = ResUI.FailedReadConfiguration + "2";
                    return -1;
                }

                LazyConfig.Instance.ProfileContent = fileContent;

                msg = string.Format(ResUI.SuccessfulConfiguration, $"{node.GetSummary()}");
            }
            catch (Exception ex)
            {
                Utils.SaveLog("GenerateClientCustomConfig", ex);
                msg = ResUI.FailedGenDefaultConfiguration;
                return -1;
            }
            return 0;
        }

        private static void MixinContent(Dictionary<string, object> fileContent, Config config, ProfileItem node)
        {
            if (!config.EnableMixinContent)
            {
                return;
            }

            var path = Utils.GetConfigPath(Global.mixinConfigFileName);
            if (!File.Exists(path))
            {
                return;
            }

            var txtFile = File.ReadAllText(Utils.GetConfigPath(Global.mixinConfigFileName));
            //txtFile = txtFile.Replace("!<str>", "");

            var mixinContent = Utils.FromYaml<Dictionary<string, object>>(txtFile);
            if (mixinContent == null)
            {
                return;
            }
            foreach (var item in mixinContent)
            {
                if (!config.EnableTun && item.Key == "tun")
                {
                    continue;
                }

                if (item.Key.StartsWith("prepend-")
                    || item.Key.StartsWith("append-")
                    || item.Key.StartsWith("removed-"))
                {
                    ModifyContentMerge(fileContent, item.Key, item.Value);
                }
                else
                {
                    fileContent[item.Key] = item.Value;
                }
            }
            return;
        }

        private static void ModifyContentMerge(Dictionary<string, object> fileContent, string key, object value)
        {
            bool blPrepend = false;
            bool blRemoved = false;
            if (key.StartsWith("prepend-"))
            {
                blPrepend = true;
                key = key.Replace("prepend-", "");
            }
            else if (key.StartsWith("append-"))
            {
                blPrepend = false;
                key = key.Replace("append-", "");
            }
            else if (key.StartsWith("removed-"))
            {
                blRemoved = true;
                key = key.Replace("removed-", "");
            }
            else
            {
                return;
            }

            if (!blRemoved && !fileContent.ContainsKey(key))
            {
                fileContent.Add(key, value);
                return;
            }
            var lstOri = (List<object>)fileContent[key];
            var lstValue = (List<object>)value;

            if (blRemoved)
            {
                foreach (var item in lstValue)
                {
                    lstOri.RemoveAll(t => t.ToString().StartsWith(item.ToString()));
                }
                return;
            }

            if (blPrepend)
            {
                lstValue.Reverse();
                foreach (var item in lstValue)
                {
                    lstOri.Insert(0, item);
                }
            }
            else
            {
                foreach (var item in lstValue)
                {
                    lstOri.Add(item);
                }
            }
        }
    }
}