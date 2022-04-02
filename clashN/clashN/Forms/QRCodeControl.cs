using System.Windows.Forms;
using clashN.Handler;
using clashN.Mode;

namespace clashN.Forms
{
    public partial class QRCodeControl : UserControl
    {
        public QRCodeControl()
        {
            InitializeComponent();
        }
        private void QRCodeControl_Load(object sender, System.EventArgs e)
        {
            txtUrl.MouseUp += txtUrl_MouseUp;
        }

        void txtUrl_MouseUp(object sender, MouseEventArgs e)
        {
            txtUrl.SelectAll();
        }

        public void showQRCode(ProfileItem item)
        {
            if (item != null)
            {
                string url = item.url;
                if (Utils.IsNullOrEmpty(url))
                {
                    picQRCode.Image = null;
                    txtUrl.Text = string.Empty;
                    return;
                }
                txtUrl.Text = url;
                picQRCode.Image = QRCodeHelper.GetQRCode(url);
            }
        }
    }
}
