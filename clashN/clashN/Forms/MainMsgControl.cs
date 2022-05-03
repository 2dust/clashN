using clashN.Base;
using clashN.Mode;
using clashN.Resx;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace clashN.Forms
{
    public partial class MainMsgControl : UserControl
    {
        private string MsgFilter = string.Empty;
        delegate void AppendTextDelegate(string text);

        public MainMsgControl()
        {
            InitializeComponent();
        }

        private void MainMsgControl_Load(object sender, EventArgs e)
        {

        }

        #region 提示信息

        public void AppendText(string text)
        {
            if (this.txtMsgBox.InvokeRequired)
            {
                Invoke(new AppendTextDelegate(AppendText), new object[] { text });
            }
            else
            {
                if (!Utils.IsNullOrEmpty(MsgFilter))
                {
                    if (!Regex.IsMatch(text, MsgFilter))
                    {
                        return;
                    }
                }
                //this.txtMsgBox.AppendText(text);
                ShowMsg(text);
            }
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
        public void ClearMsg()
        {
            txtMsgBox.Invoke((Action)delegate
            {
                txtMsgBox.Clear();
            });
        }

        public void DisplayToolStatus(Config config)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"mixed {Global.Loopback}:{config.mixedPort}");
            sb.Append(" | ");
            sb.Append($"{Global.InboundSocks} {Global.Loopback}:{config.socksPort}");
            sb.Append(" | ");
            sb.Append($"{ Global.InboundHttp} { Global.Loopback}:{config.httpPort}");

            SetToolSslInfo("inbound", sb.ToString());
        }

        public void SetToolSslInfo(string type, string value)
        {
            switch (type)
            {
                case "speed":
                    toolSslServerSpeed.Text = value;
                    break;
                case "inbound":
                    toolSslInboundInfo.Text = value;
                    break;
                case "routing":
                    toolSslRoutingRule.Text = value;
                    break;
            }

        }

        public void ScrollToCaret()
        {
            this.txtMsgBox.ScrollToCaret();
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

        private void ssMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!Utils.IsNullOrEmpty(e.ClickedItem.Text))
            {
                Utils.SetClipboardData(e.ClickedItem.Text);
            }
        }
        #endregion


    }
}
