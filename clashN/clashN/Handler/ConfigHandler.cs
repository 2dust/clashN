using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using clashN.Mode;
using clashN.Base;
using System.Linq;
using clashN.Tool;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace clashN.Handler
{
    /// <summary>
    /// 本软件配置文件处理类
    /// </summary>
    class ConfigHandler
    {
        private static string configRes = Global.ConfigFileName;
        private static object objLock = new object();

        #region ConfigHandler

        /// <summary>
        /// 载入配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int LoadConfig(ref Config config)
        {
            //载入配置文件 
            string result = Utils.LoadResource(Utils.GetPath(configRes));
            if (!Utils.IsNullOrEmpty(result))
            {
                //转成Json
                config = Utils.FromJson<Config>(result);
            }
            else
            {
                if (File.Exists(Utils.GetPath(configRes)))
                {
                    Utils.SaveLog("LoadConfig Exception");
                    return -1;
                }
            }

            if (config == null)
            {
                config = new Config
                {
                    logLevel = "warning",
                    profileItems = new List<ProfileItem>(),

                    // 默认不开启统计
                    enableStatistics = false,
                };
            }

            //本地监听
            if (config.mixedPort == 0)
            {
                config.mixedPort = 7888;
            }
            if (config.httpPort == 0)
            {
                config.httpPort = 7890;
            }
            if (config.socksPort == 0)
            {
                config.socksPort = 7891;
            }
            if (config.APIPort == 0)
            {
                config.APIPort = 9090;
            }

            if (config.profileItems == null)
            {
                config.profileItems = new List<ProfileItem>();
            }

            if (config.uiItem == null)
            {
                config.uiItem = new UIItem()
                {
                };
            }
            if (config.uiItem.mainLvColWidth == null)
            {
                config.uiItem.mainLvColWidth = new Dictionary<string, int>();
            }

            if (config.constItem == null)
            {
                config.constItem = new ConstItem();
            }
            //if (Utils.IsNullOrEmpty(config.constItem.subConvertUrl))
            //{
            //    config.constItem.subConvertUrl = Global.SubConvertUrl;
            //}
            if (Utils.IsNullOrEmpty(config.constItem.speedTestUrl))
            {
                config.constItem.speedTestUrl = Global.SpeedTestUrl;
            }
            if (Utils.IsNullOrEmpty(config.constItem.speedPingTestUrl))
            {
                config.constItem.speedPingTestUrl = Global.SpeedPingTestUrl;
            }
            if (Utils.IsNullOrEmpty(config.constItem.defIEProxyExceptions))
            {
                config.constItem.defIEProxyExceptions = Global.IEProxyExceptions;
            }

            if (config == null
                || config.profileItems.Count <= 0
                )
            {
                Global.reloadCore = false;
            }
            else
            {
                Global.reloadCore = true;

                for (int i = 0; i < config.profileItems.Count; i++)
                {
                    ProfileItem profileItem = config.profileItems[i];

                    if (Utils.IsNullOrEmpty(profileItem.indexId))
                    {
                        profileItem.indexId = Utils.GetGUID(false);
                    }
                }
            }

            LazyConfig.Instance.SetConfig(ref config);
            return 0;
        }
        /// <summary>
        /// 保参数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int SaveConfig(ref Config config, bool reload = true)
        {
            Global.reloadCore = reload;

            ToJsonFile(config);

            return 0;
        }

        /// <summary>
        /// 存储文件
        /// </summary>
        /// <param name="config"></param>
        private static void ToJsonFile(Config config)
        {
            lock (objLock)
            {
                try
                {
                    //save temp file
                    var resPath = Utils.GetPath(configRes);
                    var tempPath = $"{resPath}_temp";
                    if (Utils.ToJsonFile(config, tempPath) != 0)
                    {
                        return;
                    }

                    if (File.Exists(resPath))
                    {
                        File.Delete(resPath);
                    }
                    //rename
                    File.Move(tempPath, resPath);
                }
                catch (Exception ex)
                {
                    Utils.SaveLog("ToJsonFile", ex);
                }
            }
        }

        #endregion

        #region Profile

        /// <summary>
        /// 移除配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="indexs"></param>
        /// <returns></returns>
        public static int RemoveProfile(Config config, List<ProfileItem> indexs)
        {
            foreach (var item in indexs)
            {
                var index = config.FindIndexId(item.indexId);
                if (index >= 0)
                {
                    RemoveProfileItem(config, index);
                }
            }

            ToJsonFile(config);

            return 0;
        }

        /// <summary>
        /// 克隆配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int CopyProfile(ref Config config, List<ProfileItem> indexs)
        {
            foreach (var item in indexs)
            {
                ProfileItem profileItem = Utils.DeepCopy(item);
                profileItem.indexId = string.Empty;
                profileItem.remarks = string.Format("{0}-clone", item.remarks);

                if (string.IsNullOrEmpty(profileItem.address) || !File.Exists(Utils.GetConfigPath(profileItem.address)))
                {
                    profileItem.address = string.Empty;
                    AddProfileCommon(ref config, profileItem);
                }
                else
                {
                    var fileName = Utils.GetConfigPath(profileItem.address);
                    profileItem.address = string.Empty;
                    AddProfileViaPath(ref config, profileItem, fileName);
                }
            }

            ToJsonFile(config);

            return 0;
        }

        /// <summary>
        /// 设置活动配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int SetDefaultProfile(ref Config config, ProfileItem item)
        {
            if (item == null)
            {
                return -1;
            }

            config.indexId = item.indexId;
            Global.reloadCore = true;

            ToJsonFile(config);

            return 0;
        }

        public static int SetDefaultProfile(Config config, List<ProfileItem> lstProfile)
        {
            if (lstProfile.Exists(t => t.indexId == config.indexId))
            {
                return 0;
            }
            if (config.profileItems.Exists(t => t.indexId == config.indexId))
            {
                return 0;
            }
            if (lstProfile.Count > 0)
            {
                return SetDefaultProfile(ref config, lstProfile[0]);
            }
            if (config.profileItems.Count > 0)
            {
                return SetDefaultProfile(ref config, config.profileItems[0]);
            }
            return -1;
        }
        public static ProfileItem GetDefaultProfile(ref Config config)
        {
            if (config.profileItems.Count <= 0)
            {
                return null;
            }
            var index = config.FindIndexId(config.indexId);
            if (index < 0)
            {
                SetDefaultProfile(ref config, config.profileItems[0]);
                return config.profileItems[0];
            }

            return config.profileItems[index];
        }

        /// <summary>
        /// 移动配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="lstProfile"></param>
        /// <param name="index"></param>
        /// <param name="eMove"></param>
        /// <returns></returns>
        public static int MoveProfile(ref Config config, ref List<ProfileItem> lstProfile, int index, EMove eMove, int pos = -1)
        {
            int count = lstProfile.Count;
            if (index < 0 || index > lstProfile.Count - 1)
            {
                return -1;
            }

            for (int i = 0; i < lstProfile.Count; i++)
            {
                lstProfile[i].sort = (i + 1) * 10;
            }

            switch (eMove)
            {
                case EMove.Top:
                    {
                        if (index == 0)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[0].sort - 1;

                        break;
                    }
                case EMove.Up:
                    {
                        if (index == 0)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[index - 1].sort - 1;

                        break;
                    }

                case EMove.Down:
                    {
                        if (index == count - 1)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[index + 1].sort + 1;

                        break;
                    }
                case EMove.Bottom:
                    {
                        if (index == count - 1)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[lstProfile.Count - 1].sort + 1;

                        break;
                    }
                case EMove.Position:
                    lstProfile[index].sort = pos * 10 + 1;
                    break;
            }

            ToJsonFile(config);

            return 0;
        }

        public static int AddProfileViaContent(ref Config config, ProfileItem profileItem, string content)
        {
            if (Utils.IsNullOrEmpty(content))
            {
                return -1;
            }

            string newFileName = profileItem.address;
            if (Utils.IsNullOrEmpty(newFileName))
            {
                var ext = ".yaml";
                newFileName = string.Format("{0}{1}", Utils.GetGUID(), ext);
                profileItem.address = newFileName;
            }
            if (Utils.IsNullOrEmpty(profileItem.remarks))
            {
                profileItem.remarks = "clash_local_file";
            }

            try
            {
                File.WriteAllText(Path.Combine(Utils.GetConfigPath(), newFileName), content);
            }
            catch
            {
                return -1;
            }

            if (Utils.IsNullOrEmpty(profileItem.remarks))
            {
                profileItem.remarks = string.Format("import custom@{0}", DateTime.Now.ToShortDateString());
            }
            profileItem.enabled = true;
            AddProfileCommon(ref config, profileItem);

            ToJsonFile(config);

            return 0;
        }

        public static int AddProfileViaPath(ref Config config, ProfileItem profileItem, string fileName)
        {
            if (!File.Exists(fileName))
            {
                return -1;
            }
            var ext = Path.GetExtension(fileName);
            string newFileName = string.Format("{0}{1}", Utils.GetGUID(), ext);

            try
            {
                File.Copy(fileName, Path.Combine(Utils.GetConfigPath(), newFileName));
                if (!Utils.IsNullOrEmpty(profileItem.address))
                {
                    File.Delete(Path.Combine(Utils.GetConfigPath(), profileItem.address));
                }
            }
            catch
            {
                return -1;
            }

            profileItem.address = newFileName;
            if (Utils.IsNullOrEmpty(profileItem.remarks))
            {
                profileItem.remarks = string.Format("import custom@{0}", DateTime.Now.ToShortDateString());
            }

            AddProfileCommon(ref config, profileItem);

            ToJsonFile(config);

            return 0;
        }

        public static int EditProfile(ref Config config, ProfileItem profileItem)
        {
            if (!Utils.IsNullOrEmpty(profileItem.indexId) && config.indexId == profileItem.indexId)
            {
                Global.reloadCore = true;
            }

            AddProfileCommon(ref config, profileItem);

            //TODO auto update via url 
            //if (!Utils.IsNullOrEmpty(profileItem.url))
            //{
            //    var httpClient = new HttpClient();
            //    string result = httpClient.GetStringAsync(profileItem.url).Result;
            //    httpClient.Dispose();
            //    int ret = AddBatchProfiles(ref config, result, profileItem.indexId, profileItem.groupId);
            //}

            ToJsonFile(config);

            return 0;
        }

        public static int SortProfiles(ref Config config, ref List<ProfileItem> lstProfile, EProfileColName name, bool asc)
        {
            if (lstProfile.Count <= 0)
            {
                return -1;
            }
            var propertyName = string.Empty;
            switch (name)
            {
                case EProfileColName.remarks:
                case EProfileColName.url:
                case EProfileColName.testResult:
                case EProfileColName.updateTime:
                    propertyName = name.ToString();
                    break;
                default:
                    return -1;
            }

            var items = lstProfile.AsQueryable();

            if (asc)
            {
                lstProfile = items.OrderBy(propertyName).ToList();
            }
            else
            {
                lstProfile = items.OrderByDescending(propertyName).ToList();
            }
            for (int i = 0; i < lstProfile.Count; i++)
            {
                lstProfile[i].sort = (i + 1) * 10;
            }

            ToJsonFile(config);
            return 0;
        }

        public static int AddProfileCommon(ref Config config, ProfileItem profileItem)
        {
            if (Utils.IsNullOrEmpty(profileItem.indexId))
            {
                profileItem.indexId = Utils.GetGUID(false);
            }
            if (!config.profileItems.Exists(it => it.indexId == profileItem.indexId))
            {
                var maxSort = config.profileItems.Any() ? config.profileItems.Max(t => t.sort) : 0;
                profileItem.sort = maxSort++;

                config.profileItems.Add(profileItem);
            }

            return 0;
        }

        private static int RemoveProfileItem(Config config, int index)
        {
            try
            {
                {
                    File.Delete(Utils.GetConfigPath(config.profileItems[index].address));
                }
            }
            catch (Exception ex)
            {
                Utils.SaveLog("RemoveProfileItem", ex);
            }
            config.profileItems.RemoveAt(index);

            return 0;
        }

        public static string GetProfileContent(ProfileItem item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            if (Utils.IsNullOrEmpty(item.address))
            {
                return string.Empty;
            }
            var content = File.ReadAllText(Utils.GetConfigPath(item.address));

            return content;
        }

        public static int AddBatchProfiles(ref Config config, string clipboardData, string indexId, string groupId)
        {
            if (Utils.IsNullOrEmpty(clipboardData))
            {
                return -1;
            }

            //maybe url
            if (Utils.IsNullOrEmpty(indexId) && (clipboardData.StartsWith(Global.httpsProtocol) || clipboardData.StartsWith(Global.httpProtocol)))
            {
                ProfileItem item = new ProfileItem()
                {
                    groupId = groupId,
                    url = clipboardData,
                    coreType = ECoreType.clash,
                    address = string.Empty,
                    enabled = true,
                    remarks = "clash_subscription"
                };

                return EditProfile(ref config, item);
            }

            //maybe clashProtocol
            if (Utils.IsNullOrEmpty(indexId) && (clipboardData.StartsWith(Global.clashProtocol)))
            {
                Uri url = new Uri(clipboardData);
                if (url.Host == "install-config")
                {
                    var query = HttpUtility.ParseQueryString(url.Query);
                    if (!Utils.IsNullOrEmpty(query["url"] ?? ""))
                    {
                        ProfileItem item = new ProfileItem()
                        {
                            groupId = groupId,
                            url = query["url"],
                            coreType = ECoreType.clash,
                            address = string.Empty,
                            enabled = true,
                            remarks = "clash_subscription"
                        };

                        return EditProfile(ref config, item);
                    }
                }
            }

            //maybe file
            if (File.Exists(clipboardData))
            {
                ProfileItem item = new ProfileItem()
                {
                    groupId = groupId,
                    url = "",
                    coreType = ECoreType.clash,
                    address = string.Empty,
                    enabled = false,
                    remarks = "clash_local_file"
                };
                return AddProfileViaPath(ref config, item, clipboardData);
            }

            //Is Clash configuration
            if (clipboardData.IndexOf("port") >= 0
              && clipboardData.IndexOf("socks-port") >= 0
              && clipboardData.IndexOf("proxies") >= 0)
            { }
            else { return -1; }

            ProfileItem profileItem;
            if (!Utils.IsNullOrEmpty(indexId))
            {
                profileItem = config.GetProfileItem(indexId);
            }
            else
            {
                profileItem = new ProfileItem();
            }
            profileItem.groupId = groupId;

            if (AddProfileViaContent(ref config, profileItem, clipboardData) == 0)
            {
                return 0;

            }
            else
            {
                return -1;
            }
        }


        #endregion



        #region UI

        public static int AddformMainLvColWidth(ref Config config, string name, int width)
        {
            if (config.uiItem.mainLvColWidth == null)
            {
                config.uiItem.mainLvColWidth = new Dictionary<string, int>();
            }
            if (config.uiItem.mainLvColWidth.ContainsKey(name))
            {
                config.uiItem.mainLvColWidth[name] = width;
            }
            else
            {
                config.uiItem.mainLvColWidth.Add(name, width);
            }

            ToJsonFile(config);
            return 0;
        }
        public static int GetformMainLvColWidth(ref Config config, string name, int width)
        {
            if (config.uiItem.mainLvColWidth == null)
            {
                config.uiItem.mainLvColWidth = new Dictionary<string, int>();
            }
            if (config.uiItem.mainLvColWidth.ContainsKey(name))
            {
                return config.uiItem.mainLvColWidth[name];
            }
            else
            {
                return width;
            }
        }

        #endregion

    }
}
