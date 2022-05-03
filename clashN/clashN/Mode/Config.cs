using System;
using System.Collections.Generic;
using System.Windows.Forms;
using clashN.Base;
using System.Linq;
using System.Drawing;

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

        public string indexId { get; set; }

        public ESysProxyType sysProxyType { get; set; }

        public bool allowLANConn { get; set; }

        public bool enableStatistics { get; set; }

        public bool keepOlderDedupl { get; set; }

        public bool ignoreGeoUpdateCore { get; set; }

        public string systemProxyExceptions { get; set; }

        public int autoUpdateInterval { get; set; } = 0;
        public int autoUpdateSubInterval { get; set; } = 0;
        
        public bool enableSecurityProtocolTls13 { get; set; }

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
            if (string.IsNullOrEmpty(id))
            {
                return -1;
            }
            return profileItems.FindIndex(it => it.indexId == id);
        }

        public ProfileItem GetProfileItem(string id)
        {
            if (string.IsNullOrEmpty(id))
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
    }

    [Serializable]
    public class UIItem
    {
        public Point mainLocation { get; set; }

        public Size mainSize
        {
            get; set;
        }

        public Dictionary<string, int> mainLvColWidth
        {
            get; set;
        }
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
