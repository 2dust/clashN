using System;
using System.Collections.Generic;
using clashN.Mode;
using System.Linq;
using static clashN.Mode.ClashProxies;

namespace clashN.Handler
{
    public sealed class LazyConfig
    {
        private static readonly Lazy<LazyConfig> _instance = new Lazy<LazyConfig>(() => new LazyConfig());
        private Config _config;
        private List<CoreInfo> coreInfos;
        private Dictionary<String, ProxiesItem> _proxies;

        public static LazyConfig Instance
        {
            get { return _instance.Value; }
        }
        public void SetConfig(ref Config config)
        {
            _config = config;
        }
        public Config GetConfig()
        {
            return _config;
        }

        public void SetProxies(Dictionary<String, ProxiesItem> proxies)
        {
            _proxies = proxies;
        }
        public Dictionary<String, ProxiesItem> GetProxies()
        {
            return _proxies;
        }

        public ECoreType GetCoreType(ProfileItem profileItem)
        {
            if (profileItem != null && profileItem.coreType != null)
            {
                return (ECoreType)profileItem.coreType;
            }
            return ECoreType.clash;

        }

        public CoreInfo GetCoreInfo(ECoreType coreType)
        {
            if (coreInfos == null)
            {
                InitCoreInfo();
            }
            return coreInfos.Where(t => t.coreType == coreType).FirstOrDefault();
        }

        private void InitCoreInfo()
        {
            coreInfos = new List<CoreInfo>();

            coreInfos.Add(new CoreInfo
            {
                coreType = ECoreType.clashN,
                coreUrl = Global.NUrl,
                coreLatestUrl = Global.NUrl + "/latest",
                coreDownloadUrl32 = Global.NUrl + "/download/{0}/clashN-32.zip",
                coreDownloadUrl64 = Global.NUrl + "/download/{0}/clashN.zip",
            });

            coreInfos.Add(new CoreInfo
            {
                coreType = ECoreType.clash,
                coreExes = new List<string> { "clash-windows-amd64-v3", "clash-windows-amd64", "clash-windows-386", "clash" },
                arguments = "-f config.yaml",
                coreUrl = Global.clashCoreUrl,
                coreLatestUrl = Global.clashCoreUrl + "/latest",
                coreDownloadUrl32 = Global.clashCoreUrl + "/download/{0}/clash-windows-386-{0}.zip",
                coreDownloadUrl64 = Global.clashCoreUrl + "/download/{0}/clash-windows-amd64-{0}.zip",
                match = "Clash"
            });

            coreInfos.Add(new CoreInfo
            {
                coreType = ECoreType.clash_meta,
                coreExes = new List<string> { "Clash.Meta-windows-amd64-compatible", "Clash.Meta-windows-amd64", "Clash.Meta-windows-386", "Clash.Meta", "clash" },
                arguments = "-f config.yaml",
                coreUrl = Global.clashMetaCoreUrl,
                coreLatestUrl = Global.clashMetaCoreUrl + "/latest",
                coreDownloadUrl32 = Global.clashMetaCoreUrl + "/download/{0}/Clash.Meta-windows-386-{0}.zip",
                coreDownloadUrl64 = Global.clashMetaCoreUrl + "/download/{0}/Clash.Meta-windows-amd64-compatible-{0}.zip",
                match = "Clash Meta"
            });

        }
    }
}
