namespace ClashN
{
    internal class Global
    {
        #region 常量

        public const string AboutUrl = @"https://github.com/2dust/clashN";
        public const string UpdateUrl = AboutUrl + @"/releases";
        public const string NUrl = @"https://github.com/2dust/clashN/releases";
        public const string clashCoreUrl = "https://github.com/Dreamacro/clash/releases";
        public const string clashMetaCoreUrl = "https://github.com/MetaCubeX/Clash.Meta/releases";
        public const string geoUrl = "https://github.com/Loyalsoldier/v2ray-rules-dat/releases/latest/download/{0}.dat";

        /// <summary>
        /// SpeedTestUrl
        /// </summary>
        public const string SpeedTestUrl = @"http://cachefly.cachefly.net/10mb.test";

        public const string SpeedPingTestUrl = @"https://www.google.com/generate_204";

        public static readonly List<string> SubConvertUrls = new List<string> {
                @"https://sub.xeton.dev/sub?target=clash&url={0}",
                @"https://api.dler.io/sub?target=clash&url={0}",
                @"http://127.0.0.1:25500/sub?target=clash&url={0}",
                ""
            };

        public static readonly List<string> SubConvertConfig = new List<string> {
                @"https://raw.githubusercontent.com/ACL4SSR/ACL4SSR/master/Clash/config/ACL4SSR_Online.ini"
            };

        /// <summary>
        /// PromotionUrl
        /// </summary>
        public const string PromotionUrl = @"aHR0cHM6Ly85LjIzNDQ1Ni54eXovYWJjLmh0bWw=";

        /// <summary>
        /// 本软件配置文件名
        /// </summary>
        public const string ConfigFileName = "guiNConfig.json";

        /// <summary>
        /// 配置文件名
        /// </summary>
        public const string coreConfigFileName = "config.yaml";

        public const string mixinConfigFileName = "Mixin.yaml";

        public const string SampleMixin = "ClashN.Sample.SampleMixin.yaml";
        public const string SampleTun = "ClashN.Sample.SampleTun.yaml";

        public const string InboundSocks = "socks";
        public const string InboundHttp = "http";
        public const string Loopback = "127.0.0.1";

        /// <summary>
        /// http
        /// </summary>
        public const string httpProtocol = "http://";

        /// <summary>
        /// https
        /// </summary>
        public const string httpsProtocol = "https://";

        public const string clashProtocol = "clash://";

        /// <summary>
        /// MyRegPath
        /// </summary>
        public const string MyRegPath = "Software\\clashNGUI";

        /// <summary>
        /// Language
        /// </summary>
        public const string MyRegKeyLanguage = "CurrentLanguage";

        /// <summary>
        /// Font
        /// </summary>
        public const string MyRegKeyFont = "CurrentFont";

        /// <summary>
        /// URL Schemes
        /// </summary>
        public const string MyRegPathClasses = "SOFTWARE\\Classes\\clash";

        /// <summary>
        /// Icon
        /// </summary>
        public const string CustomIconName = "ClashN.ico";

        public const int MinFontSize = 10;

        public const string StatisticLogOverall = "StatisticLogOverall.json";

        public const string IEProxyExceptions = "localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;192.168.*";

        public static readonly List<string> IEProxyProtocols = new List<string> {
                        "{ip}:{http_port}",
                        "socks={ip}:{socks_port}",
                        "http={ip}:{http_port};https={ip}:{http_port};ftp={ip}:{http_port};socks={ip}:{socks_port}",
                        "http=http://{ip}:{http_port};https=http://{ip}:{http_port}",
                        ""
                    };

        public static readonly List<string> coreTypes = new List<string> { "Clash", "ClashPremium", "ClashMeta", };

        public static readonly List<string> allowSelectType = new List<string> { "selector", "urltest", "loadbalance", "fallback" };

        public static readonly List<string> notAllowTestType = new List<string> { "selector", "urltest", "direct", "reject", "compatible", "pass", "loadbalance", "fallback" };

        public static readonly List<string> proxyVehicleType = new List<string> { "file", "http" };

        public static readonly List<string> Languages = new List<string> { "zh-Hans", "en", "fa-IR" };

        public static readonly List<string> LogLevel = new List<string> { "debug", "info", "warning", "error", "silent" };

        #endregion 常量

        #region 全局变量

        /// <summary>
        /// 是否需要重启服务
        /// </summary>
        public static bool reloadCore
        {
            get; set;
        }

        public static Job processJob
        {
            get; set;
        }

        public static bool ShowInTaskbar
        {
            get; set;
        }

        #endregion 全局变量
    }
}