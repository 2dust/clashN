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
        private static string SampleTun = "clashN.Sample.SampleTun.yaml";

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
                File.Copy(addressFileName, fileName);

                //check again
                if (!File.Exists(fileName))
                {
                    msg = ResUI.FailedReadConfiguration + "2";
                    return -1;
                }

                var config = LazyConfig.Instance.GetConfig();
                var fileContent = Utils.FromYaml<Dictionary<string, object>>(File.ReadAllText(fileName));
                if (fileContent == null)
                {
                    msg = ResUI.FailedConversionConfiguration;
                    return -1;
                }
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

                //enable tun mode
                if (node.enableTun)
                {
                    string tun = Utils.GetEmbedText(SampleTun);
                    if (!Utils.IsNullOrEmpty(tun))
                    {
                        var tunContent = Utils.FromYaml<Dictionary<string, object>>(tun);
                        ModifyContent(fileContent, "tun", tunContent["tun"]);
                    }
                }

                File.WriteAllText(fileName, Utils.ToYaml(fileContent));

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
    }
}
