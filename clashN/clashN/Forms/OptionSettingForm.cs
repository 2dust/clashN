using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using clashN.Base;
using clashN.Handler;
using clashN.Mode;
using clashN.Resx;

namespace clashN.Forms
{
    public partial class OptionSettingForm : BaseForm
    {
        public OptionSettingForm()
        {
            InitializeComponent();
        }

        private void OptionSettingForm_Load(object sender, EventArgs e)
        {
            cmbSubConvertUrl.Items.AddRange(Global.SubConvertUrls.ToArray());

            InitBase();

            InitGUI();

        }

        /// <summary>
        /// 初始化基础设置
        /// </summary>
        private void InitBase()
        {
            //日志
            cmbloglevel.Text = config.logLevel;

            //本地监听
            txtmixedPort.Text = config.mixedPort.ToString();
            txthttpPort.Text = config.httpPort.ToString();
            txtsocksPort.Text = config.socksPort.ToString();
            txtAPIPort.Text = config.APIPort.ToString();

            chkAllowLANConn.Checked = config.allowLANConn;
            chkEnableIpv6.Checked = config.enableIpv6;

            txtsystemProxyExceptions.Text = config.systemProxyExceptions;
        }

        /// <summary>
        /// 初始化clashN GUI设置
        /// </summary>
        private void InitGUI()
        {
            //开机自动启动
            chkAutoRun.Checked = Utils.IsAutoRun();

            chkEnableStatistics.Checked = config.enableStatistics;
            chkKeepOlderDedupl.Checked = config.keepOlderDedupl;

            chkIgnoreGeoUpdateCore.Checked = config.ignoreGeoUpdateCore;
            txtautoUpdateInterval.Text = config.autoUpdateInterval.ToString();
            txtautoUpdateSubInterval.Text = config.autoUpdateSubInterval.ToString();
            chkEnableSecurityProtocolTls13.Checked = config.enableSecurityProtocolTls13;

            cmbSubConvertUrl.Text = config.constItem.subConvertUrl;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (SaveBase() != 0)
            {
                return;
            }

            if (SaveGUI() != 0)
            {
                return;
            }

            if (ConfigHandler.SaveConfig(ref config) == 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                UI.ShowWarning(ResUI.OperationFailed);
            }
        }

        /// <summary>
        /// 保存基础设置
        /// </summary>
        /// <returns></returns>
        private int SaveBase()
        {
            //日志
            string loglevel = cmbloglevel.Text.TrimEx();

            //本地监听
            string mixedPort = txtmixedPort.Text.TrimEx();
            string httpPort = txthttpPort.Text.TrimEx();
            string socksPort = txtsocksPort.Text.TrimEx();
            string APIPort = txtAPIPort.Text.TrimEx();
            if (Utils.IsNullOrEmpty(mixedPort) || !Utils.IsNumberic(mixedPort))
            {
                UI.Show(ResUI.FillLocalListeningPort);
                return -1;
            }
            if (Utils.IsNullOrEmpty(httpPort) || !Utils.IsNumberic(httpPort))
            {
                UI.Show(ResUI.FillLocalListeningPort);
                return -1;
            }
            if (Utils.IsNullOrEmpty(socksPort) || !Utils.IsNumberic(socksPort))
            {
                UI.Show(ResUI.FillLocalListeningPort);
                return -1;
            }
            if (Utils.IsNullOrEmpty(APIPort) || !Utils.IsNumberic(APIPort))
            {
                UI.Show(ResUI.FillLocalListeningPort);
                return -1;
            }

            config.mixedPort = Utils.ToInt(mixedPort);
            config.httpPort = Utils.ToInt(httpPort);
            config.socksPort = Utils.ToInt(socksPort);
            config.APIPort = Utils.ToInt(APIPort);

            config.logLevel = loglevel;


            config.allowLANConn = chkAllowLANConn.Checked;
            config.enableIpv6 = chkEnableIpv6.Checked;

            config.systemProxyExceptions = txtsystemProxyExceptions.Text.TrimEx();

            return 0;
        }

        /// <summary>
        /// 保存GUI设置
        /// </summary>
        /// <returns></returns>
        private int SaveGUI()
        {
            //开机自动启动
            Utils.SetAutoRun(chkAutoRun.Checked);


            bool lastEnableStatistics = config.enableStatistics;
            config.enableStatistics = chkEnableStatistics.Checked;
            config.keepOlderDedupl = chkKeepOlderDedupl.Checked;

            config.ignoreGeoUpdateCore = chkIgnoreGeoUpdateCore.Checked;
            config.autoUpdateInterval = Utils.ToInt(txtautoUpdateInterval.Text);
            config.autoUpdateSubInterval = Utils.ToInt(txtautoUpdateSubInterval.Text);
            config.enableSecurityProtocolTls13 = chkEnableSecurityProtocolTls13.Checked;

            config.constItem.subConvertUrl = cmbSubConvertUrl.Text.TrimEx();
            return 0;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnSetLoopback_Click(object sender, EventArgs e)
        {
            Utils.ProcessStart(Utils.GetPath("EnableLoopback.exe"));
        }

        private void btnFontSetting_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowColor = false;
            fontDialog.Font = btnFontSetting.Font;

            if (fontDialog.ShowDialog() != DialogResult.Cancel)
            {
                btnFontSetting.Font = fontDialog.Font;
                Utils.RegWriteValue(Global.MyRegPath, Global.MyRegKeyFont, Utils.ToJson(fontDialog.Font));
            }
        }

        private void btnFontReset_Click(object sender, EventArgs e)
        {
            Utils.RegWriteValue(Global.MyRegPath, Global.MyRegKeyFont, null);
            UI.Show(ResUI.OperationSuccess);

        }
    }
}
