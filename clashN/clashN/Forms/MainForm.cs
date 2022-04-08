using clashN.Base;
using clashN.Handler;
using clashN.Mode;
using clashN.Resx;
using clashN.Tool;
using NHotkey;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clashN.Forms
{
    public partial class MainForm : BaseForm
    {
        private CoreHandler coreHandler;
        private List<ProfileItem> lstSelecteds = new List<ProfileItem>();
        private StatisticsHandler statistics = null;
        private string MsgFilter = string.Empty;
        private List<ProfileItem> lstProfile = null;
        private string groupId = string.Empty;

        #region Window 事件

        public MainForm()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            HideForm();
            this.Text = $"{Utils.GetVersion()} - {(Utils.IsAdministrator() ? ResUI.RunAsAdmin : ResUI.NotRunAsAdmin)}";
            Global.processJob = new Job();

            Application.ApplicationExit += (sender, args) =>
            {
                MyAppExit(false);
            };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (ConfigHandler.LoadConfig(ref config) != 0)
            {
                UI.ShowWarning($"Loading GUI configuration file is abnormal,please restart the application{Environment.NewLine}加载GUI配置文件异常,请重启应用");
                Environment.Exit(0);
                return;
            }

            MainFormHandler.Instance.BackupGuiNConfig(config, true);
            coreHandler = new CoreHandler(UpdateCoreHandler);

            if (config.enableStatistics)
            {
                statistics = new StatisticsHandler(config, UpdateStatisticsHandler);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            InitProfilesView();
            RefreshProfiles();
            RefreshRoutingsMenu();
            RestoreUI();

            _ = LoadCore();

            HideForm();

            MainFormHandler.Instance.UpdateTask(config, UpdateTaskHandler);
            MainFormHandler.Instance.RegisterGlobalHotkey(config, OnHotkeyHandler, UpdateTaskHandler);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    StorageUI();
                    e.Cancel = true;
                    HideForm();
                    break;
                case CloseReason.ApplicationExitCall:
                case CloseReason.FormOwnerClosing:
                case CloseReason.TaskManagerClosing:
                    MyAppExit(false);
                    break;
                case CloseReason.WindowsShutDown:
                    MyAppExit(true);
                    break;
            }
        }

        private void MyAppExit(bool blWindowsShutDown)
        {
            try
            {
                coreHandler.CoreStop();

                //HttpProxyHandle.CloseHttpAgent(config);
                if (blWindowsShutDown)
                {
                    SysProxyHandle.ResetIEProxy4WindowsShutDown();
                }
                else
                {
                    SysProxyHandle.UpdateSysProxy(config, true);
                }

                StorageUI();
                ConfigHandler.SaveConfig(ref config);
                statistics?.SaveToFile();
                statistics?.Close();
            }
            catch { }
        }

        private void RestoreUI()
        {
            scMain.Panel2Collapsed = true;

            if (!config.uiItem.mainLocation.IsEmpty)
            {
                this.Location = config.uiItem.mainLocation;
            }

            if (!config.uiItem.mainSize.IsEmpty)
            {
                this.Width = config.uiItem.mainSize.Width;
                this.Height = config.uiItem.mainSize.Height;
            }

            for (int k = 0; k < lvProfiles.Columns.Count; k++)
            {
                var width = ConfigHandler.GetformMainLvColWidth(ref config, ((EProfileColName)k).ToString(), lvProfiles.Columns[k].Width);
                lvProfiles.Columns[k].Width = width;
            }
        }

        private void StorageUI()
        {
            config.uiItem.mainLocation = this.Location;

            config.uiItem.mainSize = new Size(this.Width, this.Height);

            for (int k = 0; k < lvProfiles.Columns.Count; k++)
            {
                ConfigHandler.AddformMainLvColWidth(ref config, ((EProfileColName)k).ToString(), lvProfiles.Columns[k].Width);
            }
        }

        private void OnHotkeyHandler(object sender, HotkeyEventArgs e)
        {
            switch (Utils.ToInt(e.Name))
            {
                case (int)EGlobalHotkey.ShowForm:
                    if (this.ShowInTaskbar) HideForm(); else ShowForm();
                    break;
                case (int)EGlobalHotkey.SystemProxyClear:
                    SetListenerType(ESysProxyType.ForcedClear);
                    break;
                case (int)EGlobalHotkey.SystemProxySet:
                    SetListenerType(ESysProxyType.ForcedChange);
                    break;
                case (int)EGlobalHotkey.SystemProxyUnchanged:
                    SetListenerType(ESysProxyType.Unchanged);
                    break;
            }
            e.Handled = true;
        }

        #endregion

        #region 显示配置文件 listview 和 menu

        /// <summary>
        /// 刷新配置文件
        /// </summary>
        private void RefreshProfiles()
        {
            lstProfile = config.profileItems
                //.Where(it => Utils.IsNullOrEmpty(groupId) ? true : it.groupId == groupId)
                .OrderBy(it => it.sort)
                .ToList();

            ConfigHandler.SetDefaultProfile(config, lstProfile);
            RefreshProfilesView();
            RefreshProfilesMenu();
        }

        /// <summary>
        /// 初始化配置文件列表
        /// </summary>
        private void InitProfilesView()
        {
            lvProfiles.BeginUpdate();
            lvProfiles.Items.Clear();

            lvProfiles.GridLines = true;
            lvProfiles.FullRowSelect = true;
            lvProfiles.View = View.Details;
            lvProfiles.Scrollable = true;
            lvProfiles.MultiSelect = true;
            lvProfiles.HeaderStyle = ColumnHeaderStyle.Clickable;
            lvProfiles.RegisterDragEvent(UpdateDragEventHandler);

            lvProfiles.Columns.Add("", 30);
            lvProfiles.Columns.Add(ResUI.LvAlias, 150);
            lvProfiles.Columns.Add(ResUI.LvUrl, 150);
            lvProfiles.Columns.Add(ResUI.LvAddress, 60, HorizontalAlignment.Center);
            lvProfiles.Columns.Add(ResUI.LvEnableTun, 60, HorizontalAlignment.Center);
            lvProfiles.Columns.Add(ResUI.LvEnableUpdateSub, 60, HorizontalAlignment.Center);

            if (statistics != null && statistics.Enable)
            {
                lvProfiles.Columns.Add(ResUI.LvTodayDownloadDataAmount, 70);
                lvProfiles.Columns.Add(ResUI.LvTodayUploadDataAmount, 70);
                lvProfiles.Columns.Add(ResUI.LvTotalDownloadDataAmount, 70);
                lvProfiles.Columns.Add(ResUI.LvTotalUploadDataAmount, 70);
            }
            lvProfiles.EndUpdate();
        }

        private void UpdateDragEventHandler(int index, int targetIndex)
        {
            if (index < 0 || targetIndex < 0)
            {
                return;
            }
            if (ConfigHandler.MoveProfile(ref config, ref lstProfile, index, EMove.Position, targetIndex) == 0)
            {
                RefreshProfiles();
            }
        }

        /// <summary>
        /// 刷新配置文件列表
        /// </summary>
        private void RefreshProfilesView()
        {
            int index = GetLvSelectedIndex(false);

            //lvProfiles.BeginUpdate();
            lvProfiles.Items.Clear();

            for (int k = 0; k < lstProfile.Count; k++)
            {
                string def = string.Empty;

                ProfileItem item = lstProfile[k];
                if (config.IsActiveNode(item))
                {
                    def = Global.CheckMark;
                }

                ListViewItem lvItem = new ListViewItem(def);
                Utils.AddSubItem(lvItem, EProfileColName.remarks.ToString(), item.remarks);
                Utils.AddSubItem(lvItem, EProfileColName.url.ToString(), item.url);
                Utils.AddSubItem(lvItem, EProfileColName.address.ToString(), item.address.IsNullOrWhiteSpace() ? "" : Global.CheckMark);
                Utils.AddSubItem(lvItem, EProfileColName.enableTun.ToString(), item.enableTun ? Global.CheckMark : "");
                Utils.AddSubItem(lvItem, EProfileColName.enableUpdateSub.ToString(), (item.enabled ? Global.CheckMark : "") + (item.enableConvert ? $"({Global.CheckMark})" : ""));

                if (statistics != null && statistics.Enable)
                {
                    string totalUp = string.Empty,
                          totalDown = string.Empty,
                          todayUp = string.Empty,
                          todayDown = string.Empty;
                    ProfileStatItem sItem = statistics.Statistic.Find(item_ => item_.indexId == item.indexId);
                    if (sItem != null)
                    {
                        totalUp = Utils.HumanFy(sItem.totalUp);
                        totalDown = Utils.HumanFy(sItem.totalDown);
                        todayUp = Utils.HumanFy(sItem.todayUp);
                        todayDown = Utils.HumanFy(sItem.todayDown);
                    }
                    Utils.AddSubItem(lvItem, EProfileColName.todayDown.ToString(), todayDown);
                    Utils.AddSubItem(lvItem, EProfileColName.todayUp.ToString(), todayUp);
                    Utils.AddSubItem(lvItem, EProfileColName.totalDown.ToString(), totalDown);
                    Utils.AddSubItem(lvItem, EProfileColName.totalUp.ToString(), totalUp);
                }

                if (k % 2 == 1) // 隔行着色
                {
                    lvItem.BackColor = Color.WhiteSmoke;
                }
                if (config.IsActiveNode(item))
                {
                    //lvItem.Checked = true;
                    lvItem.ForeColor = Color.DodgerBlue;
                    lvItem.Font = new Font(lvItem.Font, FontStyle.Bold);
                }

                if (lvItem != null) lvProfiles.Items.Add(lvItem);
            }
            //lvProfiles.EndUpdate();

            if (index >= 0 && index < lvProfiles.Items.Count && lvProfiles.Items.Count > 0)
            {
                lvProfiles.Items[index].Selected = true;
                lvProfiles.EnsureVisible(index); // workaround
            }
        }

        /// <summary>
        /// 刷新托盘配置文件菜单
        /// </summary>
        private void RefreshProfilesMenu()
        {
            menuProfiles.DropDownItems.Clear();
            menuProfiles2.SelectedIndexChanged -= MenuProfiles2_SelectedIndexChanged;
            menuProfiles2.Items.Clear();
            menuProfiles.Visible = false;
            menuProfiles2.Visible = false;

            if (lstProfile.Count > 20)
            {
                for (int k = 0; k < lstProfile.Count; k++)
                {
                    ProfileItem item = lstProfile[k];
                    string name = item.GetSummary();

                    if (config.IsActiveNode(item))
                    {
                        name = $"√ {name}";
                    }
                    menuProfiles2.Items.Add(name);

                }
                menuProfiles2.SelectedIndex = lstProfile.FindIndex(it => it.indexId == config.indexId);
                menuProfiles2.SelectedIndexChanged += MenuProfiles2_SelectedIndexChanged;
                menuProfiles2.Visible = true;
            }
            else
            {
                List<ToolStripMenuItem> lst = new List<ToolStripMenuItem>();
                for (int k = 0; k < lstProfile.Count; k++)
                {
                    ProfileItem item = lstProfile[k];
                    string name = item.GetSummary();

                    ToolStripMenuItem ts = new ToolStripMenuItem(name)
                    {
                        Tag = k
                    };
                    if (config.IsActiveNode(item))
                    {
                        ts.Checked = true;
                    }
                    ts.Click += new EventHandler(ts_Click);
                    lst.Add(ts);
                }
                menuProfiles.DropDownItems.AddRange(lst.ToArray());
                menuProfiles.Visible = true;
            }
        }

        private void MenuProfiles2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDefaultProfile(((ToolStripComboBox)sender).SelectedIndex);
        }

        private void ts_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripItem ts = (ToolStripItem)sender;
                int index = Utils.ToInt(ts.Tag);
                SetDefaultProfile(index);
            }
            catch
            {
            }
        }

        private void lvProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void DisplayToolStatus()
        {
            toolSslInboundInfo.Text = $"{Global.InboundSocks} {Global.Loopback}:{config.socksPort} | "
             + $"{ Global.InboundHttp} { Global.Loopback}:{config.httpPort}";

            notifyMain.Icon = MainFormHandler.Instance.GetNotifyIcon(config, this.Icon);
        }
        private void ssMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!Utils.IsNullOrEmpty(e.ClickedItem.Text))
            {
                Utils.SetClipboardData(e.ClickedItem.Text);
            }
        }

        private void lvProfiles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column < 0)
            {
                return;
            }

            try
            {
                if ((EProfileColName)e.Column == EProfileColName.def)
                {
                    foreach (ColumnHeader it in lvProfiles.Columns)
                    {
                        it.Width = -2;
                    }
                    return;
                }

                var tag = lvProfiles.Columns[e.Column].Tag?.ToString();
                bool asc = Utils.IsNullOrEmpty(tag) ? true : !Convert.ToBoolean(tag);
                if (ConfigHandler.SortProfiles(ref config, ref lstProfile, (EProfileColName)e.Column, asc) != 0)
                {
                    return;
                }
                lvProfiles.Columns[e.Column].Tag = Convert.ToString(asc);
                RefreshProfiles();
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }

            if (e.Column < 0)
            {
                return;
            }

        }



        #endregion

        #region Core 操作

        async Task LoadCore()
        {
            tsbReload.Enabled = false;
            tsbCurrentProxies.Enabled = false;

            if (Global.reloadCore)
            {
                ClearMsg();
            }
            await Task.Run(() =>
            {
                coreHandler.LoadCore(config);
            });

            Global.reloadCore = false;
            ConfigHandler.SaveConfig(ref config, false);
            statistics?.SaveToFile();

            ChangePACButtonStatus(config.sysProxyType);

            tsbReload.Enabled = true;
            tsbCurrentProxies.Enabled = true;
        }

        private void CloseCore()
        {
            ConfigHandler.SaveConfig(ref config, false);
            statistics?.SaveToFile();

            ChangePACButtonStatus(ESysProxyType.ForcedClear);

            coreHandler.CoreStop();
        }

        #endregion

        #region 功能按钮

        private void lvProfiles_Click(object sender, EventArgs e)
        {
            int index = GetLvSelectedIndex(false);
            if (index < 0)
            {
                return;
            }
            qrCodeControl.showQRCode(lstProfile[index]);
        }

        private void lvProfiles_DoubleClick(object sender, EventArgs e)
        {
            int index = GetLvSelectedIndex();
            if (index < 0)
            {
                return;
            }
            ShowProfileForm(index);
        }
        private void ShowProfileForm(int index)
        {
            var fm = new AddProfileForm();
            fm.profileItem = index >= 0 ? lstProfile[index] : null;
            fm.groupId = groupId;
            if (fm.ShowDialog() == DialogResult.OK)
            {
                RefreshProfiles();
                _ = LoadCore();
            }
        }


        private void lvProfiles_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        menuSelectAll_Click(null, null);
                        break;
                    case Keys.C:
                        menuExport2ShareUrl_Click(null, null);
                        break;
                    case Keys.V:
                        menuAddProfiles_Click(null, null);
                        break;
                    case Keys.S:
                        menuScanScreen_Click(null, null);
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        menuSetDefaultProfile_Click(null, null);
                        break;
                    case Keys.Delete:
                        menuRemoveProfile_Click(null, null);
                        break;
                    case Keys.T:
                        menuMoveTop_Click(null, null);
                        break;
                    case Keys.B:
                        menuMoveBottom_Click(null, null);
                        break;
                    case Keys.U:
                        menuMoveUp_Click(null, null);
                        break;
                    case Keys.D:
                        menuMoveDown_Click(null, null);
                        break;
                }
            }
        }

        private void menuRemoveProfile_Click(object sender, EventArgs e)
        {

            int index = GetLvSelectedIndex();
            if (index < 0)
            {
                return;
            }
            if (UI.ShowYesNo(ResUI.RemoveProfile) == DialogResult.No)
            {
                return;
            }

            ConfigHandler.RemoveProfile(config, lstSelecteds);

            RefreshProfiles();
            _ = LoadCore();
        }

        private void menuCopyProfile_Click(object sender, EventArgs e)
        {
            int index = GetLvSelectedIndex();
            if (index < 0)
            {
                return;
            }
            if (ConfigHandler.CopyProfile(ref config, lstSelecteds) == 0)
            {
                RefreshProfiles();
            }
        }

        private void menuSetDefaultProfile_Click(object sender, EventArgs e)
        {
            int index = GetLvSelectedIndex();
            if (index < 0)
            {
                return;
            }
            SetDefaultProfile(index);
        }

        private void menuClearStatistic_Click(object sender, EventArgs e)
        {
            if (statistics != null)
            {
                statistics.ClearAllServerStatistics();
            }
        }

        private void menuExport2ShareUrl_Click(object sender, EventArgs e)
        {
            GetLvSelectedIndex();

            var item = lstSelecteds[0];
            var content = ConfigHandler.GetProfileContent(item);
            if (Utils.IsNullOrEmpty(content))
            {
                content = item.url;
            }
            Utils.SetClipboardData(content);
            AppendText(false, ResUI.BatchExportSuccessfully);
        }


        private void tsbOptionSetting_Click(object sender, EventArgs e)
        {
            OptionSettingForm fm = new OptionSettingForm();
            if (fm.ShowDialog() == DialogResult.OK)
            {
                //RefreshProfiles();
                _ = LoadCore();
            }
        }

        private void tsbGlobalHotkeySetting_Click(object sender, EventArgs e)
        {
            var fm = new GlobalHotkeySettingForm();
            if (fm.ShowDialog() == DialogResult.OK)
            {
                RefreshRoutingsMenu();
                //RefreshProfiles();
                _ = LoadCore();
            }

        }

        private void tsbReload_Click(object sender, EventArgs e)
        {
            Global.reloadCore = true;
            _ = LoadCore();
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            StorageUI();
            HideForm();
            //this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 设置活动配置文件
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int SetDefaultProfile(int index)
        {
            if (index < 0)
            {
                UI.Show(ResUI.PleaseSelectProfile);
                return -1;
            }
            if (ConfigHandler.SetDefaultProfile(ref config, lstProfile[index]) == 0)
            {
                RefreshProfiles();
                _ = LoadCore();
            }
            return 0;
        }

        /// <summary>
        /// 取得ListView选中的行
        /// </summary>
        /// <returns></returns>
        private int GetLvSelectedIndex(bool show = true)
        {
            int index = -1;
            lstSelecteds.Clear();
            try
            {
                if (lvProfiles.SelectedIndices.Count <= 0)
                {
                    if (show)
                    {
                        UI.Show(ResUI.PleaseSelectProfile);
                    }
                    return index;
                }

                index = lvProfiles.SelectedIndices[0];
                foreach (int i in lvProfiles.SelectedIndices)
                {
                    lstSelecteds.Add(lstProfile[i]);
                }
                return index;
            }
            catch
            {
                return index;
            }
        }

        private void menuAddCustomProfile_Click(object sender, EventArgs e)
        {
            ShowProfileForm(-1);
        }

        private void menuAddProfiles_Click(object sender, EventArgs e)
        {
            string clipboardData = Utils.GetClipboardData();
            int ret = ConfigHandler.AddBatchProfiles(ref config, clipboardData, "", groupId);
            if (ret == 0)
            {
                RefreshProfiles();
                UI.Show(ResUI.SuccessfullyImportedProfileViaClipboard);
            }
        }

        private void menuScanScreen_Click(object sender, EventArgs e)
        {
            _ = ScanScreenTaskAsync();
        }

        public async Task ScanScreenTaskAsync()
        {
            HideForm();

            string result = await Task.Run(() =>
            {
                return Utils.ScanScreen();
            });

            ShowForm();

            if (Utils.IsNullOrEmpty(result))
            {
                UI.ShowWarning(ResUI.NoValidQRcodeFound);
            }
            else
            {
                int ret = ConfigHandler.AddBatchProfiles(ref config, result, "", groupId);
                if (ret == 0)
                {
                    RefreshProfiles();
                    UI.Show(ResUI.SuccessfullyImportedProfileViaScan);
                }
            }
        }

        private void menuUpdateSubscriptions_Click(object sender, EventArgs e)
        {
            UpdateSubscriptionProcess(false);
        }
        private void menuUpdateSubViaProxy_Click(object sender, EventArgs e)
        {
            UpdateSubscriptionProcess(true);
        }

        private void tsbBackupGuiNConfig_Click(object sender, EventArgs e)
        {
            MainFormHandler.Instance.BackupGuiNConfig(config);
        }
        #endregion


        #region 提示信息

        /// <summary>
        /// 消息委托
        /// </summary>
        /// <param name="notify"></param>
        /// <param name="msg"></param>
        void UpdateCoreHandler(bool notify, string msg)
        {
            AppendText(notify, msg);
        }

        // delegate void AppendTextDelegate(string text);
        void AppendText(bool notify, string msg)
        {
            try
            {
                AppendText(msg);
                if (notify)
                {
                    notifyMsg(msg);
                }
            }
            catch
            {
            }
        }

        void AppendText(string text)
        {
            txtMsgBox.BeginInvoke(new Action(() =>
            {
                if (!Utils.IsNullOrEmpty(MsgFilter))
                {
                    if (!Regex.IsMatch(text, MsgFilter))
                    {
                        return;
                    }
                }
                ShowMsg(text);
            }));

            //if (this.txtMsgBox.InvokeRequired)
            //{
            //    Invoke(new AppendTextDelegate(AppendText), new object[] { text });
            //}
            //else
            //{
            //    if (!Utils.IsNullOrEmpty(MsgFilter))
            //    {
            //        if (!Regex.IsMatch(text, MsgFilter))
            //        {
            //            return;
            //        }
            //    }
            //    //this.txtMsgBox.AppendText(text);
            //    ShowMsg(text);
            //}
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="msg"></param>
        private void ShowMsg(string msg)
        {
            if (txtMsgBox.Lines.Length > 999)
            {
                ClearMsg();
            }
            this.txtMsgBox.AppendText(msg);
            if (!msg.EndsWith(Environment.NewLine))
            {
                this.txtMsgBox.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// 清除信息
        /// </summary>
        private void ClearMsg()
        {
            txtMsgBox.BeginInvoke(new Action(() =>
            {
                txtMsgBox.Clear();
            }));
        }

        /// <summary>
        /// 托盘信息
        /// </summary>
        /// <param name="msg"></param>
        private void notifyMsg(string msg)
        {
            notifyMain.Text = msg;
        }

        #endregion


        #region 托盘事件

        private void notifyMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowForm();
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Close();

            Application.Exit();
        }


        private void ShowForm()
        {
            this.Show();
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.Activate();
            this.ShowInTaskbar = true;
            //this.notifyIcon1.Visible = false;
            this.txtMsgBox.ScrollToCaret();

            int index = GetLvSelectedIndex(false);
            if (index >= 0 && index < lvProfiles.Items.Count && lvProfiles.Items.Count > 0)
            {
                lvProfiles.Items[index].Selected = true;
                lvProfiles.EnsureVisible(index); // workaround
            }

            SetVisibleCore(true);
        }

        private void HideForm()
        {
            //this.WindowState = FormWindowState.Minimized;
            this.Hide();
            //this.notifyMain.Icon = this.Icon;
            this.notifyMain.Visible = true;
            this.ShowInTaskbar = false;

            SetVisibleCore(false);
        }

        #endregion

        #region 后台测速

        private void UpdateStatisticsHandler(ulong up, ulong down, List<ProfileStatItem> statistics)
        {
            if (!this.ShowInTaskbar)
            {
                return;
            }
            try
            {
                toolSslProfileSpeed.Text = string.Format("{0}/s↑ | {1}/s↓", Utils.HumanFy(up), Utils.HumanFy(down));

                foreach (var it in statistics)
                {
                    int index = lstProfile.FindIndex(item => item.indexId == it.indexId);
                    if (index < 0)
                    {
                        continue;
                    }
                    lvProfiles.Invoke((MethodInvoker)delegate
                    {
                        lvProfiles.BeginUpdate();

                        lvProfiles.Items[index].SubItems["todayDown"].Text = Utils.HumanFy(it.todayDown);
                        lvProfiles.Items[index].SubItems["todayUp"].Text = Utils.HumanFy(it.todayUp);
                        lvProfiles.Items[index].SubItems["totalDown"].Text = Utils.HumanFy(it.totalDown);
                        lvProfiles.Items[index].SubItems["totalUp"].Text = Utils.HumanFy(it.totalUp);

                        lvProfiles.EndUpdate();
                    });
                }
            }
            catch (Exception ex)
            {
                Utils.SaveLog(ex.Message, ex);
            }
        }

        private void UpdateTaskHandler(bool success, string msg)
        {
            AppendText(false, msg);
            if (success)
            {
                Global.reloadCore = true;
                _ = LoadCore();
            }
        }
        #endregion

        #region 移动配置文件

        private void menuMoveTop_Click(object sender, EventArgs e)
        {
            MoveProfile(EMove.Top);
        }

        private void menuMoveUp_Click(object sender, EventArgs e)
        {
            MoveProfile(EMove.Up);
        }

        private void menuMoveDown_Click(object sender, EventArgs e)
        {
            MoveProfile(EMove.Down);
        }

        private void menuMoveBottom_Click(object sender, EventArgs e)
        {
            MoveProfile(EMove.Bottom);
        }

        private void MoveProfile(EMove eMove)
        {
            int index = GetLvSelectedIndex();
            if (index < 0)
            {
                UI.Show(ResUI.PleaseSelectProfile);
                return;
            }
            if (ConfigHandler.MoveProfile(ref config, ref lstProfile, index, eMove) == 0)
            {
                RefreshProfiles();
            }
        }
        private void menuSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvProfiles.Items)
            {
                item.Selected = true;
            }
        }
        private void menuMoveToGroup_Click(object sender, EventArgs e)
        {
        }
        #endregion

        #region 系统代理相关
        private void menuKeepClear_Click(object sender, EventArgs e)
        {
            SetListenerType(ESysProxyType.ForcedClear);
        }
        private void menuGlobal_Click(object sender, EventArgs e)
        {
            SetListenerType(ESysProxyType.ForcedChange);
        }

        private void menuKeepNothing_Click(object sender, EventArgs e)
        {
            SetListenerType(ESysProxyType.Unchanged);
        }
        private void SetListenerType(ESysProxyType type)
        {
            config.sysProxyType = type;
            ChangePACButtonStatus(type);
        }

        private void ChangePACButtonStatus(ESysProxyType type)
        {
            SysProxyHandle.UpdateSysProxy(config, false);

            for (int k = 0; k < menuSysAgentMode.DropDownItems.Count; k++)
            {
                ToolStripMenuItem item = ((ToolStripMenuItem)menuSysAgentMode.DropDownItems[k]);
                item.Checked = ((int)type == k);
            }

            ConfigHandler.SaveConfig(ref config, false);
            DisplayToolStatus();
        }

        #endregion


        #region CheckUpdate

        private void tsbCheckUpdateN_Click(object sender, EventArgs e)
        {
            void _updateUI(bool success, string msg)
            {
                AppendText(false, msg);
                if (success)
                {
                    menuExit_Click(null, null);
                }
            };
            (new UpdateHandle()).CheckUpdateGuiN(config, _updateUI);
        }

        private void tsbCheckUpdateCore_Click(object sender, EventArgs e)
        {
            CheckUpdateCore(ECoreType.clash);
        }

        private void tsbCheckUpdateMetaCore_Click(object sender, EventArgs e)
        {
            CheckUpdateCore(ECoreType.clash_meta);
        }

        private void CheckUpdateCore(ECoreType type)
        {
            void _updateUI(bool success, string msg)
            {
                AppendText(false, msg);
                if (success)
                {
                    CloseCore();

                    string fileName = Utils.GetPath(Utils.GetDownloadFileName(msg));
                    FileManager.ZipExtractToFile(fileName, config.ignoreGeoUpdateCore ? "geo" : "");

                    AppendText(false, ResUI.MsgUpdateCoreCoreSuccessfullyMore);

                    Global.reloadCore = true;
                    _ = LoadCore();

                    AppendText(false, ResUI.MsgUpdateCoreCoreSuccessfully);
                }
            };
            (new UpdateHandle()).CheckUpdateCore(type, config, _updateUI);
        }

        private void tsbCheckUpdateGeoSite_Click(object sender, EventArgs e)
        {
            (new UpdateHandle()).UpdateGeoFile("geosite", config, (bool success, string msg) =>
            {
                AppendText(false, msg);
                if (success)
                {
                    Global.reloadCore = true;
                    _ = LoadCore();
                }
            });
        }

        private void tsbCheckUpdateGeoIP_Click(object sender, EventArgs e)
        {
            (new UpdateHandle()).UpdateGeoFile("geoip", config, (bool success, string msg) =>
            {
                AppendText(false, msg);
                if (success)
                {
                    Global.reloadCore = true;
                    _ = LoadCore();
                }
            });
        }
        #endregion

        #region Help


        private void tsbAbout_Click(object sender, EventArgs e)
        {
            Utils.ProcessStart(Global.AboutUrl);
        }


        private void tsbPromotion_Click(object sender, EventArgs e)
        {
            Utils.ProcessStart($"{Utils.Base64Decode(Global.PromotionUrl)}?t={DateTime.Now.Ticks}");
        }
        private void tsbCurrentProxies_Click(object sender, EventArgs e)
        {
            ProxiesForm fm = new ProxiesForm();
            fm.ShowDialog();
        }
        #endregion

        #region 订阅 

        private void tsbSubUpdate_Click(object sender, EventArgs e)
        {
            UpdateSubscriptionProcess(false);
        }

        private void tsbSubUpdateViaProxy_Click(object sender, EventArgs e)
        {
            UpdateSubscriptionProcess(true);
        }

        /// <summary>
        /// the subscription update process
        /// </summary>
        private void UpdateSubscriptionProcess(bool blProxy)
        {
            void _updateUI(bool success, string msg)
            {
                AppendText(false, msg);
                if (success)
                {
                    //RefreshProfiles();
                }
            };

            (new UpdateHandle()).UpdateSubscriptionProcess(config, blProxy, _updateUI);
        }

        private void tsbQRCodeSwitch_CheckedChanged(object sender, EventArgs e)
        {
            bool bShow = tsbQRCodeSwitch.Checked;
            scMain.Panel2Collapsed = !bShow;
        }
        #endregion

        #region Language

        private void tsbLanguageDef_Click(object sender, EventArgs e)
        {
            SetCurrentLanguage("en");
        }

        private void tsbLanguageZhHans_Click(object sender, EventArgs e)
        {
            SetCurrentLanguage("zh-Hans");
        }
        private void SetCurrentLanguage(string value)
        {
            Utils.RegWriteValue(Global.MyRegPath, Global.MyRegKeyLanguage, value);
            //Application.Restart();
        }

        #endregion


        #region RoutingsMenu

        /// <summary>
        ///  
        /// </summary>
        private void RefreshRoutingsMenu()
        {
            menuRoutings.DropDownItems.Clear();

            List<ToolStripMenuItem> lst = new List<ToolStripMenuItem>();

            menuRoutings.DropDownItems.AddRange(lst.ToArray());
        }

        #endregion

        #region MsgBoxMenu
        private void menuMsgBoxSelectAll_Click(object sender, EventArgs e)
        {
            this.txtMsgBox.Focus();
            this.txtMsgBox.SelectAll();
        }

        private void menuMsgBoxCopy_Click(object sender, EventArgs e)
        {
            var data = this.txtMsgBox.SelectedText.TrimEx();
            Utils.SetClipboardData(data);
        }

        private void menuMsgBoxCopyAll_Click(object sender, EventArgs e)
        {
            var data = this.txtMsgBox.Text;
            Utils.SetClipboardData(data);
        }
        private void menuMsgBoxClear_Click(object sender, EventArgs e)
        {
            this.txtMsgBox.Clear();
        }

        private void txtMsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        menuMsgBoxSelectAll_Click(null, null);
                        break;
                    case Keys.C:
                        menuMsgBoxCopy_Click(null, null);
                        break;

                }
            }

        }
        private void menuMsgBoxFilter_Click(object sender, EventArgs e)
        {
            var fm = new MsgFilterSetForm();
            fm.MsgFilter = MsgFilter;
            if (fm.ShowDialog() == DialogResult.OK)
            {
                MsgFilter = fm.MsgFilter;
                gbMsgTitle.Text = string.Format(ResUI.MsgInformationTitle, MsgFilter);
            }
        }
        #endregion     
    }
}
