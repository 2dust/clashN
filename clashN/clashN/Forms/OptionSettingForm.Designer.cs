namespace clashN.Forms
{
    partial class OptionSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionSettingForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEnableIpv6 = new System.Windows.Forms.CheckBox();
            this.txtmixedPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAPIPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAllowLANConn = new System.Windows.Forms.CheckBox();
            this.txtsocksPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbloglevel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txthttpPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbSubConvertUrl = new System.Windows.Forms.ComboBox();
            this.btnFontReset = new System.Windows.Forms.Button();
            this.txtautoUpdateSubInterval = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFontSetting = new System.Windows.Forms.Button();
            this.chkEnableSecurityProtocolTls13 = new System.Windows.Forms.CheckBox();
            this.btnSetLoopback = new System.Windows.Forms.Button();
            this.txtautoUpdateInterval = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chkIgnoreGeoUpdateCore = new System.Windows.Forms.CheckBox();
            this.chkEnableStatistics = new System.Windows.Forms.CheckBox();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtsystemProxyExceptions = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.chkEnableIpv6);
            this.groupBox1.Controls.Add(this.txtmixedPort);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtAPIPort);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkAllowLANConn);
            this.groupBox1.Controls.Add(this.txtsocksPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbloglevel);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txthttpPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // chkEnableIpv6
            // 
            resources.ApplyResources(this.chkEnableIpv6, "chkEnableIpv6");
            this.chkEnableIpv6.Name = "chkEnableIpv6";
            this.chkEnableIpv6.UseVisualStyleBackColor = true;
            // 
            // txtmixedPort
            // 
            resources.ApplyResources(this.txtmixedPort, "txtmixedPort");
            this.txtmixedPort.Name = "txtmixedPort";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtAPIPort
            // 
            resources.ApplyResources(this.txtAPIPort, "txtAPIPort");
            this.txtAPIPort.Name = "txtAPIPort";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // chkAllowLANConn
            // 
            resources.ApplyResources(this.chkAllowLANConn, "chkAllowLANConn");
            this.chkAllowLANConn.Name = "chkAllowLANConn";
            this.chkAllowLANConn.UseVisualStyleBackColor = true;
            // 
            // txtsocksPort
            // 
            resources.ApplyResources(this.txtsocksPort, "txtsocksPort");
            this.txtsocksPort.Name = "txtsocksPort";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbloglevel
            // 
            resources.ApplyResources(this.cmbloglevel, "cmbloglevel");
            this.cmbloglevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbloglevel.FormattingEnabled = true;
            this.cmbloglevel.Items.AddRange(new object[] {
            resources.GetString("cmbloglevel.Items"),
            resources.GetString("cmbloglevel.Items1"),
            resources.GetString("cmbloglevel.Items2"),
            resources.GetString("cmbloglevel.Items3"),
            resources.GetString("cmbloglevel.Items4")});
            this.cmbloglevel.Name = "cmbloglevel";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txthttpPort
            // 
            resources.ApplyResources(this.txthttpPort, "txthttpPort");
            this.txthttpPort.Name = "txthttpPort";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tabPage7
            // 
            resources.ApplyResources(this.tabPage7, "tabPage7");
            this.tabPage7.Controls.Add(this.label7);
            this.tabPage7.Controls.Add(this.cmbSubConvertUrl);
            this.tabPage7.Controls.Add(this.btnFontReset);
            this.tabPage7.Controls.Add(this.txtautoUpdateSubInterval);
            this.tabPage7.Controls.Add(this.label4);
            this.tabPage7.Controls.Add(this.btnFontSetting);
            this.tabPage7.Controls.Add(this.chkEnableSecurityProtocolTls13);
            this.tabPage7.Controls.Add(this.btnSetLoopback);
            this.tabPage7.Controls.Add(this.txtautoUpdateInterval);
            this.tabPage7.Controls.Add(this.label15);
            this.tabPage7.Controls.Add(this.chkIgnoreGeoUpdateCore);
            this.tabPage7.Controls.Add(this.chkEnableStatistics);
            this.tabPage7.Controls.Add(this.chkAutoRun);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbSubConvertUrl
            // 
            resources.ApplyResources(this.cmbSubConvertUrl, "cmbSubConvertUrl");
            this.cmbSubConvertUrl.FormattingEnabled = true;
            this.cmbSubConvertUrl.Name = "cmbSubConvertUrl";
            // 
            // btnFontReset
            // 
            resources.ApplyResources(this.btnFontReset, "btnFontReset");
            this.btnFontReset.Name = "btnFontReset";
            this.btnFontReset.UseVisualStyleBackColor = true;
            this.btnFontReset.Click += new System.EventHandler(this.btnFontReset_Click);
            // 
            // txtautoUpdateSubInterval
            // 
            resources.ApplyResources(this.txtautoUpdateSubInterval, "txtautoUpdateSubInterval");
            this.txtautoUpdateSubInterval.Name = "txtautoUpdateSubInterval";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // btnFontSetting
            // 
            resources.ApplyResources(this.btnFontSetting, "btnFontSetting");
            this.btnFontSetting.Name = "btnFontSetting";
            this.btnFontSetting.UseVisualStyleBackColor = true;
            this.btnFontSetting.Click += new System.EventHandler(this.btnFontSetting_Click);
            // 
            // chkEnableSecurityProtocolTls13
            // 
            resources.ApplyResources(this.chkEnableSecurityProtocolTls13, "chkEnableSecurityProtocolTls13");
            this.chkEnableSecurityProtocolTls13.Name = "chkEnableSecurityProtocolTls13";
            this.chkEnableSecurityProtocolTls13.UseVisualStyleBackColor = true;
            // 
            // btnSetLoopback
            // 
            resources.ApplyResources(this.btnSetLoopback, "btnSetLoopback");
            this.btnSetLoopback.Name = "btnSetLoopback";
            this.btnSetLoopback.UseVisualStyleBackColor = true;
            this.btnSetLoopback.Click += new System.EventHandler(this.btnSetLoopback_Click);
            // 
            // txtautoUpdateInterval
            // 
            resources.ApplyResources(this.txtautoUpdateInterval, "txtautoUpdateInterval");
            this.txtautoUpdateInterval.Name = "txtautoUpdateInterval";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // chkIgnoreGeoUpdateCore
            // 
            resources.ApplyResources(this.chkIgnoreGeoUpdateCore, "chkIgnoreGeoUpdateCore");
            this.chkIgnoreGeoUpdateCore.Name = "chkIgnoreGeoUpdateCore";
            this.chkIgnoreGeoUpdateCore.UseVisualStyleBackColor = true;
            // 
            // chkEnableStatistics
            // 
            resources.ApplyResources(this.chkEnableStatistics, "chkEnableStatistics");
            this.chkEnableStatistics.Name = "chkEnableStatistics";
            this.chkEnableStatistics.UseVisualStyleBackColor = true;
            // 
            // chkAutoRun
            // 
            resources.ApplyResources(this.chkAutoRun, "chkAutoRun");
            this.chkAutoRun.Name = "chkAutoRun";
            this.chkAutoRun.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtsystemProxyExceptions);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // txtsystemProxyExceptions
            // 
            resources.ApplyResources(this.txtsystemProxyExceptions, "txtsystemProxyExceptions");
            this.txtsystemProxyExceptions.Name = "txtsystemProxyExceptions";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Name = "panel2";
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // OptionSettingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "OptionSettingForm";
            this.Load += new System.EventHandler(this.OptionSettingForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbloglevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txthttpPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.CheckBox chkAutoRun;
        private System.Windows.Forms.CheckBox chkAllowLANConn;
        private System.Windows.Forms.CheckBox chkEnableStatistics;
        private System.Windows.Forms.CheckBox chkIgnoreGeoUpdateCore;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtsystemProxyExceptions;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtautoUpdateInterval;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnSetLoopback;
        private System.Windows.Forms.CheckBox chkEnableSecurityProtocolTls13;
        private System.Windows.Forms.TextBox txtsocksPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAPIPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFontSetting;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TextBox txtautoUpdateSubInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnFontReset;
        private System.Windows.Forms.TextBox txtmixedPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkEnableIpv6;
        private System.Windows.Forms.ComboBox cmbSubConvertUrl;
        private System.Windows.Forms.Label label7;
    }
}