namespace clashN.Forms
{
    partial class ProxiesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxiesForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvProxies = new clashN.Base.ListViewFlickerFree();
            this.lvDetail = new clashN.Base.ListViewFlickerFree();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSpeedtest = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSelectActivity = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvProxies);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvDetail);
            // 
            // lvProxies
            // 
            resources.ApplyResources(this.lvProxies, "lvProxies");
            this.lvProxies.FullRowSelect = true;
            this.lvProxies.GridLines = true;
            this.lvProxies.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvProxies.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvProxies.Items")))});
            this.lvProxies.MultiSelect = false;
            this.lvProxies.Name = "lvProxies";
            this.lvProxies.UseCompatibleStateImageBehavior = false;
            this.lvProxies.View = System.Windows.Forms.View.Details;
            this.lvProxies.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvProxies_ColumnClick);
            this.lvProxies.SelectedIndexChanged += new System.EventHandler(this.lvProxies_SelectedIndexChanged);
            // 
            // lvDetail
            // 
            resources.ApplyResources(this.lvDetail, "lvDetail");
            this.lvDetail.FullRowSelect = true;
            this.lvDetail.GridLines = true;
            this.lvDetail.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvDetail.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            ((System.Windows.Forms.ListViewItem)(resources.GetObject("lvDetail.Items")))});
            this.lvDetail.MultiSelect = false;
            this.lvDetail.Name = "lvDetail";
            this.lvDetail.UseCompatibleStateImageBehavior = false;
            this.lvDetail.View = System.Windows.Forms.View.Details;
            this.lvDetail.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvDetail_ColumnClick);
            this.lvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvDetail_KeyDown);
            // 
            // tsMain
            // 
            this.tsMain.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbReload,
            this.toolStripSeparator2,
            this.tsbSpeedtest,
            this.toolStripSeparator3,
            this.tsbSelectActivity,
            this.toolStripSeparator1,
            this.tsbClose});
            resources.ApplyResources(this.tsMain, "tsMain");
            this.tsMain.Name = "tsMain";
            this.tsMain.TabStop = true;
            // 
            // tsbReload
            // 
            this.tsbReload.Image = global::clashN.Properties.Resources.restart;
            resources.ApplyResources(this.tsbReload, "tsbReload");
            this.tsbReload.Name = "tsbReload";
            this.tsbReload.Click += new System.EventHandler(this.tsbReload_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsbSpeedtest
            // 
            this.tsbSpeedtest.Image = global::clashN.Properties.Resources.speedtest;
            resources.ApplyResources(this.tsbSpeedtest, "tsbSpeedtest");
            this.tsbSpeedtest.Name = "tsbSpeedtest";
            this.tsbSpeedtest.Click += new System.EventHandler(this.tsbSpeedtest_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsbSelectActivity
            // 
            this.tsbSelectActivity.Image = global::clashN.Properties.Resources.active;
            resources.ApplyResources(this.tsbSelectActivity, "tsbSelectActivity");
            this.tsbSelectActivity.Name = "tsbSelectActivity";
            this.tsbSelectActivity.Click += new System.EventHandler(this.tsbSelectActivity_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::clashN.Properties.Resources.minimize;
            resources.ApplyResources(this.tsbClose, "tsbClose");
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // ProxiesForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tsMain);
            this.Name = "ProxiesForm";
            this.Load += new System.EventHandler(this.ProxiesForm_Load);
            this.Shown += new System.EventHandler(this.ProxiesForm_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Base.ListViewFlickerFree lvProxies;
        private Base.ListViewFlickerFree lvDetail;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbReload;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripButton tsbSpeedtest;
        private System.Windows.Forms.ToolStripButton tsbSelectActivity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}