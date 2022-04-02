using clashN.Base;
using clashN.Handler;
using clashN.Mode;
using clashN.Resx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static clashN.Mode.ClashProxies;

namespace clashN.Forms
{
    public partial class ProxiesForm : BaseForm
    {
        Dictionary<String, ProxiesItem> proxies;
        ProxiesItem selectedProxy;

        public ProxiesForm()
        {
            InitializeComponent();
        }

        private void ProxiesForm_Load(object sender, EventArgs e)
        {
            InitProxiesView();
            InitDetailView();

            GetClashProxies(true);
        }

        private void ProxiesForm_Shown(object sender, EventArgs e)
        {
            lvProxies.Focus();
        }
        private void InitProxiesView()
        {
            lvProxies.BeginUpdate();
            lvProxies.Items.Clear();

            lvProxies.GridLines = true;
            lvProxies.FullRowSelect = true;
            lvProxies.View = View.Details;
            lvProxies.Scrollable = true;
            lvProxies.MultiSelect = false;
            lvProxies.HeaderStyle = ColumnHeaderStyle.Clickable;

            lvProxies.Columns.Add(ResUI.LvAlias, 130);
            lvProxies.Columns.Add(ResUI.LvServiceType, 80);
            lvProxies.Columns.Add(ResUI.LvActivity, 130);

            lvProxies.EndUpdate();
        }

        private void InitDetailView()
        {
            lvDetail.BeginUpdate();
            lvDetail.Items.Clear();

            lvDetail.GridLines = true;
            lvDetail.FullRowSelect = true;
            lvDetail.View = View.Details;
            lvDetail.Scrollable = true;
            lvDetail.MultiSelect = false;
            lvDetail.HeaderStyle = ColumnHeaderStyle.Clickable;

            lvDetail.Columns.Add("", 30);
            lvDetail.Columns.Add(ResUI.LvAlias, 130);
            lvDetail.Columns.Add(ResUI.LvServiceType, 80);
            lvDetail.Columns.Add(ResUI.LvTestResults, 60, HorizontalAlignment.Right);

            lvDetail.EndUpdate();
        }

        private void GetClashProxies(bool refreshUI)
        {
            MainFormHandler.Instance.GetClashProxies(config, it =>
            {
                proxies = it.proxies;
                LazyConfig.Instance.SetProxies(proxies);
                if (refreshUI)
                {
                    RefreshProxies();
                }
            });
        }

        private void RefreshProxies()
        {
            lvProxies.BeginInvoke(new Action(() =>
            {
                lvProxies.BeginUpdate();
                lvProxies.Items.Clear();

                foreach (KeyValuePair<string, ProxiesItem> kv in proxies)
                {
                    if (kv.Value.type != "Selector" && kv.Value.type != "URLTest")
                    {
                        continue;
                    }
                    ListViewItem lvItem = new ListViewItem(kv.Key);
                    Utils.AddSubItem(lvItem, "Type", kv.Value.type);
                    Utils.AddSubItem(lvItem, "Activity", kv.Value.now);

                    lvProxies.Items.Add(lvItem);
                    lvItem.Tag = kv.Key;
                }
                lvProxies.EndUpdate();

            }));
            selectedProxy = null;
            lvDetail.BeginInvoke(new Action(() =>
            {
                lvDetail.BeginUpdate();
                lvDetail.Items.Clear();
                lvDetail.EndUpdate();
            }));
        }
        private void RefreshDetail()
        {
            selectedProxy = null;

            lvDetail.BeginUpdate();
            lvDetail.Items.Clear();
            lvDetail.EndUpdate();

            int index = GetLvSelectedIndex();
            if (index < 0)
            {
                return;
            }

            var name = lvProxies.Items[index].Tag.ToString();
            if (Utils.IsNullOrEmpty(name))
            {
                return;
            }

            proxies.TryGetValue(name, out ProxiesItem proxy);
            if (proxy == null || proxy.all == null)
            {
                return;
            }
            selectedProxy = proxy;

            lvDetail.BeginUpdate();
            lvDetail.Items.Clear();

            foreach (var item in proxy.all)
            {
                var isActive = item == proxy.now;
                proxies.TryGetValue(item, out ProxiesItem proxy2);
                if (proxy2 == null)
                {
                    continue;
                }
                var testResult = string.Empty;
                if (proxy2.history.Count > 0)
                {
                    testResult = proxy2.history[proxy2.history.Count - 1].delay.ToString() + "ms";
                }

                ListViewItem lvItem = new ListViewItem(isActive ? "√" : "");
                Utils.AddSubItem(lvItem, "Name", item);
                Utils.AddSubItem(lvItem, "Type", proxy2.type);
                Utils.AddSubItem(lvItem, "testResult", testResult);
                lvItem.Tag = item;
                if (isActive)
                {
                    lvItem.ForeColor = Color.DodgerBlue;
                    lvItem.Font = new Font(lvItem.Font, FontStyle.Bold);
                }

                lvDetail.Items.Add(lvItem);
            }
            lvDetail.EndUpdate();
        }

        private void lvProxies_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDetail();
        }
        private void lvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    SetActiveProxy();
                    break;
            }

        }
        private void SetActiveProxy()
        {
            if (lvProxies.SelectedItems == null || lvProxies.SelectedItems.Count <= 0)
            {
                return;
            }
            var name = lvProxies.SelectedItems[0].Tag.ToString();
            if (Utils.IsNullOrEmpty(name))
            {
                return;
            }
            if (lvDetail.SelectedItems == null || lvDetail.SelectedItems.Count <= 0)
            {
                return;
            }
            var nameNode = lvDetail.SelectedItems[0].Tag.ToString();
            if (Utils.IsNullOrEmpty(nameNode))
            {
                return;
            }
            if (selectedProxy.type != "Selector")
            {
                UI.Show(ResUI.OperationFailed);
                return;
            }

            var url = $"{Global.httpProtocol}{Global.Loopback}:{config.APIPort}/proxies/{name}";
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("name", nameNode);
            _ = HttpClientHelper.GetInstance().PutAsync(url, headers);

            selectedProxy.now = nameNode;
            RefreshDetail();
        }

        private void tsbReload_Click(object sender, EventArgs e)
        {
            GetClashProxies(true);
        }

        private void tsbSpeedtest_Click(object sender, EventArgs e)
        {
            if (proxies == null)
            {
                return;
            }
            MainFormHandler.Instance.ClashProxiesDelayTest();

            UI.Show(ResUI.OperationSuccess);

            GetClashProxies(false);

        }
        private void tsbSelectActivity_Click(object sender, EventArgs e)
        {
            SetActiveProxy();
        }
        private void tsbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private int GetLvSelectedIndex()
        {
            int index = -1;
            try
            {
                if (lvProxies.SelectedIndices.Count <= 0)
                {
                    return index;
                }

                index = lvProxies.SelectedIndices[0];
                return index;
            }
            catch
            {
                return index;
            }
        }

    }
}
