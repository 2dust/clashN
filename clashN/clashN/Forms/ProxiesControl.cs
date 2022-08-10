using clashN.Base;
using clashN.Handler;
using clashN.Mode;
using clashN.Resx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static clashN.Mode.ClashProviders;
using static clashN.Mode.ClashProxies;

namespace clashN.Forms
{
    public partial class ProxiesControl : UserControl
    {
        private Config _config;
        Action<bool, string> _updateFunc;
        private Dictionary<String, ProxiesItem> proxies;
        private Dictionary<String, ProvidersItem> providers;
        private List<ProxiesItem> lstProxies;
        private List<ProxiesItem> lstDetail;
        private int sortColumn = 0;

        //    ProxiesItem selectedProxy;

        #region Init
        public ProxiesControl()
        {
            InitializeComponent();
        }

        private void ProxiesControl_Load(object sender, EventArgs e)
        {

        }

        public void Init(Config config, Action<bool, string> update)
        {
            _config = config;
            _updateFunc = update;
            InitProxiesView();
            InitDetailView();

            //GetClashProxies(true);
            DelayTestTask();
        }

        #endregion

        #region listview
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

            lvProxies.Columns.Add("", 30);
            lvProxies.Columns.Add(ResUI.LvAlias, 150);
            lvProxies.Columns.Add(ResUI.LvServiceType, 100);
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
            lvDetail.Columns.Add(ResUI.LvAlias, 150);
            lvDetail.Columns.Add(ResUI.LvServiceType, 120);
            lvDetail.Columns.Add(ResUI.LvTestResults, 100, HorizontalAlignment.Right);

            lvDetail.EndUpdate();
        }

        private void lvProxies_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDetail(GetLvSelectedIndex());
        }

        private void lvProxies_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column < 0)
            {
                return;
            }

            if (e.Column == 0)
            {
                foreach (ColumnHeader it in lvProxies.Columns)
                {
                    it.Width = -2;
                }
                return;
            }
        }

        private void lvDetail_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column < 0)
            {
                return;
            }

            if (e.Column == 0)
            {
                foreach (ColumnHeader it in lvDetail.Columns)
                {
                    it.Width = -2;
                }
                return;
            }

            RefreshDetailSub(e.Column);
        }

        private void lvDetail_DoubleClick(object sender, EventArgs e)
        {
            SetActiveProxy();
        }
        #endregion


        #region  proxy function

        private void GetClashProxies(bool refreshUI)
        {
            MainFormHandler.Instance.GetClashProxies(_config, (it, it2) =>
            {
                _updateFunc(false, "Refresh proxies");
                proxies = it?.proxies;
                providers = it2?.providers;

                LazyConfig.Instance.SetProxies(proxies);
                if (proxies == null)
                {
                    return;
                }
                if (refreshUI)
                {
                    RefreshProxies();
                }
            });
        }

        private void RefreshProxies()
        {
            lstProxies = new List<ProxiesItem>();

            var proxyGroups = MainFormHandler.Instance.GetClashProxyGroups();
            if (proxyGroups != null && proxyGroups.Count > 0)
            {
                foreach (var it in proxyGroups)
                {
                    if (Utils.IsNullOrEmpty(it.name) || !proxies.ContainsKey(it.name))
                    {
                        continue;
                    }
                    var item = proxies[it.name];
                    if (!Global.allowSelectType.Contains(item.type.ToLower()))
                    {
                        continue;
                    }
                    lstProxies.Add(new ProxiesItem()
                    {
                        now = item.now,
                        name = item.name,
                        type = item.type
                    });
                }
            }
            else
            {
                foreach (KeyValuePair<string, ProxiesItem> kv in proxies)
                {
                    if (!Global.allowSelectType.Contains(kv.Value.type.ToLower()))
                    {
                        continue;
                    }
                    lstProxies.Add(new ProxiesItem()
                    {
                        now = kv.Value.now,
                        name = kv.Key,
                        type = kv.Value.type
                    });
                }
            }

            var index = -1;
            lvProxies.BeginInvoke(new Action(() =>
            {
                index = GetLvSelectedIndex();
                if (index < 0)
                {
                    index = 0;
                }

                lvProxies.BeginUpdate();
                lvProxies.Items.Clear();

                foreach (var item in lstProxies)
                {
                    ListViewItem lvItem = new ListViewItem("");
                    Utils.AddSubItem(lvItem, "Name", item.name);
                    Utils.AddSubItem(lvItem, "Type", item.type);
                    Utils.AddSubItem(lvItem, "Activity", item.now);

                    lvProxies.Items.Add(lvItem);
                    lvItem.Tag = item.name;
                }
                lvProxies.EndUpdate();

                if (index >= 0 && index < lvProxies.Items.Count && lvProxies.Items.Count > 0)
                {
                    lvProxies.Items[index].Selected = true;
                    lvProxies.SetScrollPosition(index);

                }
            }));

            lvDetail.BeginInvoke(new Action(() =>
            {
                RefreshDetail(index);
            }));
        }
        private void RefreshDetail(int index)
        {
            //selectedProxy = null;

            lvDetail.BeginUpdate();
            lvDetail.Items.Clear();
            lvDetail.EndUpdate();

            if (index < 0 || index >= lvProxies.Items.Count)
            {
                return;
            }
            var name = lvProxies.Items[index].Tag.ToString();
            if (Utils.IsNullOrEmpty(name))
            {
                return;
            }
            if (proxies == null)
            {
                return;
            }

            proxies.TryGetValue(name, out ProxiesItem proxy);
            if (proxy == null || proxy.all == null)
            {
                return;
            }
            //selectedProxy = proxy;

            lstDetail = new List<ProxiesItem>();
            foreach (var item in proxy.all)
            {
                var isActive = item == proxy.now;

                var proxy2 = TryGetProxy(item);
                if (proxy2 == null)
                {
                    continue;
                }
                int delay = -1;
                if (proxy2.history.Count > 0)
                {
                    delay = proxy2.history[proxy2.history.Count - 1].delay;
                }
                lstDetail.Add(new ProxiesItem()
                {
                    now = isActive ? Global.CheckMark : "",
                    name = item,
                    type = proxy2.type,
                    delay = delay <= 0 ? 99999999 : delay
                });
            }

            RefreshDetailSub(-1);
        }

        private void RefreshDetailSub(int column)
        {
            if (lstDetail == null)
            {
                return;
            }
            if (column < 0)
            {
                column = sortColumn;
            }
            else
            {
                sortColumn = column;
            }
            lvDetail.BeginUpdate();
            lvDetail.Items.Clear();

            switch (column)
            {
                case 1:
                    lstDetail = lstDetail.OrderBy(x => x.name).ToList();
                    break;
                case 2:
                    lstDetail = lstDetail.OrderBy(x => x.type).ToList();
                    break;
                case 3:
                    lstDetail = lstDetail.OrderBy(x => x.delay).ToList();
                    break;
            }

            foreach (var item in lstDetail)
            {
                ListViewItem lvItem = new ListViewItem(item.now);
                Utils.AddSubItem(lvItem, "Name", item.name);
                Utils.AddSubItem(lvItem, "Type", item.type);
                Utils.AddSubItem(lvItem, "testResult", item.delay >= 99999999 ? "" : item.delay.ToString("#ms"));
                lvItem.Tag = item.name;
                if (item.now == Global.CheckMark)
                {
                    lvItem.ForeColor = Color.DodgerBlue;
                    lvItem.Font = new Font(lvItem.Font, FontStyle.Bold);
                }

                lvDetail.Items.Add(lvItem);
            }

            lvDetail.EndUpdate();
        }

        private ProxiesItem TryGetProxy(string name)
        {
            proxies.TryGetValue(name, out ProxiesItem proxy2);
            if (proxy2 != null)
            {
                return proxy2;
            }
            //from providers
            if (providers != null)
            {
                foreach (KeyValuePair<string, ProvidersItem> kv in providers)
                {
                    if (Global.proxyVehicleType.Contains(kv.Value.vehicleType.ToLower()))
                    {
                        var proxy3 = kv.Value.proxies.FirstOrDefault(t => t.name == name);
                        if (proxy3 != null)
                        {
                            return proxy3;
                        }
                    }
                }
            }
            return null;
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
            var selectedProxy = TryGetProxy(name);
            if (selectedProxy == null || selectedProxy.type != "Selector")
            {
                _updateFunc(false, ResUI.OperationFailed);
                return;
            }

            MainFormHandler.Instance.ClashSetActiveProxy(name, nameNode);

            selectedProxy.now = nameNode;
            RefreshDetail(GetLvSelectedIndex());

            _updateFunc(false, ResUI.OperationSuccess);

            GetClashProxies(true);
        }

        #endregion

        #region toolbar

        public void ProxiesClear()
        {
            proxies = null;
            providers = null;
            lstProxies = null;
            lstDetail = null;
            LazyConfig.Instance.SetProxies(proxies);

            lvProxies.BeginInvoke(new Action(() =>
            {
                lvProxies.Items.Clear();
            }));
            lvDetail.BeginInvoke(new Action(() =>
            {
                lvDetail.Items.Clear();
            }));
        }

        public void ProxiesReload()
        {
            GetClashProxies(true);
            lvProxies.Focus();
        }

        public void ProxiesDelayTest(bool blAll = true)
        {
            if (blAll)
            {
                MainFormHandler.Instance.ClashProxiesDelayTest(it =>
                {
                    _updateFunc(false, "Clash Proxies Latency Test");
                    GetClashProxies(true);
                });
            }
            else
            {
                if (lstDetail == null)
                {
                    return;
                }
                _updateFunc(false, "Clash Proxies Part Latency Test");

                MainFormHandler.Instance.ClashProxiesDelayTestPart(lstDetail, (item, result) =>
                {
                    _updateFunc(false, $"{item.name}={result}");

                    var dicResult = Utils.FromJson<Dictionary<string, object>>(result);
                    if (dicResult.ContainsKey("delay"))
                    {
                        int k = lstDetail.FindIndex(it => it.name == item.name);
                        if (k >= 0 && k < lvDetail.Items.Count)
                        {
                            lvDetail.BeginInvoke(new Action(() =>
                            {
                                lvDetail.Items[k].SubItems["testResult"].Text = $"{dicResult["delay"]}ms";
                            }));
                        }
                    }
                });
            }
        }
        public void ProxiesSelectActivity()
        {
            SetActiveProxy();
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


        private void tsbProxiesReload_Click(object sender, EventArgs e)
        {
            ProxiesReload();
        }

        private void tsbProxiesSpeedtest_Click(object sender, EventArgs e)
        {
            ProxiesDelayTest();
        }

        private void tsbProxiesSelectActivity_Click(object sender, EventArgs e)
        {
            ProxiesSelectActivity();
        }

        private void tsbProxiesSpeedtestPart_Click(object sender, EventArgs e)
        {
            ProxiesDelayTest(false);
        }

        #endregion

        #region task

        public void DelayTestTask()
        {
            Task.Run(() =>
            {
                var autoDelayTestTime = DateTime.Now;

                Thread.Sleep(1000);

                while (true)
                {
                    var dtNow = DateTime.Now;

                    if (_config.autoDelayTestInterval > 0)
                    {
                        if ((dtNow - autoDelayTestTime).Minutes % _config.autoDelayTestInterval == 0)
                        {
                            ProxiesDelayTest();
                            autoDelayTestTime = dtNow;
                        }
                        Thread.Sleep(1000);
                    }

                    Thread.Sleep(1000 * 60);
                }
            }

            );
        }

        #endregion

    }
}