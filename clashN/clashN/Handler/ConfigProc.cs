using ClashN.Mode;
using ClashN.Tool;
using System.Collections.Specialized;
using System.IO;
using System.Web;

namespace ClashN.Handler
{
    /// <summary>
    /// 本软件配置文件处理类
    /// </summary>
    internal class ConfigProc
    {
        private static string configRes = Global.ConfigFileName;
        private static readonly object objLock = new object();

        #region ConfigHandler

        /// <summary>
        /// 载入配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int LoadConfig(ref Config? config)
        {
            //载入配置文件
            string result = Utils.LoadResource(Utils.GetConfigPath(configRes));
            if (!string.IsNullOrEmpty(result))
            {
                //转成Json
                config = Utils.FromJson<Config>(result);
            }
            else
            {
                if (File.Exists(Utils.GetConfigPath(configRes)))
                {
                    Utils.SaveLog("LoadConfig Exception");
                    return -1;
                }
            }

            if (config == null)
            {
                config = new Config
                {
                    LogLevel = "warning",
                    EnableStatistics = true,
                };
            }

            //本地监听
            if (config.MixedPort == 0)
                config.MixedPort = 7888;

            if (config.HttpPort == 0)
                config.HttpPort = 7890;

            if (config.SocksPort == 0)
                config.SocksPort = 7891;

            if (config.ApiPort == 0)
                config.ApiPort = 9090;

            if (config.PacPort == 0)
            {
                config.PacPort = 7990;
            }

            if (config.UiItem == null)
            {
                config.UiItem = new UIItem()
                {
                };
            }

            if (config.ConstItem == null)
            {
                config.ConstItem = new ConstItem();
            }
            //if (string.IsNullOrEmpty(config.constItem.subConvertUrl))
            //{
            //    config.constItem.subConvertUrl = Global.SubConvertUrl;
            //}
            if (string.IsNullOrEmpty(config.ConstItem.speedTestUrl))
            {
                config.ConstItem.speedTestUrl = Global.SpeedTestUrl;
            }
            if (string.IsNullOrEmpty(config.ConstItem.speedPingTestUrl))
            {
                config.ConstItem.speedPingTestUrl = Global.SpeedPingTestUrl;
            }
            if (string.IsNullOrEmpty(config.ConstItem.defIEProxyExceptions))
            {
                config.ConstItem.defIEProxyExceptions = Global.IEProxyExceptions;
            }

            if (config.ProfileItems.Count <= 0)
            {
                Global.reloadCore = false;
            }
            else
            {
                Global.reloadCore = true;

                for (int i = 0; i < config.ProfileItems.Count; i++)
                {
                    ProfileItem profileItem = config.ProfileItems[i];

                    if (string.IsNullOrEmpty(profileItem.indexId))
                    {
                        profileItem.indexId = Utils.GetGUID(false);
                    }
                }
            }

            LazyConfig.Instance.SetConfig(config);
            return 0;
        }

        /// <summary>
        /// 保参数
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int SaveConfig(Config config, bool reload = true)
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
                    var resPath = Utils.GetConfigPath(configRes);
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

        #endregion ConfigHandler

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

            config.IndexId = item.indexId;
            Global.reloadCore = true;

            ToJsonFile(config);

            return 0;
        }

        public static int SetDefaultProfile(Config config, List<ProfileItem> lstProfile)
        {
            if (lstProfile.Exists(t => t.indexId == config.IndexId))
            {
                return 0;
            }
            if (config.ProfileItems.Exists(t => t.indexId == config.IndexId))
            {
                return 0;
            }
            if (lstProfile.Count > 0)
            {
                return SetDefaultProfile(ref config, lstProfile[0]);
            }
            if (config.ProfileItems.Count > 0)
            {
                return SetDefaultProfile(ref config, config.ProfileItems[0]);
            }
            return -1;
        }

        public static ProfileItem? GetDefaultProfile(ref Config config)
        {
            if (config.ProfileItems.Count <= 0)
            {
                return null;
            }
            var index = config.FindIndexId(config.IndexId);
            if (index < 0)
            {
                SetDefaultProfile(ref config, config.ProfileItems[0]);
                return config.ProfileItems[0];
            }

            return config.ProfileItems[index];
        }

        /// <summary>
        /// 移动配置文件
        /// </summary>
        /// <param name="config"></param>
        /// <param name="index"></param>
        /// <param name="eMove"></param>
        /// <returns></returns>
        public static int MoveProfile(ref Config config, int index, MovementTarget eMove, int pos = -1)
        {
            List<ProfileItem> lstProfile = config.ProfileItems.OrderBy(it => it.sort).ToList();
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
                case MovementTarget.Top:
                    {
                        if (index == 0)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[0].sort - 1;

                        break;
                    }
                case MovementTarget.Up:
                    {
                        if (index == 0)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[index - 1].sort - 1;

                        break;
                    }

                case MovementTarget.Down:
                    {
                        if (index == count - 1)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[index + 1].sort + 1;

                        break;
                    }
                case MovementTarget.Bottom:
                    {
                        if (index == count - 1)
                        {
                            return 0;
                        }
                        lstProfile[index].sort = lstProfile[lstProfile.Count - 1].sort + 1;

                        break;
                    }
                case MovementTarget.Position:
                    lstProfile[index].sort = pos * 10 + 1;
                    break;
            }

            ToJsonFile(config);

            return 0;
        }

        public static int AddProfileViaContent(ref Config config, ProfileItem profileItem, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return -1;
            }

            string newFileName = profileItem.address;
            if (string.IsNullOrEmpty(newFileName))
            {
                var ext = ".yaml";
                newFileName = string.Format("{0}{1}", Utils.GetGUID(), ext);
                profileItem.address = newFileName;
            }
            if (string.IsNullOrEmpty(profileItem.remarks))
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

            if (string.IsNullOrEmpty(profileItem.remarks))
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
                if (!string.IsNullOrEmpty(profileItem.address))
                {
                    File.Delete(Path.Combine(Utils.GetConfigPath(), profileItem.address));
                }
            }
            catch
            {
                return -1;
            }

            profileItem.address = newFileName;
            if (string.IsNullOrEmpty(profileItem.remarks))
            {
                profileItem.remarks = string.Format("import custom@{0}", DateTime.Now.ToShortDateString());
            }

            AddProfileCommon(ref config, profileItem);

            ToJsonFile(config);

            return 0;
        }

        public static int EditProfile(ref Config config, ProfileItem profileItem)
        {
            if (!string.IsNullOrEmpty(profileItem.indexId) && config.IndexId == profileItem.indexId)
            {
                Global.reloadCore = true;
            }

            AddProfileCommon(ref config, profileItem);

            //TODO auto update via url
            //if (!string.IsNullOrEmpty(profileItem.url))
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
            if (string.IsNullOrEmpty(profileItem.indexId))
            {
                profileItem.indexId = Utils.GetGUID(false);
            }
            if (profileItem.coreType is null)
            {
                profileItem.coreType = CoreKind.ClashMeta;
            }
            if (!config.ProfileItems.Exists(it => it.indexId == profileItem.indexId))
            {
                var maxSort = config.ProfileItems.Any() ? config.ProfileItems.Max(t => t.sort) : 0;
                profileItem.sort = maxSort++;

                config.ProfileItems.Add(profileItem);
            }

            return 0;
        }

        private static int RemoveProfileItem(Config config, int index)
        {
            try
            {
                if (File.Exists(Utils.GetConfigPath(config.ProfileItems[index].address)))
                {
                    File.Delete(Utils.GetConfigPath(config.ProfileItems[index].address));
                }
            }
            catch (Exception ex)
            {
                Utils.SaveLog("RemoveProfileItem", ex);
            }
            config.ProfileItems.RemoveAt(index);

            return 0;
        }

        public static string GetProfileContent(ProfileItem item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(item.address))
            {
                return string.Empty;
            }
            var content = File.ReadAllText(Utils.GetConfigPath(item.address));

            return content;
        }

        public static int AddBatchProfiles(ref Config config, string clipboardData, string indexId, string groupId)
        {
            if (string.IsNullOrEmpty(clipboardData))
            {
                return -1;
            }

            //maybe url
            if (string.IsNullOrEmpty(indexId) && (clipboardData.StartsWith(Global.httpsProtocol) || clipboardData.StartsWith(Global.httpProtocol)))
            {
                ProfileItem item = new ProfileItem()
                {
                    groupId = groupId,
                    url = clipboardData,
                    coreType = CoreKind.ClashMeta,
                    address = string.Empty,
                    enabled = true,
                    remarks = "clash_subscription"
                };

                return EditProfile(ref config, item);
            }

            //maybe clashProtocol
            if (string.IsNullOrEmpty(indexId) && (clipboardData.StartsWith(Global.clashProtocol)))
            {
                Uri url = new Uri(clipboardData);
                if (url.Host == "install-config")
                {
                    NameValueCollection query =
                        HttpUtility.ParseQueryString(url.Query);

                    if (!string.IsNullOrEmpty(query["url"] ?? ""))
                    {
                        ProfileItem item = new ProfileItem()
                        {
                            groupId = groupId,
                            url = query["url"] ?? string.Empty,
                            coreType = CoreKind.ClashMeta,
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
                    coreType = CoreKind.ClashMeta,
                    address = string.Empty,
                    enabled = false,
                    remarks = "clash_local_file"
                };
                return AddProfileViaPath(ref config, item, clipboardData);
            }

            //Is Clash configuration
            if (((clipboardData.IndexOf("port") >= 0 && clipboardData.IndexOf("socks-port") >= 0)
                    || clipboardData.IndexOf("mixed-port") >= 0)
              && clipboardData.IndexOf("proxies") >= 0
              && clipboardData.IndexOf("rules") >= 0)
            { }
            else
            { return -1; }

            ProfileItem? profileItem = null;
            if (!string.IsNullOrEmpty(indexId))
                profileItem = config.GetProfileItem(indexId);

            if (profileItem == null)
                profileItem = new ProfileItem();
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

        public static void ClearAllServerStatistics(ref Config config)
        {
            foreach (var item in config.ProfileItems)
            {
                item.uploadRemote = 0;
                item.downloadRemote = 0;
            }

            ToJsonFile(config);
        }

        #endregion Profile
    }
}