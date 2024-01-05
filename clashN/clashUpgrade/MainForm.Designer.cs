namespace clashUpgrade
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            ExitBtn = new System.Windows.Forms.Button();
            UpgradeBtn = new System.Windows.Forms.Button();
            CnTip = new System.Windows.Forms.Label();
            EnTip = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // ExitBtn
            // 
            ExitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            ExitBtn.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            ExitBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            ExitBtn.Location = new System.Drawing.Point(393, 112);
            ExitBtn.Name = "ExitBtn";
            ExitBtn.Size = new System.Drawing.Size(365, 100);
            ExitBtn.TabIndex = 1;
            ExitBtn.Text = "Exit (退出)";
            ExitBtn.UseVisualStyleBackColor = true;
            ExitBtn.Click += ExitBtn_Click;
            // 
            // UpgradeBtn
            // 
            UpgradeBtn.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            UpgradeBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            UpgradeBtn.Location = new System.Drawing.Point(16, 112);
            UpgradeBtn.Name = "UpgradeBtn";
            UpgradeBtn.Size = new System.Drawing.Size(365, 100);
            UpgradeBtn.TabIndex = 0;
            UpgradeBtn.Text = "Upgrade (升级)";
            UpgradeBtn.UseVisualStyleBackColor = true;
            UpgradeBtn.Click += UpgradeBtn_Click;
            // 
            // CnTip
            // 
            CnTip.AutoSize = true;
            CnTip.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            CnTip.Location = new System.Drawing.Point(16, 60);
            CnTip.Name = "CnTip";
            CnTip.Size = new System.Drawing.Size(528, 39);
            CnTip.TabIndex = 8;
            CnTip.Text = "Frok ClashN 会在成功升级后自动重启";
            // 
            // EnTip
            // 
            EnTip.AutoSize = true;
            EnTip.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            EnTip.Location = new System.Drawing.Point(16, 9);
            EnTip.Name = "EnTip";
            EnTip.Size = new System.Drawing.Size(753, 39);
            EnTip.TabIndex = 9;
            EnTip.Text = "Frok ClashN will automatically restart after upgrade";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(774, 229);
            Controls.Add(EnTip);
            Controls.Add(CnTip);
            Controls.Add(ExitBtn);
            Controls.Add(UpgradeBtn);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Frok ClashN Upgrade";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button ExitBtn;
        private System.Windows.Forms.Button UpgradeBtn;
        private System.Windows.Forms.Label CnTip;
        private System.Windows.Forms.Label EnTip;
    }
}

