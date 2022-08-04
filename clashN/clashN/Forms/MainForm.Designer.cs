namespace clashN.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.scServers = new System.Windows.Forms.SplitContainer();
            this.lvProfiles = new clashN.Base.ListViewFlickerFree();
            this.cmsLv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddProfileViaClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuScanScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExport2Clipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSubUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSubUpdateSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSubUpdateViaProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSubUpdateViaProxySelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRemoveProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCopyProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetDefaultProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMoveTop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMoveBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearProfileStatistics = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbProfile = new System.Windows.Forms.ToolStripDropDownButton();
            this.qrCodeControl = new clashN.Forms.QRCodeControl();
            this.scBig = new System.Windows.Forms.SplitContainer();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tpProxies = new System.Windows.Forms.TabPage();
            this.proxiesControl = new clashN.Forms.ProxiesControl();
            this.tpProfile = new System.Windows.Forms.TabPage();
            this.mainMsgControl = new clashN.Forms.MainMsgControl();
            this.notifyMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSysAgentMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKeepClear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGlobal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKeepNothing = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRuleMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuModeRule = new System.Windows.Forms.ToolStripMenuItem();
            this.menuModeGlobal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuModeDirect = new System.Windows.Forms.ToolStripMenuItem();
            this.menuModeKeep = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProfiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuScanScreen2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpdateSubscriptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpdateSubViaProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbCurrentProxies = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbProxiesReload = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbProxiesSpeedtest = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbQRCodeSwitch = new System.Windows.Forms.ToolStripButton();
            this.tsbReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSetting = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbOptionSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbGlobalHotkeySetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbBackupGuiNConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCheckUpdate = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbCheckUpdateN = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCheckUpdateCore = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCheckUpdateMetaCore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCheckUpdateGeo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHelp = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLanguageDef = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLanguageZhHans = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbPromotion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.scServers)).BeginInit();
            this.scServers.Panel1.SuspendLayout();
            this.scServers.Panel2.SuspendLayout();
            this.scServers.SuspendLayout();
            this.cmsLv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scBig)).BeginInit();
            this.scBig.Panel1.SuspendLayout();
            this.scBig.Panel2.SuspendLayout();
            this.scBig.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tpProxies.SuspendLayout();
            this.tpProfile.SuspendLayout();
            this.cmsMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // scServers
            // 
            resources.ApplyResources(this.scServers, "scServers");
            this.scServers.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scServers.Name = "scServers";
            // 
            // scServers.Panel1
            // 
            this.scServers.Panel1.Controls.Add(this.lvProfiles);
            // 
            // scServers.Panel2
            // 
            this.scServers.Panel2.Controls.Add(this.qrCodeControl);
            this.scServers.TabStop = false;
            // 
            // lvProfiles
            // 
            this.lvProfiles.ContextMenuStrip = this.cmsLv;
            resources.ApplyResources(this.lvProfiles, "lvProfiles");
            this.lvProfiles.FullRowSelect = true;
            this.lvProfiles.GridLines = true;
            this.lvProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvProfiles.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvProfiles.Items")))});
            this.lvProfiles.MultiSelect = false;
            this.lvProfiles.Name = "lvProfiles";
            this.lvProfiles.UseCompatibleStateImageBehavior = false;
            this.lvProfiles.View = System.Windows.Forms.View.Details;
            this.lvProfiles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvProfiles_ColumnClick);
            this.lvProfiles.SelectedIndexChanged += new System.EventHandler(this.lvProfiles_SelectedIndexChanged);
            this.lvProfiles.Click += new System.EventHandler(this.lvProfiles_Click);
            this.lvProfiles.DoubleClick += new System.EventHandler(this.lvProfiles_DoubleClick);
            this.lvProfiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvProfiles_KeyDown);
            // 
            // cmsLv
            // 
            this.cmsLv.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsLv.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddProfile,
            this.menuAddProfileViaClipboard,
            this.menuScanScreen,
            this.menuExport2Clipboard,
            this.toolStripSeparator4,
            this.tsbSubUpdate,
            this.tsbSubUpdateSelected,
            this.tsbSubUpdateViaProxy,
            this.tsbSubUpdateViaProxySelected,
            this.toolStripSeparator1,
            this.menuRemoveProfile,
            this.menuCopyProfile,
            this.menuSetDefaultProfile,
            this.toolStripSeparator3,
            this.menuMoveTop,
            this.menuMoveUp,
            this.menuMoveDown,
            this.menuMoveBottom,
            this.menuSelectAll,
            this.toolStripSeparator9,
            this.menuClearProfileStatistics});
            this.cmsLv.Name = "cmsLv";
            this.cmsLv.OwnerItem = this.tsbProfile;
            resources.ApplyResources(this.cmsLv, "cmsLv");
            // 
            // menuAddProfile
            // 
            this.menuAddProfile.Name = "menuAddProfile";
            resources.ApplyResources(this.menuAddProfile, "menuAddProfile");
            this.menuAddProfile.Click += new System.EventHandler(this.menuAddCustomProfile_Click);
            // 
            // menuAddProfileViaClipboard
            // 
            this.menuAddProfileViaClipboard.Name = "menuAddProfileViaClipboard";
            resources.ApplyResources(this.menuAddProfileViaClipboard, "menuAddProfileViaClipboard");
            this.menuAddProfileViaClipboard.Click += new System.EventHandler(this.menuAddProfiles_Click);
            // 
            // menuScanScreen
            // 
            this.menuScanScreen.Name = "menuScanScreen";
            resources.ApplyResources(this.menuScanScreen, "menuScanScreen");
            this.menuScanScreen.Click += new System.EventHandler(this.menuScanScreen_Click);
            // 
            // menuExport2Clipboard
            // 
            this.menuExport2Clipboard.Name = "menuExport2Clipboard";
            resources.ApplyResources(this.menuExport2Clipboard, "menuExport2Clipboard");
            this.menuExport2Clipboard.Click += new System.EventHandler(this.menuExport2ShareUrl_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // tsbSubUpdate
            // 
            this.tsbSubUpdate.Name = "tsbSubUpdate";
            resources.ApplyResources(this.tsbSubUpdate, "tsbSubUpdate");
            this.tsbSubUpdate.Click += new System.EventHandler(this.tsbSubUpdate_Click);
            // 
            // tsbSubUpdateSelected
            // 
            this.tsbSubUpdateSelected.Name = "tsbSubUpdateSelected";
            resources.ApplyResources(this.tsbSubUpdateSelected, "tsbSubUpdateSelected");
            this.tsbSubUpdateSelected.Click += new System.EventHandler(this.tsbSubUpdateSelected_Click);
            // 
            // tsbSubUpdateViaProxy
            // 
            this.tsbSubUpdateViaProxy.Name = "tsbSubUpdateViaProxy";
            resources.ApplyResources(this.tsbSubUpdateViaProxy, "tsbSubUpdateViaProxy");
            this.tsbSubUpdateViaProxy.Click += new System.EventHandler(this.tsbSubUpdateViaProxy_Click);
            // 
            // tsbSubUpdateViaProxySelected
            // 
            this.tsbSubUpdateViaProxySelected.Name = "tsbSubUpdateViaProxySelected";
            resources.ApplyResources(this.tsbSubUpdateViaProxySelected, "tsbSubUpdateViaProxySelected");
            this.tsbSubUpdateViaProxySelected.Click += new System.EventHandler(this.tsbSubUpdateViaProxySelected_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // menuRemoveProfile
            // 
            this.menuRemoveProfile.Name = "menuRemoveProfile";
            resources.ApplyResources(this.menuRemoveProfile, "menuRemoveProfile");
            this.menuRemoveProfile.Click += new System.EventHandler(this.menuRemoveProfile_Click);
            // 
            // menuCopyProfile
            // 
            this.menuCopyProfile.Name = "menuCopyProfile";
            resources.ApplyResources(this.menuCopyProfile, "menuCopyProfile");
            this.menuCopyProfile.Click += new System.EventHandler(this.menuCopyProfile_Click);
            // 
            // menuSetDefaultProfile
            // 
            this.menuSetDefaultProfile.Name = "menuSetDefaultProfile";
            resources.ApplyResources(this.menuSetDefaultProfile, "menuSetDefaultProfile");
            this.menuSetDefaultProfile.Click += new System.EventHandler(this.menuSetDefaultProfile_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // menuMoveTop
            // 
            this.menuMoveTop.Name = "menuMoveTop";
            resources.ApplyResources(this.menuMoveTop, "menuMoveTop");
            this.menuMoveTop.Click += new System.EventHandler(this.menuMoveTop_Click);
            // 
            // menuMoveUp
            // 
            this.menuMoveUp.Name = "menuMoveUp";
            resources.ApplyResources(this.menuMoveUp, "menuMoveUp");
            this.menuMoveUp.Click += new System.EventHandler(this.menuMoveUp_Click);
            // 
            // menuMoveDown
            // 
            this.menuMoveDown.Name = "menuMoveDown";
            resources.ApplyResources(this.menuMoveDown, "menuMoveDown");
            this.menuMoveDown.Click += new System.EventHandler(this.menuMoveDown_Click);
            // 
            // menuMoveBottom
            // 
            this.menuMoveBottom.Name = "menuMoveBottom";
            resources.ApplyResources(this.menuMoveBottom, "menuMoveBottom");
            this.menuMoveBottom.Click += new System.EventHandler(this.menuMoveBottom_Click);
            // 
            // menuSelectAll
            // 
            this.menuSelectAll.Name = "menuSelectAll";
            resources.ApplyResources(this.menuSelectAll, "menuSelectAll");
            this.menuSelectAll.Click += new System.EventHandler(this.menuSelectAll_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // menuClearProfileStatistics
            // 
            this.menuClearProfileStatistics.Name = "menuClearProfileStatistics";
            resources.ApplyResources(this.menuClearProfileStatistics, "menuClearProfileStatistics");
            this.menuClearProfileStatistics.Click += new System.EventHandler(this.menuClearStatistic_Click);
            // 
            // tsbProfile
            // 
            this.tsbProfile.DropDown = this.cmsLv;
            this.tsbProfile.Image = global::clashN.Properties.Resources.server;
            resources.ApplyResources(this.tsbProfile, "tsbProfile");
            this.tsbProfile.Name = "tsbProfile";
            // 
            // qrCodeControl
            // 
            resources.ApplyResources(this.qrCodeControl, "qrCodeControl");
            this.qrCodeControl.Name = "qrCodeControl";
            // 
            // scBig
            // 
            resources.ApplyResources(this.scBig, "scBig");
            this.scBig.Name = "scBig";
            // 
            // scBig.Panel1
            // 
            this.scBig.Panel1.Controls.Add(this.tabMain);
            // 
            // scBig.Panel2
            // 
            this.scBig.Panel2.Controls.Add(this.mainMsgControl);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tpProxies);
            this.tabMain.Controls.Add(this.tpProfile);
            resources.ApplyResources(this.tabMain, "tabMain");
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tpProxies
            // 
            this.tpProxies.Controls.Add(this.proxiesControl);
            resources.ApplyResources(this.tpProxies, "tpProxies");
            this.tpProxies.Name = "tpProxies";
            this.tpProxies.UseVisualStyleBackColor = true;
            // 
            // proxiesControl
            // 
            resources.ApplyResources(this.proxiesControl, "proxiesControl");
            this.proxiesControl.Name = "proxiesControl";
            // 
            // tpProfile
            // 
            this.tpProfile.Controls.Add(this.scServers);
            resources.ApplyResources(this.tpProfile, "tpProfile");
            this.tpProfile.Name = "tpProfile";
            this.tpProfile.UseVisualStyleBackColor = true;
            // 
            // mainMsgControl
            // 
            resources.ApplyResources(this.mainMsgControl, "mainMsgControl");
            this.mainMsgControl.Name = "mainMsgControl";
            // 
            // notifyMain
            // 
            this.notifyMain.ContextMenuStrip = this.cmsMain;
            resources.ApplyResources(this.notifyMain, "notifyMain");
            this.notifyMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyMain_MouseClick);
            // 
            // cmsMain
            // 
            this.cmsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            resources.ApplyResources(this.cmsMain, "cmsMain");
            this.cmsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSysAgentMode,
            this.menuRuleMode,
            this.menuProfiles,
            this.toolStripSeparator13,
            this.menuScanScreen2,
            this.menuUpdateSubscriptions,
            this.menuUpdateSubViaProxy,
            this.toolStripSeparator2,
            this.menuExit});
            this.cmsMain.Name = "contextMenuStrip1";
            this.cmsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmsMain.ShowCheckMargin = true;
            this.cmsMain.ShowImageMargin = false;
            // 
            // menuSysAgentMode
            // 
            this.menuSysAgentMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuKeepClear,
            this.menuGlobal,
            this.menuKeepNothing});
            this.menuSysAgentMode.Name = "menuSysAgentMode";
            resources.ApplyResources(this.menuSysAgentMode, "menuSysAgentMode");
            // 
            // menuKeepClear
            // 
            this.menuKeepClear.Name = "menuKeepClear";
            resources.ApplyResources(this.menuKeepClear, "menuKeepClear");
            this.menuKeepClear.Click += new System.EventHandler(this.menuKeepClear_Click);
            // 
            // menuGlobal
            // 
            this.menuGlobal.Name = "menuGlobal";
            resources.ApplyResources(this.menuGlobal, "menuGlobal");
            this.menuGlobal.Click += new System.EventHandler(this.menuGlobal_Click);
            // 
            // menuKeepNothing
            // 
            this.menuKeepNothing.Name = "menuKeepNothing";
            resources.ApplyResources(this.menuKeepNothing, "menuKeepNothing");
            this.menuKeepNothing.Click += new System.EventHandler(this.menuKeepNothing_Click);
            // 
            // menuRuleMode
            // 
            this.menuRuleMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuModeRule,
            this.menuModeGlobal,
            this.menuModeDirect,
            this.menuModeKeep});
            this.menuRuleMode.Name = "menuRuleMode";
            resources.ApplyResources(this.menuRuleMode, "menuRuleMode");
            // 
            // menuModeRule
            // 
            this.menuModeRule.Name = "menuModeRule";
            resources.ApplyResources(this.menuModeRule, "menuModeRule");
            this.menuModeRule.Click += new System.EventHandler(this.menuModeRule_Click);
            // 
            // menuModeGlobal
            // 
            this.menuModeGlobal.Name = "menuModeGlobal";
            resources.ApplyResources(this.menuModeGlobal, "menuModeGlobal");
            this.menuModeGlobal.Click += new System.EventHandler(this.menuModeGlobal_Click);
            // 
            // menuModeDirect
            // 
            this.menuModeDirect.Name = "menuModeDirect";
            resources.ApplyResources(this.menuModeDirect, "menuModeDirect");
            this.menuModeDirect.Click += new System.EventHandler(this.menuModeDirect_Click);
            // 
            // menuModeKeep
            // 
            this.menuModeKeep.Name = "menuModeKeep";
            resources.ApplyResources(this.menuModeKeep, "menuModeKeep");
            this.menuModeKeep.Click += new System.EventHandler(this.menuModeKeep_Click);
            // 
            // menuProfiles
            // 
            this.menuProfiles.Name = "menuProfiles";
            resources.ApplyResources(this.menuProfiles, "menuProfiles");
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // menuScanScreen2
            // 
            this.menuScanScreen2.Name = "menuScanScreen2";
            resources.ApplyResources(this.menuScanScreen2, "menuScanScreen2");
            this.menuScanScreen2.Click += new System.EventHandler(this.menuScanScreen_Click);
            // 
            // menuUpdateSubscriptions
            // 
            this.menuUpdateSubscriptions.Name = "menuUpdateSubscriptions";
            resources.ApplyResources(this.menuUpdateSubscriptions, "menuUpdateSubscriptions");
            this.menuUpdateSubscriptions.Click += new System.EventHandler(this.menuUpdateSubscriptions_Click);
            // 
            // menuUpdateSubViaProxy
            // 
            this.menuUpdateSubViaProxy.Name = "menuUpdateSubViaProxy";
            resources.ApplyResources(this.menuUpdateSubViaProxy, "menuUpdateSubViaProxy");
            this.menuUpdateSubViaProxy.Click += new System.EventHandler(this.menuUpdateSubViaProxy_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            resources.ApplyResources(this.menuExit, "menuExit");
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tsMain
            // 
            this.tsMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCurrentProxies,
            this.toolStripSeparator6,
            this.tsbProfile,
            this.tsbQRCodeSwitch,
            this.tsbReload,
            this.toolStripSeparator8,
            this.tsbSetting,
            this.toolStripSeparator7,
            this.tsbCheckUpdate,
            this.toolStripSeparator10,
            this.tsbHelp,
            this.tsbPromotion,
            this.toolStripSeparator11,
            this.tsbClose});
            resources.ApplyResources(this.tsMain, "tsMain");
            this.tsMain.Name = "tsMain";
            this.tsMain.TabStop = true;
            // 
            // tsbCurrentProxies
            // 
            this.tsbCurrentProxies.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbProxiesReload,
            this.tsbProxiesSpeedtest});
            this.tsbCurrentProxies.Image = global::clashN.Properties.Resources.active;
            resources.ApplyResources(this.tsbCurrentProxies, "tsbCurrentProxies");
            this.tsbCurrentProxies.Name = "tsbCurrentProxies";
            // 
            // tsbProxiesReload
            // 
            this.tsbProxiesReload.Name = "tsbProxiesReload";
            resources.ApplyResources(this.tsbProxiesReload, "tsbProxiesReload");
            this.tsbProxiesReload.Click += new System.EventHandler(this.tsbProxiesReload_Click);
            // 
            // tsbProxiesSpeedtest
            // 
            this.tsbProxiesSpeedtest.Name = "tsbProxiesSpeedtest";
            resources.ApplyResources(this.tsbProxiesSpeedtest, "tsbProxiesSpeedtest");
            this.tsbProxiesSpeedtest.Click += new System.EventHandler(this.tsbProxiesSpeedtest_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // tsbQRCodeSwitch
            // 
            this.tsbQRCodeSwitch.CheckOnClick = true;
            this.tsbQRCodeSwitch.ForeColor = System.Drawing.Color.Black;
            this.tsbQRCodeSwitch.Image = global::clashN.Properties.Resources.share;
            resources.ApplyResources(this.tsbQRCodeSwitch, "tsbQRCodeSwitch");
            this.tsbQRCodeSwitch.Name = "tsbQRCodeSwitch";
            this.tsbQRCodeSwitch.CheckedChanged += new System.EventHandler(this.tsbQRCodeSwitch_CheckedChanged);
            // 
            // tsbReload
            // 
            this.tsbReload.Image = global::clashN.Properties.Resources.restart;
            resources.ApplyResources(this.tsbReload, "tsbReload");
            this.tsbReload.Name = "tsbReload";
            this.tsbReload.Click += new System.EventHandler(this.tsbReload_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // tsbSetting
            // 
            this.tsbSetting.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOptionSetting,
            this.tsbGlobalHotkeySetting,
            this.toolStripSeparator14,
            this.tsbBackupGuiNConfig});
            this.tsbSetting.Image = global::clashN.Properties.Resources.option;
            resources.ApplyResources(this.tsbSetting, "tsbSetting");
            this.tsbSetting.Name = "tsbSetting";
            // 
            // tsbOptionSetting
            // 
            this.tsbOptionSetting.Name = "tsbOptionSetting";
            resources.ApplyResources(this.tsbOptionSetting, "tsbOptionSetting");
            this.tsbOptionSetting.Click += new System.EventHandler(this.tsbOptionSetting_Click);
            // 
            // tsbGlobalHotkeySetting
            // 
            this.tsbGlobalHotkeySetting.Name = "tsbGlobalHotkeySetting";
            resources.ApplyResources(this.tsbGlobalHotkeySetting, "tsbGlobalHotkeySetting");
            this.tsbGlobalHotkeySetting.Click += new System.EventHandler(this.tsbGlobalHotkeySetting_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            // 
            // tsbBackupGuiNConfig
            // 
            this.tsbBackupGuiNConfig.Name = "tsbBackupGuiNConfig";
            resources.ApplyResources(this.tsbBackupGuiNConfig, "tsbBackupGuiNConfig");
            this.tsbBackupGuiNConfig.Click += new System.EventHandler(this.tsbBackupGuiNConfig_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // tsbCheckUpdate
            // 
            this.tsbCheckUpdate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCheckUpdateN,
            this.tsbCheckUpdateCore,
            this.tsbCheckUpdateMetaCore,
            this.toolStripSeparator15,
            this.tsbCheckUpdateGeo});
            this.tsbCheckUpdate.Image = global::clashN.Properties.Resources.checkupdate;
            resources.ApplyResources(this.tsbCheckUpdate, "tsbCheckUpdate");
            this.tsbCheckUpdate.Name = "tsbCheckUpdate";
            // 
            // tsbCheckUpdateN
            // 
            this.tsbCheckUpdateN.Name = "tsbCheckUpdateN";
            resources.ApplyResources(this.tsbCheckUpdateN, "tsbCheckUpdateN");
            this.tsbCheckUpdateN.Click += new System.EventHandler(this.tsbCheckUpdateN_Click);
            // 
            // tsbCheckUpdateCore
            // 
            this.tsbCheckUpdateCore.Name = "tsbCheckUpdateCore";
            resources.ApplyResources(this.tsbCheckUpdateCore, "tsbCheckUpdateCore");
            this.tsbCheckUpdateCore.Click += new System.EventHandler(this.tsbCheckUpdateCore_Click);
            // 
            // tsbCheckUpdateMetaCore
            // 
            this.tsbCheckUpdateMetaCore.Name = "tsbCheckUpdateMetaCore";
            resources.ApplyResources(this.tsbCheckUpdateMetaCore, "tsbCheckUpdateMetaCore");
            this.tsbCheckUpdateMetaCore.Click += new System.EventHandler(this.tsbCheckUpdateMetaCore_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            resources.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
            // 
            // tsbCheckUpdateGeo
            // 
            this.tsbCheckUpdateGeo.Name = "tsbCheckUpdateGeo";
            resources.ApplyResources(this.tsbCheckUpdateGeo, "tsbCheckUpdateGeo");
            this.tsbCheckUpdateGeo.Click += new System.EventHandler(this.tsbCheckUpdateGeo_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // tsbHelp
            // 
            this.tsbHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAbout,
            this.toolStripSeparator12,
            this.tsbLanguageDef,
            this.tsbLanguageZhHans});
            this.tsbHelp.Image = global::clashN.Properties.Resources.help;
            resources.ApplyResources(this.tsbHelp, "tsbHelp");
            this.tsbHelp.Name = "tsbHelp";
            // 
            // tsbAbout
            // 
            this.tsbAbout.Name = "tsbAbout";
            resources.ApplyResources(this.tsbAbout, "tsbAbout");
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // tsbLanguageDef
            // 
            this.tsbLanguageDef.Name = "tsbLanguageDef";
            resources.ApplyResources(this.tsbLanguageDef, "tsbLanguageDef");
            this.tsbLanguageDef.Click += new System.EventHandler(this.tsbLanguageDef_Click);
            // 
            // tsbLanguageZhHans
            // 
            this.tsbLanguageZhHans.Name = "tsbLanguageZhHans";
            resources.ApplyResources(this.tsbLanguageZhHans, "tsbLanguageZhHans");
            this.tsbLanguageZhHans.Click += new System.EventHandler(this.tsbLanguageZhHans_Click);
            // 
            // tsbPromotion
            // 
            this.tsbPromotion.ForeColor = System.Drawing.Color.Black;
            this.tsbPromotion.Image = global::clashN.Properties.Resources.promotion;
            resources.ApplyResources(this.tsbPromotion, "tsbPromotion");
            this.tsbPromotion.Name = "tsbPromotion";
            this.tsbPromotion.Click += new System.EventHandler(this.tsbPromotion_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::clashN.Properties.Resources.minimize;
            resources.ApplyResources(this.tsbClose, "tsbClose");
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scBig);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tsMain);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.scServers.Panel1.ResumeLayout(false);
            this.scServers.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scServers)).EndInit();
            this.scServers.ResumeLayout(false);
            this.cmsLv.ResumeLayout(false);
            this.scBig.Panel1.ResumeLayout(false);
            this.scBig.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scBig)).EndInit();
            this.scBig.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tpProxies.ResumeLayout(false);
            this.tpProfile.ResumeLayout(false);
            this.cmsMain.ResumeLayout(false);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion
        private clashN.Base.ListViewFlickerFree lvProfiles;
        private System.Windows.Forms.NotifyIcon notifyMain;
        private System.Windows.Forms.ContextMenuStrip cmsMain;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem menuProfiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip cmsLv;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveProfile;
        private System.Windows.Forms.ToolStripMenuItem menuSetDefaultProfile;
        private System.Windows.Forms.ToolStripMenuItem menuCopyProfile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripDropDownButton tsbProfile;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem menuMoveTop;
        private System.Windows.Forms.ToolStripMenuItem menuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem menuMoveDown;
        private System.Windows.Forms.ToolStripMenuItem menuMoveBottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuSysAgentMode;
        private System.Windows.Forms.ToolStripMenuItem menuGlobal;
        private System.Windows.Forms.ToolStripMenuItem menuKeepClear;
        private System.Windows.Forms.ToolStripMenuItem menuAddProfile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer scServers;
        private QRCodeControl qrCodeControl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripDropDownButton tsbCheckUpdate;
        private System.Windows.Forms.ToolStripMenuItem tsbCheckUpdateN;
        private System.Windows.Forms.ToolStripMenuItem tsbCheckUpdateCore;
        private System.Windows.Forms.ToolStripMenuItem menuAddProfileViaClipboard;
        private System.Windows.Forms.ToolStripMenuItem menuExport2Clipboard;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripDropDownButton tsbHelp;
        private System.Windows.Forms.ToolStripMenuItem tsbAbout;
        private System.Windows.Forms.ToolStripMenuItem menuScanScreen;
        private System.Windows.Forms.ToolStripMenuItem menuScanScreen2;
        private System.Windows.Forms.ToolStripDropDownButton tsbSub;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem tsbSubSetting;
        private System.Windows.Forms.ToolStripMenuItem menuSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem tsbLanguageDef;
        private System.Windows.Forms.ToolStripMenuItem tsbLanguageZhHans;
        private System.Windows.Forms.ToolStripButton tsbPromotion;
        private System.Windows.Forms.ToolStripMenuItem menuUpdateSubscriptions;
        private System.Windows.Forms.ToolStripMenuItem menuKeepNothing;
        private System.Windows.Forms.ToolStripButton tsbReload;
        private System.Windows.Forms.ToolStripButton tsbQRCodeSwitch;
        private System.Windows.Forms.ToolStripDropDownButton tsbSetting;
        private System.Windows.Forms.ToolStripMenuItem tsbOptionSetting;
        private System.Windows.Forms.ToolStripMenuItem tsbCheckUpdateMetaCore;
        private System.Windows.Forms.ToolStripMenuItem menuClearProfileStatistics;
        private System.Windows.Forms.ToolStripMenuItem menuRuleMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem tsbBackupGuiNConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem tsbCheckUpdateGeo;
        private System.Windows.Forms.SplitContainer scBig;
        private System.Windows.Forms.ToolStripMenuItem menuUpdateSubViaProxy;
        private System.Windows.Forms.ToolStripMenuItem tsbGlobalHotkeySetting;
        private System.Windows.Forms.ToolStripMenuItem tsbSubUpdate;
        private System.Windows.Forms.ToolStripMenuItem tsbSubUpdateViaProxy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private MainMsgControl mainMsgControl;
        private System.Windows.Forms.ToolStripMenuItem tsbSubUpdateSelected;
        private System.Windows.Forms.ToolStripMenuItem tsbSubUpdateViaProxySelected;
        private System.Windows.Forms.ToolStripMenuItem menuModeKeep;
        private System.Windows.Forms.ToolStripMenuItem menuModeRule;
        private System.Windows.Forms.ToolStripMenuItem menuModeGlobal;
        private System.Windows.Forms.ToolStripMenuItem menuModeDirect;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpProxies;
        private System.Windows.Forms.TabPage tpProfile;
        private ProxiesControl proxiesControl;
        private System.Windows.Forms.ToolStripDropDownButton tsbCurrentProxies;
        private System.Windows.Forms.ToolStripMenuItem tsbProxiesReload;
        private System.Windows.Forms.ToolStripMenuItem tsbProxiesSpeedtest;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
    }
}

