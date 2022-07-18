namespace clashN.Forms
{
    partial class ProxiesControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProxiesControl));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvProxies = new clashN.Base.ListViewFlickerFree();
            this.cmsProxies = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsbProxiesReload = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbProxiesSpeedtest = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbProxiesSelectActivity = new System.Windows.Forms.ToolStripMenuItem();
            this.lvDetail = new clashN.Base.ListViewFlickerFree();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmsProxies.SuspendLayout();
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
            this.lvProxies.ContextMenuStrip = this.cmsProxies;
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
            // cmsProxies
            // 
            this.cmsProxies.ImageScalingSize = new System.Drawing.Size(20, 20);
            resources.ApplyResources(this.cmsProxies, "cmsProxies");
            this.cmsProxies.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbProxiesReload,
            this.tsbProxiesSpeedtest,
            this.tsbProxiesSelectActivity});
            this.cmsProxies.Name = "contextMenuStrip1";
            this.cmsProxies.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.cmsProxies.ShowCheckMargin = true;
            this.cmsProxies.ShowImageMargin = false;
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
            // tsbProxiesSelectActivity
            // 
            this.tsbProxiesSelectActivity.Name = "tsbProxiesSelectActivity";
            resources.ApplyResources(this.tsbProxiesSelectActivity, "tsbProxiesSelectActivity");
            this.tsbProxiesSelectActivity.Click += new System.EventHandler(this.tsbProxiesSelectActivity_Click);
            // 
            // lvDetail
            // 
            this.lvDetail.ContextMenuStrip = this.cmsProxies;
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
            this.lvDetail.SelectedIndexChanged += new System.EventHandler(this.lvDetail_SelectedIndexChanged);
            this.lvDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvDetail_KeyDown);
            // 
            // ProxiesControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ProxiesControl";
            this.Load += new System.EventHandler(this.ProxiesControl_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cmsProxies.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Base.ListViewFlickerFree lvProxies;
        private Base.ListViewFlickerFree lvDetail;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip cmsProxies;
        private System.Windows.Forms.ToolStripMenuItem tsbProxiesReload;
        private System.Windows.Forms.ToolStripMenuItem tsbProxiesSpeedtest;
        private System.Windows.Forms.ToolStripMenuItem tsbProxiesSelectActivity;
    }
}