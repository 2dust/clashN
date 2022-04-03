using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using clashN.Handler;
using clashN.Mode;
using clashN.Base;
using clashN.Resx;

namespace clashN.Forms
{
    public partial class AddProfileForm : BaseForm
    {
        public ProfileItem profileItem = null;
        public string groupId;

        public AddProfileForm()
        {
            InitializeComponent();
        }

        private void AddProfileForm_Load(object sender, EventArgs e)
        {
            cmbCoreType.Items.AddRange(Global.coreTypes.ToArray());
            cmbCoreType.Items.Add(string.Empty);

            txtAddress.ReadOnly = true;
            if (profileItem != null)
            {
                BindingProfile();
            }
            else
            {
                profileItem = new ProfileItem();
                profileItem.groupId = groupId;
                profileItem.enabled = true;
                ClearProfile();
            }
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindingProfile()
        {
            txtRemarks.Text = profileItem.remarks;
            txtAddress.Text = profileItem.address;
            txtUrl.Text = profileItem.url;
            chkEnabled.Checked = profileItem.enabled;
            txtUserAgent.Text = profileItem.userAgent;
            chkEnableTun.Checked = profileItem.enableTun;

            if (profileItem.coreType == null)
            {
                cmbCoreType.Text = string.Empty;
            }
            else
            {
                cmbCoreType.Text = profileItem.coreType.ToString();
            }
        }


        /// <summary>
        /// 清除设置
        /// </summary>
        private void ClearProfile()
        {
            txtRemarks.Text = "";
            chkEnabled.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string remarks = txtRemarks.Text;
            if (Utils.IsNullOrEmpty(remarks))
            {
                UI.Show(ResUI.PleaseFillRemarks);
                return;
            }
            if (Utils.IsNullOrEmpty(txtAddress.Text) && Utils.IsNullOrEmpty(txtUrl.Text))
            {
                UI.Show(ResUI.FillProfileAddressCustom);
                return;
            }
            profileItem.remarks = remarks;
            profileItem.url = txtUrl.Text.TrimEx();
            profileItem.enabled = chkEnabled.Checked;
            profileItem.userAgent = txtUserAgent.Text.TrimEx();
            profileItem.enableTun = chkEnableTun.Checked;

            if (profileItem.enableTun)
            {
                profileItem.coreType = ECoreType.clash_meta;
                if (!Utils.IsAdministrator())
                {
                    UI.Show(ResUI.RunAsAdmin);
                }
            }
            else if (Utils.IsNullOrEmpty(cmbCoreType.Text))
            {
                profileItem.coreType = null;
            }
            else
            {
                profileItem.coreType = (ECoreType)Enum.Parse(typeof(ECoreType), cmbCoreType.Text);
            }

            if (ConfigHandler.EditProfile(ref config, profileItem) == 0)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                UI.ShowWarning(ResUI.OperationFailed);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Utils.IsNullOrEmpty(profileItem.indexId))
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "YAML|*.yaml|All|*.*"
            };
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string fileName = fileDialog.FileName;
            if (Utils.IsNullOrEmpty(fileName))
            {
                return;
            }

            profileItem.remarks = txtRemarks.Text;

            if (ConfigHandler.AddProfileViaPath(ref config, profileItem, fileName) == 0)
            {
                BindingProfile();
                UI.Show(ResUI.SuccessfullyImportedCustomProfile);
            }
            else
            {
                UI.ShowWarning(ResUI.FailedImportedCustomProfile);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var address = txtAddress.Text;
            if (Utils.IsNullOrEmpty(address))
            {
                UI.Show(ResUI.FillProfileAddressCustom);
                return;
            }

            address = Path.Combine(Utils.GetConfigPath(), address);
            Utils.ProcessStart(address);
        }
    }
}
