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
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.lvProfiles = new clashN.Base.ListViewFlickerFree();
            this.cmsLv = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuAddProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddProfileViaClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.menuScanScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExport2Clipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSubUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSubUpdateViaProxy = new System.Windows.Forms.ToolStripMenuItem();
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
            this.qrCodeControl = new clashN.Forms.QRCodeControl();
            this.tsbProfile = new System.Windows.Forms.ToolStripDropDownButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gbMsgTitle = new System.Windows.Forms.GroupBox();
            this.txtMsgBox = new System.Windows.Forms.TextBox();
            this.cmsMsgBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuMsgBoxSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMsgBoxCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMsgBoxCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMsgBoxClear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMsgBoxFilter = new System.Windows.Forms.ToolStripMenuItem();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.toolSslInboundInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSslBlank1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSslRoutingRule = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSslBlank2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSslProfileSpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSslBlank4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuSysAgentMode = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKeepClear = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGlobal = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKeepNothing = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRoutings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProfiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProfiles2 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAddProfiles2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuScanScreen2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpdateSubscriptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpdateSubViaProxy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbCurrentProxies = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbQRCodeSwitch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSetting = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbOptionSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbGlobalHotkeySetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbBackupGuiNConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCheckUpdate = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbCheckUpdateN = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCheckUpdateCore = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCheckUpdateMetaCore = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCheckUpdateGeoSite = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCheckUpdateGeoIP = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbHelp = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLanguageDef = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbLanguageZhHans = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbPromotion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.cmsLv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbMsgTitle.SuspendLayout();
            this.cmsMsgBox.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.cmsMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // scMain
            // 
            resources.ApplyResources(this.scMain, "scMain");
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.lvProfiles);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.qrCodeControl);
            this.scMain.TabStop = false;
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
            this.tsbSubUpdateViaProxy,
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
            // tsbSubUpdateViaProxy
            // 
            this.tsbSubUpdateViaProxy.Name = "tsbSubUpdateViaProxy";
            resources.ApplyResources(this.tsbSubUpdateViaProxy, "tsbSubUpdateViaProxy");
            this.tsbSubUpdateViaProxy.Click += new System.EventHandler(this.tsbSubUpdateViaProxy_Click);
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
            // qrCodeControl
            // 
            resources.ApplyResources(this.qrCodeControl, "qrCodeControl");
            this.qrCodeControl.Name = "qrCodeControl";
            // 
            // tsbProfile
            // 
            this.tsbProfile.DropDown = this.cmsLv;
            this.tsbProfile.Image = global::clashN.Properties.Resources.server;
            resources.ApplyResources(this.tsbProfile, "tsbProfile");
            this.tsbProfile.Name = "tsbProfile";
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gbMsgTitle);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.scMain);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // gbMsgTitle
            // 
            this.gbMsgTitle.Controls.Add(this.txtMsgBox);
            this.gbMsgTitle.Controls.Add(this.ssMain);
            resources.ApplyResources(this.gbMsgTitle, "gbMsgTitle");
            this.gbMsgTitle.Name = "gbMsgTitle";
            this.gbMsgTitle.TabStop = false;
            // 
            // txtMsgBox
            // 
            this.txtMsgBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(49)))), ((int)(((byte)(52)))));
            this.txtMsgBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMsgBox.ContextMenuStrip = this.cmsMsgBox;
            resources.ApplyResources(this.txtMsgBox, "txtMsgBox");
            this.txtMsgBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(226)))), ((int)(((byte)(228)))));
            this.txtMsgBox.Name = "txtMsgBox";
            this.txtMsgBox.ReadOnly = true;
            this.txtMsgBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMsgBox_KeyDown);
            // 
            // cmsMsgBox
            // 
            this.cmsMsgBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMsgBoxSelectAll,
            this.menuMsgBoxCopy,
            this.menuMsgBoxCopyAll,
            this.menuMsgBoxClear,
            this.menuMsgBoxFilter});
            this.cmsMsgBox.Name = "cmsMsgBox";
            resources.ApplyResources(this.cmsMsgBox, "cmsMsgBox");
            // 
            // menuMsgBoxSelectAll
            // 
            this.menuMsgBoxSelectAll.Name = "menuMsgBoxSelectAll";
            resources.ApplyResources(this.menuMsgBoxSelectAll, "menuMsgBoxSelectAll");
            this.menuMsgBoxSelectAll.Click += new System.EventHandler(this.menuMsgBoxSelectAll_Click);
            // 
            // menuMsgBoxCopy
            // 
            this.menuMsgBoxCopy.Name = "menuMsgBoxCopy";
            resources.ApplyResources(this.menuMsgBoxCopy, "menuMsgBoxCopy");
            this.menuMsgBoxCopy.Click += new System.EventHandler(this.menuMsgBoxCopy_Click);
            // 
            // menuMsgBoxCopyAll
            // 
            this.menuMsgBoxCopyAll.Name = "menuMsgBoxCopyAll";
            resources.ApplyResources(this.menuMsgBoxCopyAll, "menuMsgBoxCopyAll");
            this.menuMsgBoxCopyAll.Click += new System.EventHandler(this.menuMsgBoxCopyAll_Click);
            // 
            // menuMsgBoxClear
            // 
            this.menuMsgBoxClear.Name = "menuMsgBoxClear";
            resources.ApplyResources(this.menuMsgBoxClear, "menuMsgBoxClear");
            this.menuMsgBoxClear.Click += new System.EventHandler(this.menuMsgBoxClear_Click);
            // 
            // menuMsgBoxFilter
            // 
            this.menuMsgBoxFilter.Name = "menuMsgBoxFilter";
            resources.ApplyResources(this.menuMsgBoxFilter, "menuMsgBoxFilter");
            this.menuMsgBoxFilter.Click += new System.EventHandler(this.menuMsgBoxFilter_Click);
            // 
            // ssMain
            // 
            this.ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolSslInboundInfo,
            this.toolSslBlank1,
            this.toolSslRoutingRule,
            this.toolSslBlank2,
            this.toolSslProfileSpeed,
            this.toolSslBlank4});
            resources.ApplyResources(this.ssMain, "ssMain");
            this.ssMain.Name = "ssMain";
            this.ssMain.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ssMain_ItemClicked);
            // 
            // toolSslInboundInfo
            // 
            this.toolSslInboundInfo.Name = "toolSslInboundInfo";
            resources.ApplyResources(this.toolSslInboundInfo, "toolSslInboundInfo");
            // 
            // toolSslBlank1
            // 
            resources.ApplyResources(this.toolSslBlank1, "toolSslBlank1");
            this.toolSslBlank1.Name = "toolSslBlank1";
            this.toolSslBlank1.Spring = true;
            // 
            // toolSslRoutingRule
            // 
            this.toolSslRoutingRule.Name = "toolSslRoutingRule";
            resources.ApplyResources(this.toolSslRoutingRule, "toolSslRoutingRule");
            // 
            // toolSslBlank2
            // 
            this.toolSslBlank2.Name = "toolSslBlank2";
            resources.ApplyResources(this.toolSslBlank2, "toolSslBlank2");
            this.toolSslBlank2.Spring = true;
            // 
            // toolSslProfileSpeed
            // 
            resources.ApplyResources(this.toolSslProfileSpeed, "toolSslProfileSpeed");
            this.toolSslProfileSpeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolSslProfileSpeed.Name = "toolSslProfileSpeed";
            // 
            // toolSslBlank4
            // 
            this.toolSslBlank4.Name = "toolSslBlank4";
            resources.ApplyResources(this.toolSslBlank4, "toolSslBlank4");
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
            this.menuRoutings,
            this.menuProfiles,
            this.menuProfiles2,
            this.toolStripSeparator13,
            this.menuAddProfiles2,
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
            // menuRoutings
            // 
            this.menuRoutings.Name = "menuRoutings";
            resources.ApplyResources(this.menuRoutings, "menuRoutings");
            // 
            // menuProfiles
            // 
            this.menuProfiles.Name = "menuProfiles";
            resources.ApplyResources(this.menuProfiles, "menuProfiles");
            // 
            // menuProfiles2
            // 
            this.menuProfiles2.BackColor = System.Drawing.SystemColors.Window;
            this.menuProfiles2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.menuProfiles2.DropDownWidth = 500;
            resources.ApplyResources(this.menuProfiles2, "menuProfiles2");
            this.menuProfiles2.Name = "menuProfiles2";
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // menuAddProfiles2
            // 
            this.menuAddProfiles2.Name = "menuAddProfiles2";
            resources.ApplyResources(this.menuAddProfiles2, "menuAddProfiles2");
            this.menuAddProfiles2.Click += new System.EventHandler(this.menuAddProfiles_Click);
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
            this.tsbProfile,
            this.tsbCurrentProxies,
            this.toolStripSeparator6,
            this.tsbQRCodeSwitch,
            this.toolStripSeparator8,
            this.tsbSetting,
            this.toolStripSeparator5,
            this.tsbReload,
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
            this.tsbCurrentProxies.Image = global::clashN.Properties.Resources.active;
            resources.ApplyResources(this.tsbCurrentProxies, "tsbCurrentProxies");
            this.tsbCurrentProxies.Name = "tsbCurrentProxies";
            this.tsbCurrentProxies.Click += new System.EventHandler(this.tsbCurrentProxies_Click);
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
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // tsbReload
            // 
            this.tsbReload.Image = global::clashN.Properties.Resources.restart;
            resources.ApplyResources(this.tsbReload, "tsbReload");
            this.tsbReload.Name = "tsbReload";
            this.tsbReload.Click += new System.EventHandler(this.tsbReload_Click);
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
            this.tsbCheckUpdateGeoSite,
            this.tsbCheckUpdateGeoIP});
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
            // tsbCheckUpdateGeoSite
            // 
            this.tsbCheckUpdateGeoSite.Name = "tsbCheckUpdateGeoSite";
            resources.ApplyResources(this.tsbCheckUpdateGeoSite, "tsbCheckUpdateGeoSite");
            this.tsbCheckUpdateGeoSite.Click += new System.EventHandler(this.tsbCheckUpdateGeoSite_Click);
            // 
            // tsbCheckUpdateGeoIP
            // 
            this.tsbCheckUpdateGeoIP.Name = "tsbCheckUpdateGeoIP";
            resources.ApplyResources(this.tsbCheckUpdateGeoIP, "tsbCheckUpdateGeoIP");
            this.tsbCheckUpdateGeoIP.Click += new System.EventHandler(this.tsbCheckUpdateGeoIP_Click);
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
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tsMain);
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.cmsLv.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.gbMsgTitle.ResumeLayout(false);
            this.gbMsgTitle.PerformLayout();
            this.cmsMsgBox.ResumeLayout(false);
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.cmsMain.ResumeLayout(false);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbMsgTitle;
        private System.Windows.Forms.TextBox txtMsgBox;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
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
        private System.Windows.Forms.SplitContainer scMain;
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
        private System.Windows.Forms.ToolStripMenuItem menuAddProfiles2;
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
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel toolSslBlank1;
        private System.Windows.Forms.ToolStripStatusLabel toolSslProfileSpeed;
        private System.Windows.Forms.ToolStripStatusLabel toolSslBlank4;
        private System.Windows.Forms.ToolStripMenuItem menuUpdateSubscriptions;
        private System.Windows.Forms.ToolStripMenuItem menuKeepNothing;
        private System.Windows.Forms.ToolStripButton tsbReload;
        private System.Windows.Forms.ToolStripButton tsbQRCodeSwitch;
        private System.Windows.Forms.ToolStripDropDownButton tsbSetting;
        private System.Windows.Forms.ToolStripMenuItem tsbOptionSetting;
        private System.Windows.Forms.ToolStripMenuItem tsbCheckUpdateMetaCore;
        private System.Windows.Forms.ToolStripMenuItem menuClearProfileStatistics;
        private System.Windows.Forms.ToolStripMenuItem menuRoutings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ContextMenuStrip cmsMsgBox;
        private System.Windows.Forms.ToolStripMenuItem menuMsgBoxSelectAll;
        private System.Windows.Forms.ToolStripMenuItem menuMsgBoxCopy;
        private System.Windows.Forms.ToolStripMenuItem menuMsgBoxCopyAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem tsbBackupGuiNConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem tsbCheckUpdateGeoSite;
        private System.Windows.Forms.ToolStripMenuItem tsbCheckUpdateGeoIP;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem menuMsgBoxFilter;
        private System.Windows.Forms.ToolStripStatusLabel toolSslInboundInfo;
        private System.Windows.Forms.ToolStripStatusLabel toolSslRoutingRule;
        private System.Windows.Forms.ToolStripStatusLabel toolSslBlank2;
        private System.Windows.Forms.ToolStripComboBox menuProfiles2;
        private System.Windows.Forms.ToolStripMenuItem menuUpdateSubViaProxy;
        private System.Windows.Forms.ToolStripMenuItem menuMsgBoxClear;
        private System.Windows.Forms.ToolStripMenuItem tsbGlobalHotkeySetting;
        private System.Windows.Forms.ToolStripMenuItem tsbSubUpdate;
        private System.Windows.Forms.ToolStripMenuItem tsbSubUpdateViaProxy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbCurrentProxies;
    }
}

