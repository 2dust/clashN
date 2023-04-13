using System.Drawing;

namespace ClashN.Mode
{
    /// <summary>
    /// 本软件配置文件实体类
    /// </summary>
    [Serializable]
    public class Config
    {
        #region property

        public int MixedPort { get; set; } = 7888;

        public int HttpPort { get; set; } = 7890;

        public int SocksPort { get; set; } = 7891;

        public int ApiPort { get; set; } = 9090;

        public string LogLevel { get; set; }

        public bool EnableIpv6 { get; set; }

        public string IndexId { get; set; }

        public SysProxyType SysProxyType { get; set; }

        public ERuleMode ruleMode { get; set; }

        public bool AllowLANConn { get; set; }

        public bool AutoRun { get; set; }

        public bool EnableStatistics { get; set; }

        public string SystemProxyExceptions { get; set; }
        public string SystemProxyAdvancedProtocol { get; set; }

        public int AutoUpdateSubInterval { get; set; } = 10;
        public int AutoDelayTestInterval { get; set; } = 10;

        public bool EnableSecurityProtocolTls13 { get; set; }

        public bool EnableMixinContent { get; set; }

        public int PacPort { get; set; }

        public bool AutoHideStartup { get; set; }

        public bool EnableTun { get; set; }

        #endregion property

        #region other entities

        public List<ProfileItem> ProfileItems { get; } = new List<ProfileItem>();

        public UIItem? UiItem { get; set; }

        public ConstItem? ConstItem { get; set; }

        public List<KeyShortcut> globalHotkeys { get; } = new List<KeyShortcut>();

        #endregion other entities

        #region function

        public int FindIndexId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return -1;
            }
            return ProfileItems.FindIndex(it => it.indexId == id);
        }

        public ProfileItem? GetProfileItem(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return ProfileItems.FirstOrDefault(it => it.indexId == id);
        }

        public bool IsActiveNode(ProfileItem item)
        {
            if (!string.IsNullOrEmpty(item.indexId) && item.indexId == IndexId)
            {
                return true;
            }

            return false;
        }

        #endregion function
    }

    [Serializable]
    public class ProfileItem
    {
        public ProfileItem()
        {
            indexId = string.Empty;
            sort = 0;
            url = string.Empty;
            enabled = true;
            address = string.Empty;
            remarks = string.Empty;
            testResult = string.Empty;
            groupId = string.Empty;
            enableConvert = false;
        }

        #region function

        public string GetSummary()
        {
            string summary = string.Format("{0}", remarks);
            return summary;
        }

        #endregion function

        public string indexId { get; set; }

        public int sort { get; set; }

        public string address { get; set; }

        public string remarks { get; set; }

        public string testResult { get; set; }

        public string groupId { get; set; } = string.Empty;
        public CoreKind? coreType { get; set; }

        public string url { get; set; }

        public bool enabled { get; set; } = true;

        public string userAgent { get; set; } = string.Empty;

        public bool enableConvert { get; set; }

        public long updateTime { get; set; }
        public ulong uploadRemote { get; set; }
        public ulong downloadRemote { get; set; }
        public ulong totalRemote { get; set; }
        public long expireRemote { get; set; }
    }

    [Serializable]
    public class UIItem
    {
        public Point mainLocation { get; set; }

        public double mainWidth { get; set; }
        public double mainHeight { get; set; }

        public bool colorModeDark { get; set; }
        public string? colorPrimaryName { get; set; }
        public string currentFontFamily { get; set; } = string.Empty;
        public int currentFontSize { get; set; }

        public int proxiesSorting { get; set; }
        public bool proxiesAutoRefresh { get; set; }

        public int connectionsSorting { get; set; }
        public bool connectionsAutoRefresh { get; set; }
    }

    [Serializable]
    public class ConstItem
    {
        public string subConvertUrl { get; set; } = string.Empty;
        public string speedTestUrl { get; set; } = string.Empty;
        public string speedPingTestUrl { get; set; } = string.Empty;
        public string defIEProxyExceptions { get; set; } = string.Empty;
    }
}