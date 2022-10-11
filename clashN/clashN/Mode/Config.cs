using clashN.Base;
using System.Drawing;
using System.Windows.Forms;

namespace clashN.Mode
{
    /// <summary>
    /// 本软件配置文件实体类
    /// </summary>
    [Serializable]
    public class Config
    {
        #region property
        public int mixedPort { get; set; }

        public int httpPort { get; set; }

        public int socksPort { get; set; }

        public int APIPort { get; set; }

        public string logLevel { get; set; }

        public bool enableIpv6 { get; set; }

        public string indexId { get; set; }

        public ESysProxyType sysProxyType { get; set; }

        public ERuleMode ruleMode { get; set; }

        public bool allowLANConn { get; set; }

        public bool enableStatistics { get; set; }

        public string systemProxyExceptions { get; set; }
        public string systemProxyAdvancedProtocol { get; set; }

        public int autoUpdateSubInterval { get; set; } = 10;
        public int autoDelayTestInterval { get; set; } = 10;

        public bool enableSecurityProtocolTls13 { get; set; }

        public bool enableMixinContent { get; set; }

        #endregion

        #region other entities

        public List<ProfileItem> profileItems
        {
            get; set;
        }

        public UIItem uiItem
        {
            get; set;
        }

        public ConstItem constItem
        {
            get; set;
        }

        public List<KeyEventItem> globalHotkeys
        {
            get; set;
        }

        #endregion

        #region function         

        public int FindIndexId(string id)
        {
            if (Utils.IsNullOrEmpty(id))
            {
                return -1;
            }
            return profileItems.FindIndex(it => it.indexId == id);
        }

        public ProfileItem GetProfileItem(string id)
        {
            if (Utils.IsNullOrEmpty(id))
            {
                return null;
            }
            return profileItems.FirstOrDefault(it => it.indexId == id);
        }

        public bool IsActiveNode(ProfileItem item)
        {
            if (!Utils.IsNullOrEmpty(item.indexId) && item.indexId == indexId)
            {
                return true;
            }

            return false;
        }

        #endregion

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
            enableTun = false;
            enableConvert = false;
        }

        #region function
        public string GetSummary()
        {
            string summary = string.Format("{0}", remarks);
            return summary;
        }

        public void SetTestResult(string value)
        {
            testResult = value;
        }
        public string GetUpdateTime()
        {
            if (updateTime <= 0)
            {
                return String.Empty;
            }
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTime.AddSeconds(updateTime).ToLocalTime().ToString("MM/dd HH:mm");
        }

        public bool HasUrl
        {
            get { return !url.IsNullOrEmpty(); }
        }
        public bool HasAddress
        {
            get { return !address.IsNullOrEmpty(); }
        }
        public string StrUpdateTime
        {
            get
            {
                if (updateTime <= 0)
                {
                    return String.Empty;
                }
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dateTime.AddSeconds(updateTime).ToLocalTime().ToString("MM-dd HH:mm");
            }
        }
        public string TrafficUsed
        {
            get { return Utils.HumanFy(uploadRemote + downloadRemote); }
        }
        public string TrafficTotal
        {
            get { return totalRemote <= 0 ? "∞" : Utils.HumanFy(totalRemote); }
        }
        public string StrExpireTime
        {
            get
            {
                if (expireRemote <= 0)
                {
                    return String.Empty;
                }
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dateTime.AddSeconds(expireRemote).ToLocalTime().ToString("yyyy-MM-dd");
            }
        }


        #endregion

        public string indexId
        {
            get; set;
        }

        public int sort
        {
            get; set;
        }

        public string address
        {
            get; set;
        }

        public string remarks
        {
            get; set;
        }

        public string testResult
        {
            get; set;
        }

        public string groupId
        {
            get; set;
        } = string.Empty;
        public ECoreType? coreType
        {
            get; set;
        }

        public string url
        {
            get; set;
        }

        public bool enabled { get; set; } = true;

        public string userAgent
        {
            get; set;
        } = string.Empty;

        public bool enableTun { get; set; }

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

    }

    [Serializable]
    public class ConstItem
    {
        public string subConvertUrl
        {
            get; set;
        }
        public string speedTestUrl
        {
            get; set;
        }
        public string speedPingTestUrl
        {
            get; set;
        }
        public string defIEProxyExceptions
        {
            get; set;
        }
    }

    [Serializable]
    public class KeyEventItem
    {
        public EGlobalHotkey eGlobalHotkey { get; set; }

        public bool Alt { get; set; }

        public bool Control { get; set; }

        public bool Shift { get; set; }

        public Keys? KeyCode { get; set; }
    }
}
