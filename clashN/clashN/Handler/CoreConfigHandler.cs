using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using clashN.Base;
using clashN.Mode;
using clashN.Resx;

namespace clashN.Handler
{
    /// <summary>
    /// Core配置文件处理类
    /// </summary>
    class CoreConfigHandler
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
                if (Utils.IsNullOrEmpty(addressFileName))
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

                var config = LazyConfig.Instance.GetConfig();
                var txtFile = File.ReadAllText(addressFileName);
                txtFile = txtFile.Replace("!<str>", "");

                var fileContent = Utils.FromYaml<Dictionary<string, object>>(txtFile);
                if (fileContent == null)
                {
                    msg = ResUI.FailedConversionConfiguration;
                    return -1;
                }
                //mixed-port
                ModifyContent(fileContent, "mixed-port", config.mixedPort);
                //port
                ModifyContent(fileContent, "port", config.httpPort);
                //socks-port
                ModifyContent(fileContent, "socks-port", config.socksPort);
                //log-level
                ModifyContent(fileContent, "log-level", config.logLevel);
                //external-controller
                ModifyContent(fileContent, "external-controller", $"{Global.Loopback}:{config.APIPort}");
                //allow-lan
                if (config.allowLANConn)
                {
                    ModifyContent(fileContent, "allow-lan", "true");
                    ModifyContent(fileContent, "bind-address", "*");
                }
                else
                {
                    ModifyContent(fileContent, "allow-lan", "false");
                }

                //ipv6
                ModifyContent(fileContent, "ipv6", config.enableIpv6);

                //mode
                if (!fileContent.ContainsKey("mode"))
                {
                    ModifyContent(fileContent, "mode", ERuleMode.Rule.ToString().ToLower());
                }
                else
                {
                    if (config.ruleMode != ERuleMode.Unchanged)
                    {
                        ModifyContent(fileContent, "mode", config.ruleMode.ToString().ToLower());
                    }
                }

                //enable tun mode
                if (node.enableTun)
                {
                    string tun = Utils.GetEmbedText(Global.SampleTun);
                    if (!Utils.IsNullOrEmpty(tun))
                    {
                        var tunContent = Utils.FromYaml<Dictionary<string, object>>(tun);
                        ModifyContent(fileContent, "tun", tunContent["tun"]);
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

                File.WriteAllText(fileName, Utils.ToYaml(fileContent));
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
            if (!config.enableMixinContent)
            {
                return;
            }

            var path = Utils.GetPath(Global.mixinConfigFileName);
            if (!File.Exists(path))
            {
                return;
            }

            var txtFile = File.ReadAllText(Utils.GetPath(Global.mixinConfigFileName));
            txtFile = txtFile.Replace("!<str>", "");

            var mixinContent = Utils.FromYaml<Dictionary<string, object>>(txtFile);
            if (mixinContent == null)
            {
                return;
            }
            foreach (var item in mixinContent)
            {
                if (!node.enableTun && item.Key == "tun")
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
                    ModifyContent(fileContent, item.Key, item.Value);
                }
            }
            return;
        }

        private static void ModifyContent(Dictionary<string, object> fileContent, string key, object value)
        {
            if (fileContent.ContainsKey(key))
            {
                fileContent[key] = value;
            }
            else
            {
                fileContent.Add(key, value);
            }
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

            if (!fileContent.ContainsKey(key))
            {
                fileContent.Add(key, value);
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
